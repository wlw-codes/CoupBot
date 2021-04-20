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

            foreach (var module in _commandService.Modules)
            {
                response += $"__{module.Name}__ - {module.Summary}\n";
                
                foreach (var command in module.Commands)
                {
                    response += $"`{command.Name}`: {command.Summary}\n";
                }

                response += "\n\n";
            }

            await SendAsync(response);
        }
    }
}