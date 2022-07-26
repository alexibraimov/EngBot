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
    public class AcceptScheduleCommand : BaseCommand
    {
        public AcceptScheduleCommand(ITelegramBotClient bot, IRepository<UserInfo> repository, long chatId, CancellationToken cancellationToken)
            : base(bot, repository, chatId, cancellationToken)
        {

        }

        public override string Command => "accept_schedule";

        public override bool CanExecute()
        {
            return true;
        }

        public async override void Execute()
        {
            var userInfo = _repository.FindById(_chatId);

            if (userInfo == null)
            {
                return;
            }

            var messageId = userInfo.LastMessageId;
            if (messageId == 0)
            {
                return;
            }
            userInfo.LastMessageId = 0;
            _repository.Save(userInfo);

            try
            {
                await _bot.DeleteMessageAsync(
                          chatId: _chatId,
                          messageId: messageId,
                          cancellationToken: _cancellationToken);
            }
            catch
            {

            }
        }
    }
}
