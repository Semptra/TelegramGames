namespace TelegramGames.Icq.ConsoleApp
{
    using System;
    using System.IO;
    using System.Text.Json;
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

            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(appsettingsPath))
            {
                log.Error("appsettings.json not found in \"{0}\"", appsettingsPath);
                Console.ReadLine();
                Environment.Exit(1);
            }

            var appsettings = JsonSerializer.Deserialize<IcqBotConfiguration>(File.ReadAllText(appsettingsPath));

            // TODO: Add commands
            serviceCollection.AddSingleton<ICommand, UpdateIcqCommand>();
            serviceCollection.AddSingleton<ICommand, StatsCommand>();
            serviceCollection.AddSingleton<ICommand, TopCommand>();
            serviceCollection.AddSingleton<ICommand, Top10Command>();

            serviceCollection.AddSingleton<ILogger>(_ => log);
            serviceCollection.AddDbContext<IcqBotContext>(options => options.UseSqlite(appsettings.ConnectionString));
            serviceCollection.AddSingleton<IBotRunnerOptions>(_ => new BotRunnerOptions (appsettings.Token, appsettings.BotName));
            serviceCollection.AddSingleton<IBotRunner, IcqBotRunner>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
