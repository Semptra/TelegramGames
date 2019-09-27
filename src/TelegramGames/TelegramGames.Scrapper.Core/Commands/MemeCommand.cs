namespace TelegramGames.Scrapper.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Models;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Scrapper.Core.Services;
    using Telegram.Bot.Types.Enums;

    public class MemeCommand : ICommand
    {
        private const int MaxCaptionLength = 4000;

        private readonly KnowYourMemeParser knowYourMemeParser;

        public MemeCommand(IServiceProvider serviceProvider)
        {
            knowYourMemeParser = serviceProvider.GetServices<IWebParser>()
                .First(parser => parser.Url == Constants.KnowYourMemeUrl) as KnowYourMemeParser;
        }

        public string Name => "meme";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var meme = knowYourMemeParser.GetRandomMeme();

            if (meme.Description.Length > MaxCaptionLength)
            {
                var chunks = Enumerable.Range(0, meme.Description.Length / MaxCaptionLength)
                                       .Select(i => meme.Description.Substring(i * MaxCaptionLength, MaxCaptionLength));

                telegramBotClient.SendPhoto(message.Chat.Id, meme.PhotoUrl, $"<b>{meme.Title}</b>{chunks.First()}", ParseMode.Html);

                foreach(var chunk in chunks.Skip(1))
                {
                    telegramBotClient.SendTextMessage(message.Chat.Id, chunk, ParseMode.Html);
                }
            }
            else
            {
                telegramBotClient.SendPhoto(message.Chat.Id, meme.PhotoUrl, $"<b>{meme.Title}</b>{meme.Description}", ParseMode.Html);
            }
        }
    }
}
