using System.Linq;
using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("commands")]
        [Summary("View the commands.")]
        public async Task Commands()
        {
            var response = $"*The command prefix for this server is `{Context.DbGuild.Prefix}`*.\n\n";

            foreach (var module in _commandService.Modules) // for every command module
            {
                response += $"__{module.Name}__ - {module.Summary}\n"; // add the name and its summary
                response = module.Commands.Aggregate(response,
                    (current, command) =>
                        current +
                        $"`{command.Name}`: {command.Summary}\n"); // add each command in that module and its summary
                response += "\n\n";
            }

            await Context.SendAsync(response);
        }
    }
}