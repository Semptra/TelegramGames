namespace TelegramGames.Icq.Core
{
    using System;
    using System.Collections.Generic;
    using Serilog;
    using TelegramGames.Core;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;

    public class IcqBotRunner : BotRunner
    {
        private readonly IcqBotContext context;

        public IcqBotRunner(IBotRunnerOptions botRunnerOptions, IEnumerable<ICommand> commands, ILogger log, IcqBotContext context) : base (botRunnerOptions, commands, log)
        {
            this.context = context;
        }

        public override void Start()
        {
            context.Database.EnsureCreated();
            context.LaunchTimes.Add(new Database.Models.LaunchTime { LastLaunchTime = DateTime.Now.ToUniversalTime() });
            context.SaveChanges();

            base.Start();
        }
    }
}
