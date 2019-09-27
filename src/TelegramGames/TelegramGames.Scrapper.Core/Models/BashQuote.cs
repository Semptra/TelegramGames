namespace TelegramGames.Scrapper.Core.Models
{
    public class BashQuote
    {
        public BashQuote(string number, string date, string text, string rating)
        {
            Number = number;
            Date = date;
            Text = text;
            Rating = rating;
        }

        public string Number { get; }

        public string Date { get; }

        public string Text { get; }

        public string Rating { get; }
    }
}
