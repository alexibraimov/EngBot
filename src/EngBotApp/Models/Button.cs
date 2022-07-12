using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class Button
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }
    }
}
