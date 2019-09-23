namespace TelegramGames.Icq.ConsoleApp
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using TelegramGames.Core;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core;
    using TelegramGames.Icq.Core.Commands;
    using TelegramGames.Icq.Core.Database;

    public class Program
    {
        public static void Main()
        {
            var services = BuildServices();
            var runner = services.GetRequiredService<IBotRunner>();
            runner.Start();
            Console.WriteLine("Press Enter to stop...");
            Console.ReadLine();
            runner.Stop();
        }

        static IServiceProvider BuildServices()
        {
            var serviceCollection = new ServiceCollection();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            // TODO: Add commands
            serviceCollection.AddSingleton<ICommand, UpdateIcqCommand>();
            serviceCollection.AddSingleton<ICommand, TopCommand>();

            serviceCollection.AddSingleton<ILogger>(_ => logger);
            serviceCollection.AddDbContext<IcqBotContext>(options => options.UseSqlite("Data Source=/Users/user/Projects/icqbot.db"));
            serviceCollection.AddSingleton<IBotRunnerOptions>(_ => new BotRunnerOptions ("", ""));
            serviceCollection.AddSingleton<IBotRunner, IcqBotRunner>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
