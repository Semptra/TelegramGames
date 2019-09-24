namespace TelegramGames.Icq.Core.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Icq { get; set; }

        public int LastIcqDelta { get; set; }

        public int TotalPlayed { get; set; }

        public int TotalFailed { get; set; }

        public DateTime LastTimePlayed { get; set; }
    }
}
