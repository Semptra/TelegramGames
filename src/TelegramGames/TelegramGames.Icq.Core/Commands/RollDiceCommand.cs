namespace TelegramGames.Icq.Core.Commands
{
    using System;
    using System.Linq;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;
    using TelegramGames.Icq.Core.Database.Models;

    public class RollDiceCommand : ICommand
    {
        private readonly Random random;
        private readonly IcqBotContext context;

        public RollDiceCommand(IcqBotContext context)
        {
            random = new Random();
            this.context = context;
        }

        public string Name => "roll_dice";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var firstDiceResult = random.Next(1, 7);
            var secondDiceResult = random.Next(1, 7);
            var currentPlayer = message.From.GetFullName();
            var isAnyPlayedAlready = context.DiceResults.Any();

            if (isAnyPlayedAlready)
            {
                string winnerMessage;
                var lastDiceResult = context.DiceResults.Last();
                var result = (firstDiceResult + secondDiceResult) - (lastDiceResult.LastFirstDiceResult + lastDiceResult.LastSecondDiceResult);
                if (result < 0)
                {
                    winnerMessage = $"Победитель: {lastDiceResult.LastUserPlayed}!";
                }
                else if (result > 0)
                {
                    winnerMessage = $"Победитель: {currentPlayer}!";
                }
                else
                {
                    winnerMessage = "Ничья!";
                }

                telegramBotClient.SendTextMessage(message.Chat.Id, $"{currentPlayer}, Вы сыграли в игру \"Кости\" против {lastDiceResult.LastUserPlayed}.\n" +
                    $"Ваш результат: {firstDiceResult} и {secondDiceResult} против {lastDiceResult.LastFirstDiceResult} и {lastDiceResult.LastSecondDiceResult}.\n" +
                    $"{winnerMessage}");

                context.DiceResults.RemoveRange(context.DiceResults.ToList());
            }
            else
            {
                var diceResult = new DiceResult
                {
                    LastUserPlayed = currentPlayer,
                    LastFirstDiceResult = firstDiceResult,
                    LastSecondDiceResult = secondDiceResult
                };

                context.DiceResults.Add(diceResult);
                telegramBotClient.SendTextMessage(message.Chat.Id, $"{diceResult.LastUserPlayed}, Вы сыграли в игру \"Кости\".\n" +
                    $"Ваш результат: {diceResult.LastFirstDiceResult} и {diceResult.LastSecondDiceResult}!\n" +
                    $"Ожидаем результата противника.");
            }

            context.SaveChanges();
        }
    }
}
