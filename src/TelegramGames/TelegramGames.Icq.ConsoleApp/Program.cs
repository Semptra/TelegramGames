namespace TelegramGames.Icq.ConsoleApp
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using TelegramGames.Core;
    using TelegramGames.Core.Models;
    
    public class Program
    {
        public static void Main()
        {
            var services = BuildServices();
        }

        static IServiceProvider BuildServices()
        {
            var serviceCollection = new ServiceCollection();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            serviceCollection.AddSingleton<ILogger>(_ => logger);

            serviceCollection.AddSingleton<IBotRunnerOptions>(_ => new BotRunnerOptions ("", ""));

            // TODO: Add commands
            serviceCollection.AddSingleton<ICommand>();

            serviceCollection.AddSingleton<IBotRunner, BotRunner>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
