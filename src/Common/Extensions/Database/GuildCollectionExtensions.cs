using System;
using System.Threading.Tasks;
using CoupBot.Database.Models;
using MongoDB.Driver;

namespace CoupBot.Common.Extensions.Database
{
    public static class GuildCollectionExtensions
    {
        private static UpdateDefinition<Guild> GetFactory(ulong guildId)
            => new UpdateDefinitionBuilder<Guild>()
                .SetOnInsert(x => x.GuildId, guildId);

        public static Task<Guild> GetGuildAsync(this IMongoCollection<Guild> collection, ulong guildId)
            => collection.GetAsync(x => x.GuildId == guildId, GetFactory(guildId));

        public static Task UpsertGuildAsync(this IMongoCollection<Guild> collection, ulong guildId,
            Action<Guild> update)
            => collection.UpsertAsync(x => x.GuildId == guildId, update, GetFactory(guildId));
    }
}