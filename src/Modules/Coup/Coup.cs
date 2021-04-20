using System;
using CoupBot.Common;
using Discord.Addons.Interactive;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CoupBot.Modules.Coup
{
    [Name("Coup")]
    [Summary("Commands pertaining to the coup functionality.")]
    public partial class Coup : Module
    {
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractiveService _interactiveService;

        public Coup(CommandService commandService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _interactiveService = _serviceProvider.GetRequiredService<InteractiveService>();
        }
    }
}
