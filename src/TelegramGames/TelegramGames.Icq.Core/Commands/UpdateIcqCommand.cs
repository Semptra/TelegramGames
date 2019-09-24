namespace TelegramGames.Icq.Core.Commands
{
    using System;
    using System.Linq;
    using Serilog;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using TelegramGames.Core.Extensions;
    using TelegramGames.Core.Models;
    using TelegramGames.Icq.Core.Database;
    using TelegramGames.Icq.Core.Extensions;

    public class UpdateIcqCommand : ICommand
    {
        private readonly Random random;
        private readonly IcqBotContext context;
        private readonly ILogger log;

        public UpdateIcqCommand(IcqBotContext context, ILogger log)
        {
            random = new Random();
            this.context = context;
            this.log = log;
        }

        public string Name => "update_icq";

        public void Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            var messageDate = message.Date.ToUniversalTime();
            var lastLanchDate = context.LaunchTimes.OrderBy(x => x.LastLaunchTime).Last().LastLaunchTime.ToUniversalTime();
            if (messageDate < lastLanchDate)
            {
                log.Warning("Recieved message with date {0} which is earlier then {1}", lastLanchDate, messageDate);
                return;
            }

            var currentUser = context.Users.FirstOrDefault(x => x.Id == message.From.Id);
            if (currentUser == null)
            {
                AddNewUser(message, telegramBotClient);
            }
            else
            {
                if (IsUpdateInTime(currentUser.LastTimePlayed))
                {
                    UpdateUser(currentUser, message, telegramBotClient);
                }
                else
                {
                    PunishUser(currentUser, message, telegramBotClient);
                }
            }

            context.SaveChanges();
        }

        private void AddNewUser(Message message, ITelegramBotClient telegramBotClient)
        {
            var icq = random.Next(1, 10);
            var newUser = new Database.Models.User
            {
                Id = message.From.Id,
                Name = message.From.GetFullName(),
                Icq = icq,
                LastIcqDelta = icq,
                TotalPlayed = 1,
                TotalFailed = 0,
                LastTimePlayed = DateTime.Now.ToUniversalTime()
            };

            telegramBotClient.SendTextMessage(message.Chat.Id, $"Добро пожаловать в игру \"ICQ Meter\", {newUser.Name}!");
            telegramBotClient.SendTextMessage(message.Chat.Id, $"{newUser.Name}, Ваш ICQ теперь равен {newUser.Icq}, поздравляем!");

            context.Users.Add(newUser);
        }

        private void UpdateUser(Database.Models.User user, Message message, ITelegramBotClient telegramBotClient)
        {
            int icqDelta;
            string changeText;

            if (IsPositive())
            {
                icqDelta = random.Next(1, 12);
                changeText = "увеличился";
            }
            else
            {
                icqDelta = random.Next(-9, -1);
                changeText = "уменьшился";
            }

            var newIcq = user.Icq + icqDelta;
            if (newIcq <= 0)
            {
                newIcq = 0;
            }

            user.Icq = newIcq;
            user.Name = message.From.GetFullName();
            user.LastIcqDelta = icqDelta;
            user.TotalPlayed += 1;
            user.LastTimePlayed = DateTime.Now.ToUniversalTime();

            telegramBotClient.SendTextMessage(message.Chat.Id, $"{user.Name}, Ваш ICQ {changeText} на {Math.Abs(icqDelta)} и теперь равен {user.Icq}, поздравляем!");

            context.Users.Update(user);
        }

        private void PunishUser(Database.Models.User user, Message message, ITelegramBotClient telegramBotClient)
        {
            var icqDelta = random.Next(-6, -2);
            var newIcq = user.Icq + icqDelta;
            if (newIcq <= 0)
            {
                newIcq = 0;
            }

            user.Icq = newIcq;
            user.Name = message.From.GetFullName();
            user.LastIcqDelta = icqDelta;
            user.TotalPlayed += 1;
            user.TotalFailed += 1;

            telegramBotClient.SendTextMessage(message.Chat.Id, $"{user.Name}, Вы уже играли сегодня, поэтому Ваш ICQ уменьшился на {Math.Abs(icqDelta)} и теперь равен {user.Icq}, будьте внимательны!");

            context.Users.Update(user);
        }

        private bool IsUpdateInTime(DateTime lastTimePlayed)
        {
            var deltaTime = DateTime.Now.ToUniversalTime() - lastTimePlayed;
            var delta = deltaTime.CompareTo(TimeSpan.FromDays(1));
            return delta >= 0;
        }

        private bool IsPositive()
        {
            return random.Next(0, 2) == 0;
        }
    }
}
