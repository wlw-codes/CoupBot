using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("git")]
        [Summary("View the link for the Git repository.")]
        public async Task Git()
        {
            await DmAsync(Context.User, Configuration.RepositoryUrl);
        }
    }
}