namespace TelegramGames.Core.Extensions
{
    using Telegram.Bot;
    using Telegram.Bot.Types.Enums;

    public static class ITelegramBotClientExtensions
    {
        public static void SendTextMessage(
            this ITelegramBotClient telegramBotClient, 
            long chatId, 
            string message, 
            ParseMode parseMode = ParseMode.Default)
        {
            telegramBotClient.SendTextMessageAsync(chatId, message, parseMode)
                             .ConfigureAwait(false)
                             .GetAwaiter()
                             .GetResult();
        }
    }
}
