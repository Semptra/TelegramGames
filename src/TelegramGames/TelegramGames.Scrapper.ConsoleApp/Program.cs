namespace TelegramGames.Scrapper.ConsoleApp
{
    using System;
    using System.IO;
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using TelegramGames.Core;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core;
    using TelegramGames.Scrapper.Core.Commands;
    using TelegramGames.Scrapper.Core.Services;

    class Program
    {
        static void Main(string[] args)
        {
            var services = BuildServices();
            var runner = services.GetRequiredService<IBotRunner>();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var log = services.GetRequiredService<ILogger>();
                if (args.ExceptionObject is Exception ex)
                {
                    log.Error($"Unhandled exception", ex);
                }
                else
                {
                    log.Error("Unknown exception");
                }
            };

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

            var appsettings = JsonSerializer.Deserialize<ScrapperBotConfiguration>(File.ReadAllText(appsettingsPath));

            // TODO: Add commands
            serviceCollection.AddSingleton<ICommand, QuoteCommand>();

            serviceCollection.AddSingleton<ILogger>(_ => log);
            serviceCollection.AddSingleton<IWebParser, BashParser>();
            serviceCollection.AddSingleton<IBotRunnerOptions>(_ => new BotRunnerOptions(appsettings.Token, appsettings.BotName));
            serviceCollection.AddSingleton<IBotRunner, BotRunner>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
