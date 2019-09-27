namespace TelegramGames.Icq.Core.Commands
{
    using System.Linq;
    using System.Text;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;

    public class Top10Command : ICommand
    {
        private readonly IcqBotContext context;

        public Top10Command(IcqBotContext context)
        {
            this.context = context;
        }

        public string Name => "top10";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var messageBuilder = new StringBuilder("Рейтинг ICQ (топ 10):\n\n");

            var users = context.Users.OrderByDescending(x => x.Icq).Take(10).ToList();
            for(int i = 0; i < users.Count; i++)
            {
                messageBuilder.Append($"{i + 1}. {users[i].Name} c ICQ {users[i].Icq}.\n");
            }

            telegramBotClient.SendTextMessage(message.Chat.Id, messageBuilder.ToString());
        }
    }
}
