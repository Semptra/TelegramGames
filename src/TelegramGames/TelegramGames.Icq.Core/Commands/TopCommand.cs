namespace TelegramGames.Icq.Core.Commands
{
    using System.Linq;
    using System.Text;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;
    using TelegramGames.Icq.Core.Extensions;

    public class TopCommand : ICommand
    {
        private readonly IcqBotContext context;

        public TopCommand(IcqBotContext context)
        {
            this.context = context;
        }

        public string Name => "top";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var messageBuilder = new StringBuilder("Рейтинг ICQ:\n\n");

            var users = context.Users.OrderByDescending(x => x.Icq).ToList();
            for(int i = 0; i < users.Count; i++)
            {
                messageBuilder.Append($"{i + 1}. {users[i].Name} c ICQ {users[i].Icq}.");
            }

            telegramBotClient.SendTextMessage(message.Chat.Id, messageBuilder.ToString());
        }
    }
}
