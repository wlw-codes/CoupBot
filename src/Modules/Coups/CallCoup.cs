using System;
using System.Linq;
using System.Threading.Tasks;
using CoupBot.Common;
using CoupBot.Common.Extensions.Database;
using CoupBot.Common.Extensions.Discord;
using CoupBot.Common.Structures;
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
            var coupSearch = Context.DbGuild.Coups.OrderBy(x => x.TimeInitiated).FirstOrDefault(); // grab the latest coup in the server, or null

            if (coupSearch != null) // if there is at least one coup
            {
                var waitUntil = coupSearch.TimeInitiated;

                if (DateTime.Now < waitUntil.Add(Context.DbGuild.CoupCampaignTime)) // if the coup is still ongoing
                {
                    waitUntil = waitUntil.Add(Context.DbGuild.CoupCampaignTime);

                    await Context.ReplyAsync(
                        $"there is already a coup active in this server. Please wait until the coup ends on {waitUntil.ToShortDateString()} at {waitUntil.ToShortTimeString()} to initiate another.");
                    return;
                }

                if (DateTime.Now < coupSearch.TimeInitiated.Add(Context.DbGuild.CoupCooldown)
                    .Add(Context.DbGuild.CoupCampaignTime)) // if the coup cooldown is still ongoing
                {
                    waitUntil = waitUntil.Add(Context.DbGuild.CoupCooldown).Add(Context.DbGuild.CoupCampaignTime);

                    await Context.ReplyAsync(
                        $"the owner has enough strength to retain control until at least {waitUntil.ToShortDateString()} at {waitUntil.ToShortTimeString()}.");
                    return;
                }
            }
            
            // code below only executes if there is no coup that is active and if the cooldown is over from the last one

            if (Context.Guild.OwnerId == Context.User.Id) // if the owner of the server executes the command
            {
                await Context.ReplyAsync("you cannot call a coup against yourself!");
                return;
            }

            var confirmationMessage = await Context.ReplyAsync(
                $"are you sure you wish to call a coup against **{await Context.Guild.GetOwnerAsync()}**?\n\nReply with \"yes\" to continue, or anything else to cancel. You may also ignore this message for {Configuration.CoupAttemptCancelTime} seconds.");
            var response = await _interactiveService.NextMessageAsync(
                new SocketCommandContext((DiscordSocketClient) Context.Client, (SocketUserMessage) Context.Message),
                timeout: TimeSpan.FromSeconds(Configuration
                    .CoupAttemptCancelTime)); // we have to register a new SocketCommandContext as our usual Context is of type ICommandContext

            if (response is not {Content: "yes"}) // if the message is null, or the content is not "yes"
            {
                await confirmationMessage.ModifyAsync(x =>
                    x.Content = "You have decided not to initiate the coup attempt.");

                await response.DeleteAsync(); // try and delete the response, will silently fail if response is null
                return;
            }

            var totalMessages = await Context.Guild.GetMessageCount();

            await _dbGuilds.UpsertGuildAsync(Context.DbGuild.GuildId, x => x.Coups.Add(new Coup
            {
                ChallengerId = Context.User.Id,
                TimeInitiated = DateTime.Now,
                TotalPossibleVotes = totalMessages
            }));

            await confirmationMessage.ModifyAsync(x =>
                x.Content =
                    $"Coup initiated. You have {Context.DbGuild.CoupCampaignTime.TotalHours}h to campaign, or until >50% of the {totalMessages} votes available are distributed to a single candidate. Good luck!");
            await response.DeleteAsync();
        }
    }
}