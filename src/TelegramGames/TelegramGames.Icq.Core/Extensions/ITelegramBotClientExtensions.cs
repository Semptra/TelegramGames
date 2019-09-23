namespace TelegramGames.Icq.Core.Extensions
{
    using Telegram.Bot;
    using Telegram.Bot.Types;

    public static class ITelegramBotClientExtensions
    {
        public static void SendTextMessage(this ITelegramBotClient telegramBotClient, long chatId, string message)
        {
            telegramBotClient.SendTextMessageAsync(chatId, message)
                             .ConfigureAwait(false)
                             .GetAwaiter()
                             .GetResult();
        }
    }
}
