﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EngBotApp.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected ITelegramBotClient _bot;
        protected long _chatId;
        protected CancellationToken _cancellationToken;
        public BaseCommand(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
        {
            _bot = bot;
            _chatId = chatId;
            _cancellationToken = cancellationToken;
        }

        public abstract string Command { get; }

        public abstract bool CanExecute();

        public abstract void Execute();

        public bool IsCommad(string command) => !string.IsNullOrEmpty(command) && command.ToLower().Contains(Command.ToLower());
    }
}
