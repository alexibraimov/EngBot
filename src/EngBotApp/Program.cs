using EngBotApp.Files;
using EngBotApp.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EngBotApp
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var words = WordCollection.All;
            try
            {
                var bot = new BotWrapper().Startup("{token}");
                Console.ReadLine();
                bot.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}