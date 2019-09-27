namespace TelegramGames.Scrapper.Core.Services
{
    using HtmlAgilityPack;

    public abstract class WebParser : IWebParser
    {
        public abstract string Url { get; }

        protected HtmlDocument Document => LoadDocument(); 

        private HtmlDocument LoadDocument()
        {
            var loader = new HtmlWeb();
            return loader.Load(Url);
        }
    }
}
