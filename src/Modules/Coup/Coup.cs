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
        private readonly InteractiveService _interactiveService;

        public Coup(IServiceProvider serviceProvider)
        {
            _interactiveService = serviceProvider.GetRequiredService<InteractiveService>();
        }
    }
}
