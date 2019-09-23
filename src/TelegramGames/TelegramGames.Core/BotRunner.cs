namespace TelegramGames.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types.Enums;
    using TelegramGames.Core.Comparers;
    using TelegramGames.Core.Models;

    public class BotRunner : IBotRunner
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly string botName;
        private readonly IList<ICommand> commands;
        private readonly ILogger log;

        public BotRunner(IBotRunnerOptions botRunnerOptions, IList<ICommand> commands, ILogger log)
        {
            botName = botRunnerOptions.BotName;
            telegramBotClient = new TelegramBotClient(botRunnerOptions.Token);
            telegramBotClient.OnMessage += OnMessageRecieved;
            this.commands = commands.Distinct(new CommandEqualityComparer()).ToList();
            this.log = log;
        }

        public void Start()
        {
            log.Information("BorRunner for bot '{0}' started", botName);
            telegramBotClient.StartReceiving();
        }

        public void Stop()
        {
            telegramBotClient.StopReceiving();
            log.Information("BorRunner for bot '{0}' stopped", botName);
        }

        private void OnMessageRecieved(object sender, MessageEventArgs args)
        {
            var message = args.Message;

            log.Information("Recieved message of type {0} from user {1} ({2})", message.Type, message.From.Username, string.Join(" ", message.From.FirstName, message.From.LastName));

            var command = commands.FirstOrDefault(x => message.Type == MessageType.Text && IsMessageContainsBotName(message.Text, x));
            if (command != null)
            {
                log.Information("Executing command {0}", command.Name);
                command.Execute(message, telegramBotClient);
            }
            else
            {
                log.Error("Command for message '{0}' not found", message.Text);
            }
        }

        private bool IsMessageContainsBotName(string message, ICommand command)
        {
            return string.Equals($"/{command.Name}", message, StringComparison.InvariantCulture) ||
                   string.Equals($"/{command.Name}@{botName}", message, StringComparison.InvariantCulture);
        }
    }
}
