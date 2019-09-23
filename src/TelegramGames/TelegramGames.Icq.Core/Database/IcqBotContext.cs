namespace TelegramGames.Icq.Core.Database
{
    using Microsoft.EntityFrameworkCore;
    using TelegramGames.Icq.Core.Database.Models;

    public class IcqBotContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<LaunchTime> LaunchTimes { get; set; }

        public IcqBotContext(DbContextOptions options) : base(options)
        {
        }
    }
}
