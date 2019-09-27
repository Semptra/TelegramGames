using System.Linq;
using TelegramGames.Scrapper.Core.Models;

namespace TelegramGames.Scrapper.Core.Services
{
    public class KnowYourMemeParser : WebParser
    {
        public override string Url => Constants.KnowYourMemeUrl;

        public KnowYourMeme GetRandomMeme()
        {
            var article = Document.DocumentNode.SelectSingleNode("//div[@id='content']/div[@id='maru']/article");

            // Something like: 2018 New York City Power Plant Explosions
            var title = article.SelectSingleNode("//header/section/h1/a").InnerText;

            // Something like: https://i.kym-cdn.com/entries/icons/original/000/027/973/Blue_Light_Sky_Pic_CREDIT_Rich_Villodas_(1).png
            var photoUrl = article.SelectSingleNode("//header/a").Attributes["href"].Value;

            var x = article.SelectSingleNode("//div[@id='entry_body']/section[@class='bodycopy']");

            var description = string.Join("\n",
                article.SelectSingleNode("//div[@id='entry_body']/section[@class='bodycopy']")
                       .ChildNodes
                       .Where(node => node.Name == "h2" || node.Name == "p")
                       .Select(node => node.Name == "h2" ? $"<b>{node.InnerText}</b>\n" : node.InnerHtml)
                       .Select(text => text.Replace("<br>", "\n")
                                           .Replace("<i>", string.Empty)
                                           .Replace("</i>", string.Empty)
                                           .Replace("<em>", string.Empty)
                                           .Replace("</em>", string.Empty)));

            return new KnowYourMeme(title, photoUrl, description);
        }
    }
}
