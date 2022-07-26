using EngBotApp.Files;
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
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace EngBotApp.Commands
{
    public class WordCommand : BaseCommand
    {
        private UserWord _userWord;
        public WordCommand(ITelegramBotClient bot, IRepository<UserInfo> repository, long chatId, UserWord userWord, CancellationToken cancellationToken)
            : base(bot, repository, chatId, cancellationToken)
        {
            _userWord = userWord;
        }

        public override string Command => "word_command";

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

            try
            {
                await _bot.SendTextMessageAsync(chatId: _chatId, $"<b>{_userWord.ToString()}</b>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: _cancellationToken);
                using (var stream = VoiceCollection.GetStream(_userWord.WordId))
                {
                    await _bot.SendVoiceAsync(
                        chatId: _chatId,
                        voice: stream,
                        duration: (int) VoiceCollection.GetSoundDuration(_userWord.WordId).TotalSeconds,
                        disableNotification: true,
                        cancellationToken: _cancellationToken); 
                }
            }
            catch
            {

            }
        }
    }
}
