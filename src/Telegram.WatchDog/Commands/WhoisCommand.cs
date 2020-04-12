using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.WatchDog.Services;

namespace Telegram.WatchDog.Commands
{
    public class WhoisCommand : ICommand
    {
        private AclService aclService;
        private readonly TelegramBotClient botClient;

        public string Command => "/whois";

        public string Description => "Retorna informações sobre um usuário";

        public WhoisCommand(AclService aclService, TelegramBotClient botClient)
        {
            this.aclService = aclService;
            this.botClient = botClient;
        }

        public void Handle(Message message)
        {
            User userToIdentify = message.ReplyToMessage?.From;

            if (userToIdentify == null && message.Entities.IsNotNullAndIsNotEmpty())
            {
                (MessageEntity mentionEntity, string mentionText) = message.GetFirstEntityOf(it => it.Type == Bot.Types.Enums.MessageEntityType.Mention);
                if (mentionEntity != null)
                {
                    //this.botClient.GetChatMemberAsync(message.Chat.Id, )
                }
            }

            if (userToIdentify == null)
            {
                userToIdentify = message.From;
            }


            if (userToIdentify != null && aclService.IsAdmin(message.From, message.Chat))
            {
                string messageText = Newtonsoft.Json.JsonConvert.SerializeObject(userToIdentify, Newtonsoft.Json.Formatting.Indented);

                this.botClient.SendTextMessageAsync(message.Chat.Id, $"<pre>{messageText}</pre>",
                    parseMode: Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: message.MessageId
                ).Sync();
            }
        }
    }
}
