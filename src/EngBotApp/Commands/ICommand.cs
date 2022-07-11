using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EngBotApp.Commands
{
    public interface ICommand
    {
        bool IsCommad(string command);

        string Command { get; }

        void Execute();

        bool CanExecute();

    }
}
