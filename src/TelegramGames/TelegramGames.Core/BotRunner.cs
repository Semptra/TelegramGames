namespace TelegramGames.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types.Enums;
    using TelegramGames.Core.Comparers;
    using TelegramGames.Core.Models;

    public class BotRunner
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly string botName;
        private readonly IList<ICommand> commands;

        public BotRunner(string token, string botName, IList<ICommand> commands)
        {
            this.botName = botName;
            this.commands = commands.Distinct(new CommandEqualityComparer()).ToList();
            telegramBotClient = new TelegramBotClient(token);
            telegramBotClient.OnMessage += OnMessageRecieved;
        }

        public void Start()
        {
            telegramBotClient.StartReceiving();
        }

        public void Stop()
        {
            telegramBotClient.StartReceiving();
        }

        private void OnMessageRecieved(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            var command = commands.FirstOrDefault(x => message.Type == MessageType.Text && IsMessageContainsBotName(message.Text, x));
            if (command != null)
            {
                command.Execute(message, telegramBotClient);
            }
        }

        private bool IsMessageContainsBotName(string message, ICommand command)
        {
            return string.Equals($"/{command.Name}", message, StringComparison.InvariantCulture) ||
                   string.Equals($"/{command.Name}@{botName}", message, StringComparison.InvariantCulture);
        }
    }
}
