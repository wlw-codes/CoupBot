using System;
using CoupBot.Common;
using Discord.Commands;

namespace CoupBot.Modules.System
{
    [Name("System")]
    [Summary("Commands pertaining to the core function of the bot.")]
    public partial class System : Module
    {
        private readonly CommandService _commandService;

        public System(CommandService commandService)
        {
            _commandService = commandService;
        }
    }
}
