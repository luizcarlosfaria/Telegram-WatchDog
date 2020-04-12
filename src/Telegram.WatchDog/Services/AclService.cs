using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.WatchDog.Services
{
    public class AclService: IService
    {
        public AclService()
        { 
        
        }

        public Task Initialize(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task Stop(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Test()
        { }
    }
}
