namespace TelegramGames.Icq.Core.Commands
{
    using System.Linq;
    using System.Text;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;
    using TelegramGames.Icq.Core.Extensions;

    public class StatsCommand : ICommand
    {
        private readonly IcqBotContext context;

        public StatsCommand(IcqBotContext context)
        {
            this.context = context;
        }

        public string Name => "stats";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var currentUser = context.Users.FirstOrDefault(x => x.Id == message.From.Id);
            if (currentUser == null)
            {
                telegramBotClient.SendTextMessage(message.Chat.Id, $"{message.From.GetFullName()}, Вы еще не использовали ICQ Meter, поэтому у нас нет Вашей статистики.");
                return;
            }

            var userPosition = context.Users.OrderByDescending(x => x.Icq)
                                            .Select((user, index) => new { user, index })
                                            .Where(pair => pair.user.Id == currentUser.Id)
                                            .Select(pair => pair.index + 1)
                                            .FirstOrDefault();

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"Статистика игрока {currentUser.Name}:\n");
            messageBuilder.AppendLine($"Позиция в топе: {userPosition}");
            messageBuilder.AppendLine($"ICQ: {currentUser.Icq}");
            messageBuilder.AppendLine($"Последнее изменение ICQ: {currentUser.LastIcqDelta}");
            messageBuilder.AppendLine($"Всего сыграно: {currentUser.TotalPlayed}");
            messageBuilder.AppendLine($"Всего неудач: {currentUser.TotalFailed}");

            telegramBotClient.SendTextMessage(message.Chat.Id, messageBuilder.ToString());
        }
    }
}
