namespace TelegramGames.Core.Extensions
{
    using System;
    using Telegram.Bot;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Types.Enums;

    public static class ITelegramBotClientExtensions
    {
        public static void SendTextMessage(
            this ITelegramBotClient telegramBotClient, 
            long chatId, 
            string message, 
            ParseMode parseMode = ParseMode.Default)
        {
            try
            {
                telegramBotClient.SendTextMessageAsync(chatId, message, parseMode)
                                 .ConfigureAwait(false)
                                 .GetAwaiter()
                                 .GetResult();
            }
            catch (ApiRequestException ex)
            {
                Console.WriteLine(ex);
                telegramBotClient.SendSorryMessage(chatId);
            }
        }

        public static void SendPhoto(
            this ITelegramBotClient telegramBotClient,
            long chatId,
            string photoUrl,
            string caption,
            ParseMode parseMode = ParseMode.Default)
        {
            try
            {
                telegramBotClient.SendPhotoAsync(chatId, photoUrl, caption, parseMode)
                             .ConfigureAwait(false)
                             .GetAwaiter()
                             .GetResult();
            }
            catch (ApiRequestException ex)
            {
                Console.WriteLine(ex);
                telegramBotClient.SendSorryMessage(chatId);
            }
        }

        static void SendSorryMessage(this ITelegramBotClient telegramBotClient, long chatId)
        {
            telegramBotClient.SendTextMessage(chatId, "Oops, something bad happened during the request.");
        }
    }
}
