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
    public class EngUseCase : IDisposable
    {
        private ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();
        private Models.Contexts.IRepository<UserInfo> _repository;
        private bool _isDisposed;

        public EngUseCase()
        {
            _repository = new UserRepository();
        }

        public void Setup()
        {
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
        }

        public void SendMessage(ITelegramBotClient botClient, long chatId, string username, string message, CancellationToken cancellationToken)
        {
            if (!_repository.Contains(chatId))
            {
                _repository.Save(UserInfo.Create(chatId, username));
            }

            var userInfo = _repository.FindById(chatId);

            switch (message)
            {
                case "/start":
                    {
                        _commands.Enqueue(new StartCommand(botClient,  _repository, chatId, cancellationToken));
                        userInfo.MessageId = 0;
                        _repository.Save(userInfo);
                    }
                    break;
                case "/set_schedule":
                    {
                        _commands.Enqueue(new ScheduleCommand(botClient, _repository, chatId, cancellationToken));
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

                        _commands.Enqueue(new ScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
                case "accept_schedule":
                    {
                        _commands.Enqueue(new AcceptScheduleCommand(botClient, _repository, chatId, cancellationToken));
                    }
                    break;
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _commands = null;
        }
    }
}
