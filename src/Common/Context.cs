using System;
using System.Threading.Tasks;
using CoupBot.Database.Models;
using CoupBot.Database.Repositories;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CoupBot.Common
{
    public class Context : SocketCommandContext
    {
        public User DbUser { get; set; }
        public Guild DbGuild { get; set; }
        public IGuildUser GuildUser { get; }

        private readonly UserRepository _userRepo;
        private readonly GuildRepository _guildRepo;

        public Context(SocketUserMessage msg, IServiceProvider serviceProvider, DiscordSocketClient client = null) : base(client, msg)
        {
            _userRepo = serviceProvider.GetService<UserRepository>();
            _guildRepo = serviceProvider.GetService<GuildRepository>();

            GuildUser = User as IGuildUser;
        }

        public async Task InitializeAsync()
        {
            DbGuild = await _guildRepo.GetGuildAsync(Guild.Id);
            DbUser = await _userRepo.GetUserAsync(GuildUser.Id, GuildUser.GuildId);
        }
    }
}
