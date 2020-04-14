using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.WatchDog.Services
{
    public class AclService : IService
    {
        private readonly TelegramBotClient botClient;

        public AclService(TelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public IEnumerable<UpdateType> RequiredUpdates => new UpdateType[] { };

        public Task Initialize(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task Stop(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Test()
        {



        }

        public bool IsAdmin(User user, Chat chat) => this.Is(user, chat, it => true);

        public bool Is(User user, Chat chat, Func<ChatMember, bool> func)
        {
            var admins = botClient.GetChatAdministratorsAsync(chat.Id).Sync();
            return admins.Any(it => it.User.Id == user.Id && func(it));
        }
    }
}
