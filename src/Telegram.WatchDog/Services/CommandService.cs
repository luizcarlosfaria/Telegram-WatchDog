using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.WatchDog.Commands;

namespace Telegram.WatchDog.Services
{
    public class CommandService : IService
    {
        private readonly ILogger<CommandService> logger;
        private readonly TelegramBotClient botClient;
        private readonly User me;
        private readonly IEnumerable<ICommand> commands;

        public IEnumerable<UpdateType> RequiredUpdates => new UpdateType[] { UpdateType.Message };

        public CommandService(ILogger<CommandService> logger, TelegramBotClient botClient, User user, IEnumerable<ICommand> commands)
        {
            this.logger = logger;
            this.botClient = botClient;
            this.me = user;
            this.commands = commands;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            if (this.commands.Any())
            {
                await this.botClient.SetMyCommandsAsync(this.commands.Select(it => new BotCommand() { Command = it.Command, Description = it.Description }), cancellationToken);
            }
            else
            {
                await this.botClient.SetMyCommandsAsync(new[] { new Bot.Types.BotCommand() { Command = "/hello", Description = "Hello World" } }, cancellationToken);
            }

            this.botClient.OnMessage += this.BotClient_OnMessage;
        }

        public Task Stop(CancellationToken cancellationToken) => Task.CompletedTask;


        private void BotClient_OnMessage(object sender, Bot.Args.MessageEventArgs e)
        {
            (MessageEntity firstMessageEntity, string commandText) = e.Message.GetFirstEntityOf(it => it.Type == MessageEntityType.BotCommand);

            if (this.commands.Any() && firstMessageEntity != null && e.Message.Type == MessageType.Text)
            {
                ICommand command = this.commands.FirstOrDefault(it => it.Command == commandText || $"{it.Command}@{this.me.Username}" == commandText);

                if (command != null)
                {
                    command.Handle(e.Message);
                }

                this.logger.LogInformation("Command: {time}", e.Message.Entities.First().ToString());
            }
        }
    }
}
