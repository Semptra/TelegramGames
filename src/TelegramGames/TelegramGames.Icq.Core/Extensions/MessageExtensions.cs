namespace TelegramGames.Icq.Core.Extensions
{
    using Telegram.Bot.Types;

    public static class MessageExtensions
    {
        public static string GetFullName(this Message message)
        {
            return string.Join(" ", message.From.FirstName, message.From.LastName).Trim();
        }
    }
}
