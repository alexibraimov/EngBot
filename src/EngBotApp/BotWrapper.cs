using EngBotApp.Commands;
using EngBotApp.Models.Contexts;
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
        private bool _isDisposed;
        private CancellationTokenSource _cts;
        private ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();
        public BotWrapper()
        {
        }

        public BotWrapper Startup(string token) 
        {
            Initialize(token).Wait();
            Task.Run(() =>
            {
                while (!_isDisposed)
                {
                    if (_commands.TryDequeue(out ICommand command))
                    {
                        if (command.CanExecute())
                        {
                            command.Execute();
                        }
                    }
                }
            });
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
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
          
            // Only process Message updates: https://core.telegram.org/bots/api#message
            Message message = update.Message;

            if (update.Type == UpdateType.CallbackQuery)
            {
                var callback = update.CallbackQuery;
                if (callback!=null)
                {
                    switch (callback.Data)
                    {
                        default:
                            break;
                    }
                }
                return;
            }
            if (update.Message == null)
                return;
            using (var db = new TelegramContext())
            {
                var tUser = new Models.TelegramUser()
                {
                    ChatId = update.Message.Chat.Id,
                    Username = update.Message.Chat.Username,
                    LastName = update.Message.Chat.LastName,
                    FirstName = update.Message.Chat.FirstName,
                };
                db.Add(tUser);
                foreach (var item in db.GetAllWords(tUser))
                {
                    db.Add(item);
                }
                db.SaveChanges();
            }
            using (var db = new TelegramContext())
            {
                var u = db.Users.First();

            }
            var messageText = update.Message.Text;
            if (update.Message.Text == null)
                return;


            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            switch (message.Text)
            {
                case "/start":
                    {
                        _commands.Enqueue(new StartCommand(botClient, chatId, cancellationToken));
                    }
                    break;
                case "/set_schedule":
                    {
                        _commands.Enqueue(new ScheduleCommand(botClient, chatId, cancellationToken));
                    }
                    break;
                default:
                    break;
            }

        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _isDisposed = true;
            _cts?.Cancel();
        }
    }
}
