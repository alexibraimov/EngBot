using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace EngBotApp.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        public ScheduleCommand(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
            : base(bot, chatId, cancellationToken)
        {

        }

        public override string Command => "set_schedule";

        public override bool CanExecute()
        {
            return true;
        }

        public async override void Execute()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[] {
                new [] {InlineKeyboardButton.WithCallbackData(text: "01:00", callbackData: "{\"type\":\"schedule\", \"data\":\"01:00\"}"),
                       InlineKeyboardButton.WithCallbackData(text: "-", callbackData: "{\"type\":\"schedule\", \"data\":\"01:00\"}")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "02:00", callbackData: "2")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "03:00", callbackData: "3")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "04:00", callbackData: "4")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "05:00", callbackData: "5")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "06:00", callbackData: "6")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "07:00", callbackData: "7")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "08:00", callbackData: "8")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "09:00", callbackData: "9")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "10:00", callbackData: "10")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "11:00", callbackData: "11")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "12:00", callbackData: "12")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "13:00", callbackData: "13")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "14:00", callbackData: "14")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "15:00", callbackData: "15")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "16:00", callbackData: "16")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "17:00", callbackData: "17")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "18:00", callbackData: "18")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "19:00", callbackData: "19")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "20:00", callbackData: "20")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "21:00", callbackData: "21")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "22:00", callbackData: "22")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "23:00", callbackData: "23")},
                new [] {InlineKeyboardButton.WithCallbackData(text: "00:00", callbackData: "24")},
            });

            await _bot.SendTextMessageAsync(
              chatId: _chatId,
              text: "Выберите расписание:",
              replyMarkup: inlineKeyboard,
              cancellationToken: _cancellationToken);
        }
    }
}
