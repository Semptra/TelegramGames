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

        public DateTime LastTimePlayed { get; set; }
    }
}
