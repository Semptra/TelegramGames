namespace TelegramGames.Scrapper.Core.Models
{
    public class KnowYourMeme
    {
        public KnowYourMeme(string title, string photoUrl, string description)
        {
            Title = title;
            PhotoUrl = photoUrl;
            Description = description;
        }

        public string Title { get; }

        public string PhotoUrl { get; }

        public string Description { get; }
    }
}
