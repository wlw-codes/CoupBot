using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("readme")]
        [Summary("View the readme for this bot.")]
        public async Task Readme()
        {
            var readmeFile =
                await File.ReadAllTextAsync(AppContext.BaseDirectory +
                                            "../../../../README.md"); // read the readme as string

            await Context.DmAsync(readmeFile);
        }
    }
}