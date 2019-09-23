namespace TelegramGames.Core.Comparers
{
    using System.Collections.Generic;
    using TelegramGames.Core.Models;

    public class CommandEqualityComparer : IEqualityComparer<ICommand>
    {
        public bool Equals(ICommand x, ICommand y)
        {
            return x.GetHashCode().Equals(y.GetHashCode());
        }

        public int GetHashCode(ICommand obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
