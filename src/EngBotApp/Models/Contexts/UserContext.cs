using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models.Contexts
{
    public class UserContext
    {
        private string _currentDirectory;
        private Dictionary<long, UserInfo> _users;

        public UserContext()
        {
            _users = new Dictionary<long, UserInfo>();
            _currentDirectory = Path.Combine(Environment.CurrentDirectory, "Chats");
            Directory.CreateDirectory(_currentDirectory);
            Init();
        }

        private void Init()
        {
            var files = Directory.GetFiles(_currentDirectory);

            foreach (var file in files)
            {
                using (var sw = new StreamReader(file))
                {
                    try
                    {
                        var userInfo = JsonConvert.DeserializeObject<UserInfo>(sw.ReadToEnd());
                        if (userInfo == null)
                        {
                            continue;
                        }
                        if (_users.ContainsKey(userInfo.ChatId))
                        {
                            _users[userInfo.ChatId] = userInfo;
                        }
                        else
                        {
                            _users.Add(userInfo.ChatId, userInfo);
                        }
                    }
                    catch 
                    {
                    }
                }
            }
        }

        public bool Contains(long chatId)
        {
            return _users != null && _users.ContainsKey(chatId);
        }

        public IList<UserInfo> GetAll()
        {
            return _users.Values.ToList();
        }

        public UserInfo Get(long chatId)
        {
            return _users.ContainsKey(chatId) ? _users[chatId] : null;
        }

        public void Save(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return;
            }

            var path = Path.Combine(_currentDirectory, $"chat{userInfo.ChatId}.json");
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine(userInfo.ToString());
            }
            _users[userInfo.ChatId] = userInfo;
        }
    }
}
