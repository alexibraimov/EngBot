using EngBotApp.Commands;
using EngBotApp.Models;
using EngBotApp.Models.Contexts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EngBotApp.UseCases
{
    public class BotUseCase : IDisposable
    {
        private ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();
        private Models.Contexts.IRepository<UserInfo> _repository;
        private bool _isDisposed;
        private ITelegramBotClient _botClient;
        private WordMachine _wordMachine;
        public BotUseCase()
        {
            _repository = new UserRepository();
            _wordMachine = new WordMachine(_repository);
            _wordMachine.OnWord += OnWord;
        }

        private void OnWord(long chatId, UserWord word)
        {
            _commands.Enqueue(new WordCommand(_botClient, _repository, chatId, word, CancellationToken.None));
        }

        public void Setup(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            new Thread(()=> 
            {
                while (!_isDisposed)
                {
                    if (_commands.TryDequeue(out ICommand command))
                    {
                        if (command.CanExecute())
                        {
                            command.Execute();
                            if (command.Command == "accept_schedule")
                            {
                                Thread.Sleep(500);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }) { Name = "Telegram bot thread" }.Start();
        }

        public void SendMessage(ITelegramBotClient botClient, long chatId, string username, string message, CancellationToken cancellationToken)
        {
            if (!_repository.Contains(chatId))
            {
                _repository.Save(UserInfo.Create(chatId, username));
            }

            var userInfo = _repository.FindById(chatId);
            if (userInfo == null)
            {
                return;
            }
            if (userInfo.LastMessageId != 0)
            {
                _commands.Enqueue(new AcceptScheduleCommand(botClient, _repository, chatId, cancellationToken));
            }
            switch (message)
            {
                case "/start":
                    {
                        _commands.Enqueue(new StartCommand(botClient,  _repository, chatId, cancellationToken));
                    }
                    break;
                //case "/set_schedule":
                //    {
                //        _commands.Enqueue(new SetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                //    }
                //    break;
                //case "/get_schedule":
                //    {
                //        _commands.Enqueue(new GetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                //    }
                //    break;
                case "/setup":
                    {
                        _commands.Enqueue(new TimezoneCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
                default:
                    break;
            }
        }

        public void CallbackQuery(ITelegramBotClient botClient, long chatId, string username, string data, CancellationToken cancellationToken)
        {
            var dataQuery = JsonConvert.DeserializeObject<JObject>(data);
            var userInfo = _repository.FindById(chatId);
            if (userInfo == null)
                return;
            switch (dataQuery["type"].ToString())
            {
                case "schedule":
                    {
                        var time = (int) dataQuery["data"]["hour"];
                        var isEnabled = (bool) dataQuery["data"]["isEnabled"];

                        if (!isEnabled)
                        {

                            userInfo.Schedule.Add(TimeSpan.FromHours(time));
                        }
                        else
                        {

                            userInfo.Schedule.Remove(TimeSpan.FromHours(time));
                        }

                        _commands.Enqueue(new SetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
                case "accept_schedule":
                    {
                        _commands.Enqueue(new GetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
                case "change_schedule":
                    {
                        _commands.Enqueue(new SetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
                case "timezone":
                    {
                        var time = (TimeSpan)dataQuery["data"]["time"];
                        userInfo.Timezone = time;
                        _repository.Save(userInfo);
                    }
                    break;
                case "accept_timezone":
                    {
                        _commands.Enqueue(new SetScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _commands = null;
            _wordMachine.Dispose();
        }
    }
}
