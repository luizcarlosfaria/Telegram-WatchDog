using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace Telegram.WatchDog.Commands
{
    public interface ICommand
    {
        public string Command { get; }

        public string Description { get; }

        void Handle(Message message);
    }
}
