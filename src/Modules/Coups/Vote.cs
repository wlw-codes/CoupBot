using System.Linq;
using System.Threading.Tasks;
using CoupBot.Common.Extensions.Database;
using Discord;
using Discord.Commands;
using CoupBot.Common.Extensions.Discord;
using CoupBot.Common.Extensions.Models;
using CoupBot.Common.Preconditions.Command;

namespace CoupBot.Modules.Coups
{
    public partial class Coups
    {
        [Command("vote")]
        [Summary("Formalise your support for either candidate in an ongoing coup.")]
        [Remarks("Leviticus#2026")]
        [RequireOngoingCoup]
        public async Task Vote([Remainder] IGuildUser user = null)
        {
            var coupSearch =
                Context.DbGuild
                    .GetCurrentCoup(); // grab the latest coup in the server. we don't need to check for null as the precondition does it for us

            var challenger = await Context.Guild.GetUserAsync(coupSearch.ChallengerId);
            var ruler = await Context.Guild.GetOwnerAsync();

            if (user == null) // if the user parameter was left empty (defaults to null)
            {
                await Context.ReplyAsync(
                    $"in this coup, you can vote for:\n\nRuler: 🔸**{ruler.Username}**\nChallenger: 🔹**{challenger.Username}**");
                return;
            }

            if (Context.User.Id == challenger.Id ||
                Context.User.Id == ruler.Id) // if the user who ran the command is a party in the coup
            {
                await Context.ReplyAsync("you cannot vote as you are a party in this coup!");
                return;
            }

            if (user != challenger && user != ruler) // if the user parameter is not a party in the coup
            {
                await Context.ReplyAsync(
                    $"in this coup, you can only vote for:\n\nRuler: 🔸**{ruler.Username}**\nChallenger: 🔹**{challenger.Username}**");
                return;
            }

            var votesToGive = await Context.GuildUser.GetMessageCount();
            var userName = "";
            
            if (user == challenger)
            {
                userName = $"🔹**{challenger.Username}**";
                
                await _dbGuilds.UpsertGuildAsync(Context.DbGuild.GuildId,
                    x => x.Coups.OrderBy(y => y.TimeInitiated).First().VotesForChallenger +=
                        votesToGive); // add votes to challenger
            }
            else
            {
                userName = $"🔸**{ruler.Username}**";
                
                await _dbGuilds.UpsertGuildAsync(Context.DbGuild.GuildId,
                    x => x.Coups.OrderBy(y => y.TimeInitiated).First().VotesForRuler +=
                        votesToGive); // add votes to ruler
            }

            await Context.ReplyAsync($"you have successfully given **{votesToGive}** votes to {userName}.");
        }
    }
}