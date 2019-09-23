using System.Collections.Generic;

namespace TelegramGames.Core.Models
{
    public class BotRunnerOptions : IBotRunnerOptions
    {
        public BotRunnerOptions(string token, string botName)
        {
            Token = token;
            BotName = botName;
            Options = new Dictionary<string, string>();
        }

        public string Token { get; }

        public string BotName { get; }

        public IDictionary<string, string> Options { get; }
    }
}
