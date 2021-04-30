using System;
using System.Threading.Tasks;
using CoupBot.Common.Extensions.Database;
using CoupBot.Database.Models;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CoupBot.Common
{
    public class Context : ICommandContext
    {
        private readonly IMongoCollection<Guild> _dbGuilds;
        private readonly IMongoCollection<User> _dbUsers;

        public User DbUser { get; private set; }
        public Guild DbGuild { get; private set; }
        public IDiscordClient Client { get; }
        public IGuild Guild { get; }
        public IMessageChannel Channel { get; }
        public ITextChannel TextChannel { get; }
        public IUser User { get; }
        public IGuildUser GuildUser { get; }
        public IUserMessage Message { get; }

        public Context(IDiscordClient client, IUserMessage message, IServiceProvider serviceProvider)
        {
            _dbGuilds = serviceProvider.GetRequiredService<IMongoCollection<Guild>>();
            _dbUsers = serviceProvider.GetRequiredService<IMongoCollection<User>>();

            Client = client;
            Message = message;
            Channel = message.Channel;
            TextChannel = message.Channel as ITextChannel;
            Guild = TextChannel?.Guild;
            User = message.Author;
            GuildUser = User as IGuildUser;
        }

        public async Task InitialiseAsync()
        {
            if (Guild != null)
            {
                DbGuild = await _dbGuilds.GetGuildAsync(Guild.Id);
                DbUser = await _dbUsers.GetUserAsync(GuildUser.Id, GuildUser.GuildId);
            }
        }

        public async Task<IUserMessage> SendAsync(string text)
            => await Channel.SendMessageAsync(text);

        public async Task<IUserMessage> ReplyAsync(string text)
            => await SendAsync($"{User.Mention}, {text}");

        public async Task<IUserMessage> DmAsync(string text, IUser user = null)
        {
            user ??= User; // if the user parameter is null, user becomes the user who ran the command

            var userDm = await user.GetOrCreateDMChannelAsync(); // open the DM with them

            try
            {
                await userDm.SendMessageAsync(text);

                if (Channel != userDm) // if the channel the command was ran in is not the DM channel
                {
                    return await ReplyAsync("check your DMs.");
                }
            }
            catch // catch exception where the user's privacy settings block messages from bots
            {
                return await ReplyAsync(
                    "please go into User Settings > Privacy & Safety > Allow direct messages from server members (apply to all members).");
            }

            return null;
        }
    }
}