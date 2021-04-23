using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    public partial class System
    {
        [Command("command")]
        [Summary("View additional details about a specific command.")]
        public async Task Command([Remainder] string commandName = null)
        {
            if (commandName == null)
            {
                await Commands();
                return;
            }
            
            var response = $"*The command prefix for this server is `{Context.DbGuild.Prefix}`.\n" +
                           $"Required parameters are surrounded by `<>` and optional parameters by `[]`.*\n\n";
            var search = _commandService.Commands.SingleOrDefault(x => x.Name.ToLower() == commandName);

            if (search == null)
            {
                await Context.ReplyAsync($"the command `{commandName}` does not exist.");
                return;
            }

            response += $"`{search.Name} ";
                
            foreach (var parameter in search.Parameters)
            {
                if (parameter.IsOptional)
                {
                    response += $"[{parameter.Name}] ";
                }
                else
                {
                    response += $"<{parameter.Name}> ";
                }
            }
                
            response = response.Remove(response.Length - 1);
            response += $"`: {search.Summary}";
            
            await Context.SendAsync(response);
        }
    }
}