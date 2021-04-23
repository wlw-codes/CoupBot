using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("invite")]
        [Summary("View the link for inviting the bot.")]
        public async Task Invite()
        {
            await Context.DmAsync(Configuration.InviteLink);
        }
    }
}