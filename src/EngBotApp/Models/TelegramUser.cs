using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class TelegramUser
    {
        [JsonProperty("id")]
        public long TelegramUserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("chatId")]
        public long ChatId { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("words")]
        public ICollection<TelegramWord> Words { get; set; }
    }
}
