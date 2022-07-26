using EngBotApp.Models;
using EngBotApp.Models.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EngBotApp.Commands
{
    public class SetScheduleCommand : BaseCommand
    {
        private static IDictionary<string, int> _times = new Dictionary<string, int>()
        {
           ["01:00"] = 1,
           ["02:00"] = 2,
           ["03:00"] = 3,
           ["04:00"] = 4,
           ["05:00"] = 5,
           ["06:00"] = 6,
           ["07:00"] = 7,
           ["08:00"] = 8,
           ["09:00"] = 9,
           ["10:00"] = 10,
           ["11:00"] = 11,
           ["12:00"] = 12,
           ["13:00"] = 13,
           ["14:00"] = 14,
           ["15:00"] = 15,
           ["16:00"] = 16,
           ["17:00"] = 17,
           ["18:00"] = 18,
           ["19:00"] = 19,
           ["20:00"] = 20,
           ["21:00"] = 21,
           ["22:00"] = 22,
           ["23:00"] = 23,
           ["00:00"] = 0,
        };

        public SetScheduleCommand(ITelegramBotClient bot, IRepository<UserInfo> repository, long chatId, CancellationToken cancellationToken)
            : base(bot, repository, chatId, cancellationToken)
        {

        }

        public override string Command => "set_schedule";

        public override bool CanExecute()
        {
            return true;
        }

        public async override void Execute()
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            var userInfo = _repository.FindById(_chatId);
            var userTimes = userInfo.Schedule.Select(time=>time.ToString(@"hh\:mm"));
            foreach (var time in _times)
            {
                var isEnabled = userTimes.Contains(time.Key);
                var timeStr = time.Key + (isEnabled ? " ✓" : string.Empty);
                keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(text: timeStr, callbackData: JsonConvert.SerializeObject(new Button() { Type = "schedule", Data = new Newtonsoft.Json.Linq.JObject() { ["hour"] = time.Value, ["isEnabled"] = isEnabled } })) });   
            }
            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(text: "Принять", callbackData: JsonConvert.SerializeObject(new Button() { Type = "accept_schedule", Data = null })) });
            Message message;
            if (userInfo.MessageId == 0)
            {
                 message = await _bot.SendTextMessageAsync(
                   chatId: _chatId,
                   text: "<b>Выберите расписание: (UTC)</b>",
                   parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                   replyMarkup: new InlineKeyboardMarkup(keyboard),
                   cancellationToken: _cancellationToken);
            }
            else
            {
                 message = await _bot.EditMessageTextAsync(
                      chatId: _chatId,
                      messageId: userInfo.MessageId,
                      text: "<b>Выберите расписание: (UTC)</b>",
                      parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                      replyMarkup: new InlineKeyboardMarkup(keyboard),
                      cancellationToken: _cancellationToken);
            }

            if (message != null)
            {
                userInfo.MessageId = message.MessageId;
                _repository.Save(userInfo);
            }
        }
    }
}
