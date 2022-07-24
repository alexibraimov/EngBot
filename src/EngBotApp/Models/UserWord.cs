using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class UserWord
    {
        [JsonProperty("wordId")]
        public long WordId { get; set; }   
        [JsonProperty("en")]
        public string En { get; set; }
        [JsonProperty("ru")]
        public string Ru { get; set; }
        [JsonProperty("isRemembered")]
        public bool IsRemembered { get; set; }
    }
}
