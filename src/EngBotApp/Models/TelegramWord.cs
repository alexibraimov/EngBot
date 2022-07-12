using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class TelegramWord
    {
        [JsonProperty("id")]
        public long TelegramWordId { get; set; }
        [JsonProperty("wordId")]
        public int WordId { get; set; }
        
        [JsonProperty("en")]
        public string En { get; set; }
        [JsonProperty("ru")]
        public string Ru { get; set; }
        [JsonProperty("isRemembered")]
        public bool IsRemembered { get; set; }

        public long TelegramUserId { get; set; }
        [JsonProperty("user")]
        public TelegramUser TelegramUser { get; set; }
    }
}
