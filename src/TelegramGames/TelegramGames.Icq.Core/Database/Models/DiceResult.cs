namespace TelegramGames.Icq.Core.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class DiceResult
    {
        [Key]
        public int Id { get; set; }

        public string LastUserPlayed { get; set; }

        public int LastFirstDiceResult { get; set; }

        public int LastSecondDiceResult { get; set; }
    }
}
