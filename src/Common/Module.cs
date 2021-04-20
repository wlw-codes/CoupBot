using System;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;

namespace CoupBot.Common
{
    public abstract class Module : ModuleBase<Context>
    {
        protected async Task<RestUserMessage> SendAsync(string message)
        {
            return await Context.Channel.SendMessageAsync(message);
        }

        protected async Task<RestUserMessage> ReplyAsync(string message)
        {
            return await Context.Channel.SendMessageAsync(Context.User.Mention + $", {message}");
        }

        protected async Task<RestUserMessage> ReplyErrorAsync(string message)
        {
            return await Context.Channel.SendMessageAsync("**Command error!**" + $"\n\n{message}");
        }

        protected async Task DmAsync(IUser user, string message)
        {
            var userDm = await user.GetOrCreateDMChannelAsync();

            try
            {
                await userDm.SendMessageAsync(message);

                if (Context.Channel != userDm)
                {
                    await Context.Message.AddReactionAsync(new Emoji("📬"));
                }
            }
            catch
            {
                await ReplyErrorAsync("Please go into User Settings > Privacy & Safety > Allow direct messages from server members (apply to all members).");
            }
        }
    }
}
