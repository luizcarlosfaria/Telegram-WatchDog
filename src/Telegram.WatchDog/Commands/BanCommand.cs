using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.WatchDog.Services;

namespace Telegram.WatchDog.Commands
{
    public class BanCommand : ICommand
    {
        private readonly AclService aclService;

        public string Command => "/ban";

        public string Description => "Bane um usuário";


        public BanCommand(AclService aclService)
        {
            this.aclService = aclService;
        }

    }
}
