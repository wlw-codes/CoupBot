using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("help")]
        [Summary("View the help message.")]
        public async Task Help()
        {
            await DmAsync(Context.User, Configuration.HelpMessage);
        }
    }
}
