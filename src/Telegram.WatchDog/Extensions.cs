using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.WatchDog
{
    public static class Extensions
    {

        /// <summary>
        /// Register a type as itself and your interface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        public static void AddSingleton2<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        {
            services.AddSingleton<TImplementation>();

            services.AddSingleton<TInterface>(sp => sp.GetRequiredService<TImplementation>());
        }

    }
}
