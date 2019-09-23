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
        public BotRunner(IBotRunnerOptions botRunnerOptions, IEnumerable<ICommand> commands, ILogger log)
        {
            BotName = botRunnerOptions.BotName;
            TelegramBotClient = new TelegramBotClient(botRunnerOptions.Token);
            TelegramBotClient.OnMessage += OnMessageRecieved;
            Commands = commands.Distinct(new CommandEqualityComparer()).ToList();
            Log = log;
        }

        protected ITelegramBotClient TelegramBotClient { get; }

        protected string BotName { get; }

        protected IList<ICommand> Commands { get; }

        protected ILogger Log { get; }

        public virtual void Start()
        {
            Log.Information("BorRunner for bot '{0}' started", BotName);
            TelegramBotClient.StartReceiving();
        }

        public virtual void Stop()
        {
            TelegramBotClient.StopReceiving();
            Log.Information("BorRunner for bot '{0}' stopped", BotName);
        }

        private void OnMessageRecieved(object sender, MessageEventArgs args)
        {
            var message = args.Message;

            Log.Information("Recieved message of type {0} from user {1} ({2})", message.Type, message.From.Username, string.Join(" ", message.From.FirstName, message.From.LastName).Trim());

            var command = Commands.FirstOrDefault(x => message.Type == MessageType.Text && IsMessageContainsBotName(message.Text, x));
            if (command != null)
            {
                Log.Information("Executing command {0}", command.Name);
                command.Execute(message, TelegramBotClient);
            }
            else
            {
                Log.Error("Command for message '{0}' not found", message.Text);
            }
        }

        private bool IsMessageContainsBotName(string message, ICommand command)
        {
            return string.Equals($"/{command.Name}", message, StringComparison.InvariantCulture) ||
                   string.Equals($"/{command.Name}@{BotName}", message, StringComparison.InvariantCulture);
        }
    }
}
