using EngBotApp.Models;
using EngBotApp.Models.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace EngBotApp.Commands
{
    public class TimezoneCommand : BaseCommand
    {

        public TimezoneCommand(ITelegramBotClient bot, IRepository<UserInfo> repository, long chatId, CancellationToken cancellationToken)
            : base(bot, repository, chatId, cancellationToken)
        {

        }

        public override string Command => "timezone";

        public override bool CanExecute()
        {
            return true;
        }

        public async override void Execute()
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones().Select(x=>x.BaseUtcOffset).Distinct().ToArray();

            var keyboard = new List<InlineKeyboardButton[]>();
            var userInfo = _repository.FindById(_chatId);
            var userTimes = userInfo.Schedule.Select(time => time.ToString(@"hh\:mm"));

            var rowCount = timezones.Length / 5 + (timezones.Length % 5 == 0 ? 0:1);
            var columnCount = 5;
            var index = 0;
            for (int row = 0; row < rowCount; row++)
            {
                var rows = new List<InlineKeyboardButton>();
                for (int column = 0; column < columnCount; column++)
                {
                    if (index < timezones.Length)
                    {
                        var time = (timezones[index] < TimeSpan.Zero ? "-" : "+") + timezones[index].ToString(@"hh\:mm");
                        rows.Add(InlineKeyboardButton.WithCallbackData(text: time, callbackData: JsonConvert.SerializeObject(new Button() { Type = "timezone", Data = new Newtonsoft.Json.Linq.JObject() { ["time"] = timezones[index] } })));
                        index++;
                    }
                }

                keyboard.Add(rows.ToArray());
            }


            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(text: "Принять", callbackData: JsonConvert.SerializeObject(new Button() { Type = "accept_timezone", Data = null })) });


            var message = await _bot.SendTextMessageAsync(
                    chatId: _chatId,
                    text: "Выберите таймзону",
                    replyMarkup: new InlineKeyboardMarkup(keyboard),
                    cancellationToken: _cancellationToken);

            userInfo.LastMessageId = message.MessageId;
            _repository.Save(userInfo);
        }
    }
}
