using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("discord")]
        [Summary("View the link for the Discord server.")]
        public async Task Discord()
        {
            await DmAsync(Context.User, Configuration.SupportServerUrl);
        }
    }
}