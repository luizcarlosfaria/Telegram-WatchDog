using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.WatchDog.Commands;
using Telegram.WatchDog.Services;

namespace Telegram.WatchDog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static string GetEnv(string key) => Environment.GetEnvironmentVariable(key);

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(sp => new TelegramBotClient(GetEnv("TELEGRAM_API_TOKEN")));

                    services.AddSingleton(sp => sp.GetRequiredService<TelegramBotClient>().GetMeAsync().Sync());

                    services.AddSingleton2<IService, AclService>();

                    services.AddSingleton2<IService, CommandService>();

                    services.AddSingleton<ICommand, WhoisCommand>();

                    services.AddSingleton<ICommand, BanCommand>();

                    services.AddHostedService<WhatchDogService>();
                });
    }
}
