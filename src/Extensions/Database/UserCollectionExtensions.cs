using System;
using System.Threading.Tasks;
using CoupBot.Database.Models;
using MongoDB.Driver;

namespace CoupBot.Common.Extensions.Database
{
    public static class UserCollectionExtensions
    {
        private static UpdateDefinition<User> GetFactory(ulong userId, ulong guildId)
            => new UpdateDefinitionBuilder<User>()
                .SetOnInsert(x => x.UserId, userId)
                .SetOnInsert(x => x.GuildId, guildId);

        public static Task<User> GetUserAsync(this IMongoCollection<User> collection, ulong userId, ulong guildId)
            => collection.GetAsync(x => x.UserId == userId && x.GuildId == guildId, GetFactory(userId, guildId));

        public static Task UpsertUserAsync(this IMongoCollection<User> collection, ulong userId, ulong guildId,
            Action<User> update)
            => collection.UpsertAsync(x => x.UserId == userId && x.GuildId == guildId, update,
                GetFactory(userId, guildId));
    }
}