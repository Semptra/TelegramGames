namespace TelegramGames.Scrapper.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.Extensions.DependencyInjection;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Models;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Scrapper.Core.Services;
    using Telegram.Bot.Types.Enums;

    public class BashQuoteCommand : ICommand
    {
        private readonly BashParser bashParser;

        public BashQuoteCommand(IServiceProvider serviceProvider)
        {
            bashParser = serviceProvider.GetServices<IWebParser>()
                .First(parser => parser.Url == Constants.BashUrl) as BashParser;
        }

        public string Name => "bash_quote";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var quote = bashParser.GetRandomQuote();
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"Цитата <b>{quote.Number}</b> от <b>{quote.Date}</b>");
            messageBuilder.AppendLine($"Рейтинг: <b>{quote.Rating}</b>");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine(quote.Text);

            telegramBotClient.SendTextMessage(message.Chat.Id, messageBuilder.ToString(), ParseMode.Html);
        }
    }
}
