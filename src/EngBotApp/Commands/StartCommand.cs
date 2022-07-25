using EngBotApp.Models;
using EngBotApp.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EngBotApp.Commands
{
    public class StartCommand : BaseCommand
    {
        public StartCommand(ITelegramBotClient bot, Models.Contexts.IRepository<UserInfo> repository, long chatId, CancellationToken cancellationToken) 
            : base(bot, repository, chatId, cancellationToken) { }

        public override string Command => "start";

        public override bool CanExecute()
        {
            return true;
        }

        public override async void Execute()
        {
            var text = "Вас привествует EngBot!\n";
            text += "Список команд:\n";
            text += "/set_schedule\n";
            text += "/get_schedule";
            await _bot.SendTextMessageAsync(
               chatId: _chatId,
               text: text,
               cancellationToken: _cancellationToken);
        }
    
    }
}
