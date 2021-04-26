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
            if (commandName == null) // if the user does not supply a command
            {
                await Commands(); // just show them all available commands
                return;
            }

            var response = $"*The command prefix for this server is `{Context.DbGuild.Prefix}`.\n" +
                           $"Required parameters are surrounded by `<>` and optional parameters by `[]`.*\n\n";
            var search = _commandService.Commands.SingleOrDefault(x => x.Name.ToLower() == commandName);

            if (search == null) // if the above search finds no matches
            {
                await Context.ReplyAsync($"the command `{commandName}` does not exist.");
                return;
            }

            response += $"`{search.Name} "; // add the command name

            foreach (var parameter in search.Parameters)
            {
                if (parameter.IsOptional)
                {
                    response += $"[{parameter.Name}] "; // add optional params wrapped in []
                }
                else
                {
                    response += $"<{parameter.Name}> "; // add required params wrapped in <>
                }
            }

            response = response.Remove(response.Length - 1); // remove the whitespace at the end
            response += $"`: {search.Summary}"; // add the command summary

            if (search.Remarks?.Length > 0) // if the command has remarks
            {
                response += $"\n`{search.Name} {search.Remarks}`"; // add an example
            }

            await Context.SendAsync(response);
        }
    }
}