using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using CoupBot.Common.Extensions;

namespace CoupBot.Modules.Coups
{
    public partial class Coups
    {
        [Command("vote")]
        [Summary("Formalise your support for either candidate in an ongoing coup.")]
        [Remarks("Leviticus#2026")]
        public async Task Vote([Remainder] IGuildUser user = null)
        {
            var coupSearch = Context.DbGuild.Coups.OrderBy(x => x.TimeInitiated).FirstOrDefault(); // grab the latest coup in the server, or null
            
            if (coupSearch == null) // if the guild has no historic coups
            {
                await Context.ReplyAsync(
                    $"there is no ongoing coup at this time. You can initiate one by using `{Context.DbGuild.Prefix}callcoup`.");
                return;
            }
            
            // code below only executes if the guild has had at least one coup before
            
            var coupExpiresOn =
                coupSearch.TimeInitiated.Add(Context.DbGuild.CoupCampaignTime); // the DateTime the coup expires on
            
            if (DateTime.Now > coupExpiresOn) // if the last active coup is over
            {
                var cooldownOverOn =
                    coupExpiresOn.Add(Context.DbGuild.CoupCooldown); // the DateTime the coup and cooldown expires on
            
                if (DateTime.Now < cooldownOverOn) // if the cooldown is still active
                {
                    await Context.ReplyAsync(
                        $"the owner has enough strength to retain control until at least {cooldownOverOn.ToShortDateString()} at {cooldownOverOn.ToShortTimeString()}.");
                    return;
                }
            
                // code below only executes if the last coup has ended, and the cooldown has expired
            
                await Context.ReplyAsync(
                    $"there is no ongoing coup at this time. You can initiate one by using `{Context.DbGuild.Prefix}callcoup`.");
                return;
            }
            
            // code below only executes if the last coup is ongoing
            
            var challenger = await Context.Guild.GetUserAsync(coupSearch.ChallengerId);
            var ruler = await Context.Guild.GetOwnerAsync();
            
            if (user == null) // if the user parameter was left empty (defaults to null)
            {
                await Context.ReplyAsync(
                    $"in this coup, you can vote for:\n\nRuler: **{ruler.Username}**\nChallenger: **{challenger.Username}**");
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
                    $"in this coup, you can only vote for:\n\nRuler: **{ruler.Username}**\nChallenger: **{challenger.Username}**");
                return;
            }

            var votesToGive = await Context.GuildUser.GetMessageCount();

            if (user == challenger)
            {
                await _guildRepository.ModifyAsync(Context.DbGuild,
                    x => x.Coups.OrderBy(y => y.TimeInitiated).First().VotesForChallenger += votesToGive); // add votes to challenger
            }
            else
            {
                await _guildRepository.ModifyAsync(Context.DbGuild,
                    x => x.Coups.OrderBy(y => y.TimeInitiated).First().VotesForRuler += votesToGive); // add votes to ruler
            }

            await ReplyAsync($"you have successfully given **{votesToGive}** votes to **{user.Username}**.");
        }
    }
}