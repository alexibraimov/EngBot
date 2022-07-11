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
        public StartCommand(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken) 
            : base(bot, chatId, cancellationToken) { }

        public override string Command => "start";

        public override bool CanExecute()
        {
            return true;
        }

        public override async void Execute()
        {
            await _bot.SendTextMessageAsync(
               chatId: _chatId,
               text: "Добро пожаловать!" ,
               cancellationToken: _cancellationToken);
        }
    
    }
}
