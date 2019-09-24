namespace TelegramGames.Core.Extensions
{
    using Telegram.Bot.Types;

    public static class UserExtensions
    {
        public static string GetFullName(this User user)
        {
            return string.Join(" ", user.FirstName, user.LastName).Trim();
        }
    }
}
