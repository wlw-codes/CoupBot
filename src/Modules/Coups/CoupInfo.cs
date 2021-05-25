using System;
using System.Threading.Tasks;
using CoupBot.Common.Extensions.Models;
using CoupBot.Common.Preconditions.Command;
using Discord.Commands;

namespace CoupBot.Modules.Coups
{
    public partial class Coups
    {
        [Command("coupinfo")]
        [Summary("View detailed information about the most recent coup.")]
        [RequireOngoingCoup]
        public async Task CoupInfo()
        {
            var coupSearch =
                Context.DbGuild
                    .GetCurrentCoup(); // grab the latest coup in the server. we don't need to check for null as the precondition does it for us
            var challenger = await Context.Guild.GetUserAsync(coupSearch.ChallengerId);
            var owner = await Context.Guild.GetOwnerAsync();
            var initiated =
                $"**{coupSearch.TimeInitiated.ToShortDateString()}** at **{coupSearch.TimeInitiated.ToShortTimeString()}**";
            var expires = coupSearch.TimeInitiated.Add(Context.DbGuild.CoupCampaignTime).Subtract(DateTime.Now)
                .Hours;
            var message = "";

            message +=
                $"__Current coup__\nInitiated by: 🔹**{challenger.Username}**\nAgainst: 🔸**{owner.Username}**\nInitiated on: {initiated}\nExpires in: **{expires}h**\nTotal possible votes: **{coupSearch.TotalPossibleVotes}**";

            if (coupSearch.VotesForChallenger + coupSearch.VotesForRuler > 0) // if there are any votes placed
            {
                message +=
                    $"\nVote distribution: 🔸**{coupSearch.VotesForRuler}** | 🔹**{coupSearch.VotesForChallenger}** | ▫️**{coupSearch.TotalPossibleVotes - (coupSearch.VotesForRuler + coupSearch.VotesForChallenger)}**";
            }

            await Context.SendAsync(message);
        }
    }
}