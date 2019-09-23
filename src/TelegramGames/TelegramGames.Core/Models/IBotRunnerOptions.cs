using System.Collections.Generic;

namespace TelegramGames.Core.Models
{
    public interface IBotRunnerOptions
    {
        string Token { get; }

        string BotName { get; }

        IDictionary<string, string> Options { get; }
    }
}
