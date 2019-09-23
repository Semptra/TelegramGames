namespace TelegramGames.Icq.Core.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LaunchTime
    {
        [Key]
        public int Id { get; set; }

        public DateTime LastLaunchTime { get; set; }
    }
}
