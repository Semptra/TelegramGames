namespace TelegramGames.Scrapper.Core.Services
{
    using TelegramGames.Scrapper.Core.Models;

    public class BashParser : WebParser
    {
        public override string Url => Constants.BashUrl;

        // Look at https://www.w3schools.com/xml/xpath_syntax.asp for XPath tutorial
        public BashQuote GetRandomQuote()
        {
            var article = Document.DocumentNode.SelectSingleNode("(//section[@class='quotes']/article[@class='quote'])[1]");

            // Something like: #450440
            var number = article.SelectSingleNode("//div/header/a[@class='quote__header_permalink']").InnerText;

            // Something like: 21.05.2018 в 11:44
            // Take only first 10 characters
            var date = article.SelectSingleNode("//div/header/div[@class='quote__header_date']").InnerText.Trim().Substring(0, 10);

            // Newline <br> must be replaced with \n
            var text = article.SelectSingleNode("//div/div[@class='quote__body']")
                              .InnerHtml
                              .Replace("<br>", "\n")
                              .Trim(' ', '\n', '\r');

            // Something like: 3109
            var rating = article.SelectSingleNode("//div/footer/div[@class='quote__total']").InnerText;

            return new BashQuote(number, date, text, rating);
        }
    }
}
