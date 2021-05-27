using System.Linq;
using CoupBot.Common.Structures;
using CoupBot.Database.Models;

namespace CoupBot.Common.Extensions.Models
{
    public static class GuildExtensions
    {
        public static Coup GetCurrentCoup(this Guild guild)
        {
            return guild.Coups.OrderBy(x => x.TimeInitiated).FirstOrDefault();
        }
    }
}