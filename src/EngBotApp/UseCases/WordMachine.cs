using EngBotApp.Helpers;
using EngBotApp.Models;
using EngBotApp.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EngBotApp.UseCases
{
    public class WordMachine : IDisposable
    {
        public event Action<long, UserWord> OnWord;
        private bool _isDispose;
        private IRepository<UserInfo> _repository;
        public WordMachine(IRepository<UserInfo> repository)
        {
            _repository = repository;
            new Thread(MachineProcess)
            { Name = "Words thread" }.Start();
        }

        private void MachineProcess()
        {
            while (!_isDispose)
            {
                try
                {
                    var currentTime = DateTime.UtcNow;

                    var users = _repository.GetAll().ToArray();

                    foreach (var user in users)
                    {
                        if (user.LastUpdateDate == null || currentTime.Hour != user.LastUpdateDate.Value.Hour)
                        {
                            foreach (var time in user.Schedule)
                            {
                                if (currentTime.Hour == time.Hours)
                                {
                                    var words = user.Words.ToArray();
                                    var word = words[RandomHelper.Random.Next(0, words.Length - 1)];

                                    user.LastUpdateDate = currentTime;
                                    user.Words.Remove(word);
                                    user.RememderedWords.Add(word);

                                    _repository.Save(user);

                                    OnWord?.Invoke(user.ChatId, word);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Thread.Sleep(10000);
                }
            }
        }


        public void Dispose()
        {
            _isDispose = true;
        }
    }
}
