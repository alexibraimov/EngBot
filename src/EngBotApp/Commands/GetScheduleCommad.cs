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
    public class GetScheduleCommand : BaseCommand
    {
        public GetScheduleCommand(ITelegramBotClient bot, IRepository<UserInfo> repository, long chatId, CancellationToken cancellationToken)
            : base(bot, repository, chatId, cancellationToken)
        {

        }

        public override string Command => "get_schedule";

        public override bool CanExecute()
        {
            return true;
        }

        public async override void Execute()
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            var userInfo = _repository.FindById(_chatId);

            if (userInfo == null)
            {
                return;
            }

            var text = "<b>Ваше расписание:</b>\n";

            foreach (var time in userInfo.Schedule.OrderBy(x=> 
            {
                if (x == TimeSpan.Zero)
                    return TimeSpan.FromHours(24);
                return x;
            }).Select(time => time.ToString(@"hh\:mm")))
            {
                text += $"\t{time}\n";
            }

            keyboard.Add(new[] { InlineKeyboardButton.WithCallbackData(text: "Изменить", callbackData: JsonConvert.SerializeObject(new Button() { Type = "change_schedule", Data = null })) });

            Message message;
            if (userInfo.LastMessageId == 0)
            {
                message = await _bot.SendTextMessageAsync(
                          chatId: _chatId,
                          text: text,
                          parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                          replyMarkup: new InlineKeyboardMarkup(keyboard),
                          cancellationToken: _cancellationToken);
            }
            else
            {
                message = await _bot.EditMessageTextAsync(
                     chatId: _chatId,
                     messageId: userInfo.LastMessageId,
                     text: text,
                     parseMode : Telegram.Bot.Types.Enums.ParseMode.Html,
                     replyMarkup: new InlineKeyboardMarkup(keyboard),
                     cancellationToken: _cancellationToken);
            }

            userInfo.LastMessageId = message.MessageId;
            userInfo.IsSetup = true;
            _repository.Save(userInfo);
        }
    }
}
