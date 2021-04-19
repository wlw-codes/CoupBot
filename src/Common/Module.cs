using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CoupBot.Common
{
    public abstract class Module : ModuleBase<Context>
    {
        private readonly IServiceProvider _serviceProvider;

        public Module(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync(string message)
        {
            await Context.Channel.SendMessageAsync(message);
        }

        public async Task ReplyAsync(string message)
        {
            await Context.Channel.SendMessageAsync(Context.User.Mention + $", {message}");
        }

        public async Task ReplyErrorAsync(string message)
        {
            await Context.Channel.SendMessageAsync("**Command error!**" + $"\n\n{message}");
        }

        public async Task DmAsync(IUser user, string message)
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
