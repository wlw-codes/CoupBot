using System;
using System.Linq;
using System.Threading.Tasks;
using CoupBot.Common;
using CoupBot.Common.Extensions.Database;
using CoupBot.Common.Extensions.Discord;
using CoupBot.Common.Extensions.Models;
using CoupBot.Common.Preconditions.Command;
using CoupBot.Common.Structures;
using Discord.Commands;
using Discord.WebSocket;

namespace CoupBot.Modules.Coups
{
    public partial class Coups
    {
        [Command("callcoup")]
        [Summary("Initiate a coup against the server owner.")]
        [RequireNoOngoingCoup]
        [NotOwner]
        public async Task CallCoup()
        {
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

            var totalMessages = await Context.Guild.GetMessageCountAsync();

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