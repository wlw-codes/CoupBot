using System;
using System.Threading.Tasks;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.Coup
{
    public partial class Coup
    {
        [Command("callcoup")]
        [Summary("Initiate a coup upon the server owner.")]
        public async Task CallCoup()
        {
            if (Context.DbGuild.CoupActive)
            {
                await ReplyErrorAsync("there is already a coup active in this server. Please wait <time left> until it ends.");
                return;
            }
            
            var confirmationMessage = await ReplyAsync($"are you sure you wish to call a coup upon **{Context.Guild.Owner.Username}**?\n\nReply with \"yes\" to continue, or anything else to cancel. You may also ignore this message for {Configuration.CoupAttemptCancelTime} seconds.");
            var response = await _interactiveService.NextMessageAsync(Context, timeout: TimeSpan.FromSeconds(Configuration.CoupAttemptCancelTime));

            if (response is not { Content: "yes"} )
            {
                await confirmationMessage.ModifyAsync(x =>
                    x.Content = "You have decided not to initiate the coup attempt.");
                
                await response.DeleteAsync();
            }
            else
            {
                await confirmationMessage.ModifyAsync(x => x.Content = "Coup initiated.");
                await response.DeleteAsync();
            }
        }
    }
}