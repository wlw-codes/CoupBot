using System;
using System.Linq;
using System.Threading.Tasks;
using CoupBot.Common;
using CoupBot.Common.Structures;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CoupBot.Modules.Coups
{
    public partial class Coups
    {
        [Command("callcoup")]
        [Summary("Initiate a coup against the server owner.")]
        public async Task CallCoup()
        {
            var coupSearch = Context.DbGuild.Coups.OrderBy(x => x.TimeInitiated).FirstOrDefault();
            
            if (coupSearch != null)
            {
                var waitUntil = coupSearch.TimeInitiated;
                
                if (DateTime.Now < waitUntil.Add(Context.DbGuild.CoupCampaignTime))
                {
                    waitUntil = waitUntil.Add(Context.DbGuild.CoupCampaignTime);
                
                    await Context.ReplyAsync($"there is already a coup active in this server. Please wait until the coup ends on {waitUntil.ToShortDateString()} at {waitUntil.ToShortTimeString()} to initiate another.");
                    return;
                }
            
                if (DateTime.Now < coupSearch.TimeInitiated.Add(Context.DbGuild.CoupCooldown).Add(Context.DbGuild.CoupCampaignTime))
                {
                    waitUntil = waitUntil.Add(Context.DbGuild.CoupCooldown).Add(Context.DbGuild.CoupCampaignTime);
                
                    await Context.ReplyAsync($"the owner has enough strength to retain control until at least {waitUntil.ToShortDateString()} at {waitUntil.ToShortTimeString()}.");
                    return;
                }
            }
            
            if (Context.Guild.OwnerId == Context.User.Id)
            {
                await Context.ReplyAsync("you cannot call a coup against yourself!");
                return;
            }
            
            var confirmationMessage = await Context.ReplyAsync($"are you sure you wish to call a coup against **{await Context.Guild.GetOwnerAsync()}**?\n\nReply with \"yes\" to continue, or anything else to cancel. You may also ignore this message for {Configuration.CoupAttemptCancelTime} seconds.");
            var response = await _interactiveService.NextMessageAsync(new SocketCommandContext((DiscordSocketClient)Context.Client, (SocketUserMessage)Context.Message), timeout: TimeSpan.FromSeconds(Configuration.CoupAttemptCancelTime));
            
            if (response is not { Content: "yes"} )
            {
                await confirmationMessage.ModifyAsync(x =>
                    x.Content = "You have decided not to initiate the coup attempt.");
                
                await response.DeleteAsync();
            }
            else
            {
                var totalMessages = 0;
                
                foreach (var textChannel in await Context.Guild.GetTextChannelsAsync())
                {
                    var messageCount = await textChannel.GetMessagesAsync(int.MaxValue).FlattenAsync();
                    totalMessages += messageCount.Count() -1;
                }
                
                await _guildRepository.ModifyAsync(Context.DbGuild, x => x.Coups.Add(new Coup
                {
                    ChallengerId = Context.User.Id,
                    TimeInitiated = DateTime.Now,
                    TotalPossibleVotes = totalMessages
                }));
                
                await confirmationMessage.ModifyAsync(x => x.Content = $"Coup initiated. You have {Context.DbGuild.CoupCampaignTime.TotalHours}h to campaign, or until >50% of the {totalMessages} votes available are distributed to a single candidate. Good luck!");
                await response.DeleteAsync();
            }
        }
    }
}