using EngBotApp.Commands;
using EngBotApp.Models.Contexts;
using EngBotApp.UseCases;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EngBotApp
{
    public class BotWrapper : IDisposable
    {
        private CancellationTokenSource _cts;
        private BotUseCase _engUseCase;
        public BotWrapper()
        {
        }

        public BotWrapper Startup(string token) 
        {
            Initialize(token).Wait();
            return this;
        }

        private async Task Initialize(string token)
        {
            var botClient = new TelegramBotClient(token);

            _cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(updateHandler: HandleUpdateAsync,
                                     pollingErrorHandler: HandlePollingErrorAsync,
                                     receiverOptions: null,
                                     cancellationToken: _cts.Token);

            var me = await botClient.GetMeAsync();

            _engUseCase = new BotUseCase();
            _engUseCase.Setup(botClient);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update == null)
            {
                return;
            }
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;

                        if (message == null)
                        {
                            return;
                        }

                        var chatId = message.Chat.Id;
                        var username = message.Chat.Username;

                        _engUseCase.SendMessage(botClient, chatId, username, message.Text, _cts.Token);
                    }
                    break;
                case UpdateType.CallbackQuery:
                    {
                        var callback = update.CallbackQuery;
                        if (callback == null || callback.Message == null || callback.Message.Chat == null)
                        {
                            return;
                        }
                        var chatId = update.CallbackQuery.Message.Chat.Id;
                        var username = update.CallbackQuery.Message.Chat.Username;

                        _engUseCase.CallbackQuery(botClient, chatId, username, callback.Data, _cts.Token);             
                    }
                    break;
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _engUseCase.Dispose();
        }
    }
}
