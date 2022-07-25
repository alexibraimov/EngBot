using EngBotApp.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class UserInfo
    {
        public UserInfo(long chatId, string userName)
        {
            ChatId = chatId;
            Username = userName;
            IsEnabled = true;
            Words = new List<UserWord>();
            Schedule = new List<TimeSpan>();
            RememderedWords = new List<UserWord>();
        }

        [JsonProperty("chatId")]
        public long ChatId { get; set; }
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }
        [JsonProperty("words")]
        public List<UserWord> Words {get; set;}
        [JsonProperty("rememderedWords")]
        public List<UserWord> RememderedWords { get; set; }
        [JsonProperty("schedule")]
        public List<TimeSpan> Schedule { get; set; }
        [JsonProperty("messageId")]
        public int MessageId { get; set; }
        [JsonProperty("lastUpdateDate")]
        public DateTime? LastUpdateDate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


        public static UserInfo Create(long id, string username)
        {
            var userInfo = new UserInfo(id, username);

            userInfo.Schedule = new List<TimeSpan>()
            {
                TimeSpan.FromHours(9),
                TimeSpan.FromHours(10),
                TimeSpan.FromHours(11),
                TimeSpan.FromHours(12),
                TimeSpan.FromHours(13),
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TimeSpan.FromHours(16),
                TimeSpan.FromHours(17),
                TimeSpan.FromHours(18),
                TimeSpan.FromHours(19),
                TimeSpan.FromHours(20),
                TimeSpan.FromHours(21)
            };

            userInfo.Words = WordCollection.All.Select(w => new UserWord() 
            {
                WordId = w.Id,
                En = w.En,
                Ru = w.Ru,
            }).ToList();

            return userInfo;
        }
    }
}
