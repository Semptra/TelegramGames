namespace TelegramGames.Core.Models
{
    public class BotRunnerOptions : IBotRunnerOptions
    {
        public BotRunnerOptions(string token, string botName)
        {
            Token = token;
            BotName = botName;
        }

        public string Token { get; }

        public string BotName { get; }
    }
}
