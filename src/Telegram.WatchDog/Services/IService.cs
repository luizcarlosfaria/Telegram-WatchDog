using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.WatchDog.Services
{
    public interface IService
    {
        Task Initialize(CancellationToken cancellationToken);
        Task Stop(CancellationToken cancellationToken);
    }
}

