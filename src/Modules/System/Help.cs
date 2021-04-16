using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("Help")]
        [Alias("info")]
        [Summary("View the basic information regarding this bot.")]
        public async Task Help()
        {
            await DmAsync(Context.User, Configuration.HelpMessage);
        }
    }
}
