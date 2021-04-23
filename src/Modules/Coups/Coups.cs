using CoupBot.Common;
using CoupBot.Database.Repositories;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace CoupBot.Modules.Coups
{
    [Name("Coup")]
    [Summary("Commands pertaining to the coup functionality.")]
    public partial class Coups : ModuleBase<Context>
    {
        private readonly InteractiveService _interactiveService;
        private readonly GuildRepository _guildRepository;

        public Coups(InteractiveService interactiveService, GuildRepository guildRepository)
        {
            _interactiveService = interactiveService;
            _guildRepository = guildRepository;
        }
    }
}
