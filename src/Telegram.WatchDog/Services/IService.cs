using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.WatchDog.Services
{
    public interface IService
    {
        Task Initialize(CancellationToken cancellationToken);

        IEnumerable<UpdateType> RequiredUpdates { get; }

        Task Stop(CancellationToken cancellationToken);
    }
}

