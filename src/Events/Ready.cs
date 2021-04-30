using System;
using System.Threading.Tasks;
using CoupBot.Common;
using Discord.WebSocket;

namespace CoupBot.Events
{
    public class Ready
    {
        private readonly DiscordSocketClient _client;

        public Ready(DiscordSocketClient client)
        {
            _client = client;

            _client.Ready += HandleReadyAsync;
        }

        private async Task HandleReadyAsync()
        {
            await _client.SetGameAsync(Configuration.Game);

            Console.WriteLine(
                $"{_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator} ({_client.CurrentUser.Id}) v{Configuration.Version} has connected to Discord.");
        }
    }
}