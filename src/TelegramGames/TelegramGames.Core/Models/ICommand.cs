namespace TelegramGames.Core.Models
{
    using Telegram.Bot;
    using Telegram.Bot.Types;

    public interface ICommand
    {
        string Name { get; }

        void Execute(Message message, ITelegramBotClient telegramBotClient);
    }
}
