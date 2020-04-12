using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace Telegram.WatchDog.Commands
{
    public class MeCommand : ICommand
    {
        public string Command => "/me";

        public string Description => "Retorna informações sobre o usuário";
       
    }
}
