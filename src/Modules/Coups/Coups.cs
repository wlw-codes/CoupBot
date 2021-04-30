using CoupBot.Common;
using CoupBot.Database.Models;
using Discord.Addons.Interactive;
using Discord.Commands;
using MongoDB.Driver;

namespace CoupBot.Modules.Coups
{
    [Name("Coup")]
    [Summary("Commands pertaining to the coup functionality.")]
    [RequireContext(ContextType.Guild)]
    public partial class Coups : ModuleBase<Context>
    {
        private readonly InteractiveService _interactiveService;
        private readonly IMongoCollection<Guild> _dbGuilds;

        public Coups(InteractiveService interactiveService, IMongoCollection<Guild> dbGuilds)
        {
            _interactiveService = interactiveService;
            _dbGuilds = dbGuilds;
        }
    }
}