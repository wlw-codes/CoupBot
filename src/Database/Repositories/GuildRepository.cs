using System.Threading.Tasks;
using CoupBot.Database.Models;
using MongoDB.Driver;

namespace CoupBot.Database.Repositories
{
    public class GuildRepository : BaseRepository<Guild>
    {
        public GuildRepository(IMongoCollection<Guild> guilds) : base(guilds) { }

        public async Task<Guild> GetGuildAsync(ulong guildId)
        {
            var dbGuild = await GetAsync(x => x.GuildId == guildId);

            if (dbGuild != default) return dbGuild;
            
            var createdGuild = new Guild(guildId);

            await InsertAsync(createdGuild);

            return createdGuild;

        }
    }
}
