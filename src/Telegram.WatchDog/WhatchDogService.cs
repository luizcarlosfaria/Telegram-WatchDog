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
            /*
                UpdateType.Message,
                UpdateType.EditedMessage,
                UpdateType.ChannelPost,
                UpdateType.EditedChannelPost
            */
            UpdateType[] updateTypes = null;
            if (this.services.Any())
            {
                await this.InitializeServices(cancellationToken);

                updateTypes = this.GetRequiredUpdateTypes();
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

        private UpdateType[] GetRequiredUpdateTypes()
        {
            List<UpdateType> updateTypes = new List<UpdateType>();

            foreach (var service in this.services)
            {
                var updates = service.RequiredUpdates;
                if (updates.Any())
                {
                    updateTypes.AddRange(updates);
                }
            }

            return updateTypes.Distinct().ToArray();
        }

        private async Task InitializeServices(CancellationToken cancellationToken)
        {
            foreach (var service in this.services)
            {
                await service.Initialize(cancellationToken);
            }
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
