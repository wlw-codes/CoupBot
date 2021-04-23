using System;
using System.Threading.Tasks;
using CoupBot.Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CoupBot.Events
{
    public class MessageReceived
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        //private readonly ErrorHandler _errorHandler;

        public MessageReceived(IServiceProvider serviceProvider, CommandService commandService)
        {
            _serviceProvider = serviceProvider;
            _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            _commandService = commandService;
            //_errorHandler = _serviceProvider.GetRequiredService<ErrorHandler>();
            
            _client.MessageReceived += HandleMessageAsync;
        }

        private async Task HandleMessageAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage {Source: MessageSource.User} message) return;

            var argPos = 0;
            var context = new Context(_client, message, _serviceProvider);

            await context.InitializeAsync();
            
            if (context.Channel is not IDMChannel)
            {
                if (!message.HasStringPrefix(context.DbGuild.Prefix, ref argPos)) return;
            }
            else
            {
                if (!message.HasStringPrefix(Configuration.Prefix, ref argPos)) return;
            }

            var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

            //if (!result.IsSuccess) await _errorHandler.HandleCommandErrorAsync(result, context);
        }
    }
}
