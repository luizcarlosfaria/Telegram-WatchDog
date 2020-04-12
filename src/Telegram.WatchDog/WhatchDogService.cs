using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.WatchDog.Commands;
using Telegram.WatchDog.Services;

namespace Telegram.WatchDog
{
    public class WhatchDogService : BackgroundService
    {
        private readonly ILogger<WhatchDogService> _logger;
        private readonly TelegramBotClient botClient;
        private readonly IEnumerable<IService> services;

        public WhatchDogService(ILogger<WhatchDogService> logger, TelegramBotClient botClient, IEnumerable<IService> services)
        {
            this._logger = logger;
            this.botClient = botClient;
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            UpdateType[] updateTypes = new[] {
                UpdateType.Message,
                UpdateType.EditedMessage,
                UpdateType.ChannelPost,
                UpdateType.EditedChannelPost
            };

            if (this.services.Any())
            {
                foreach (var service in this.services)
                    await service.Initialize(cancellationToken);
            }

            this.botClient.StartReceiving(updateTypes, cancellationToken);

            await this.MainThread(cancellationToken);

            if (this.services.Any())
            {
                foreach (var service in this.services.Reverse())
                    await service.Stop(cancellationToken);
            }

            this.botClient.StopReceiving();
        }

        private async Task MainThread(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(500, cancellationToken);
            }
        }



        
    }
}
