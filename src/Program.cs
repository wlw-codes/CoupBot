using CoupBot.Common.Structures;
using CoupBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CoupBot
{
    internal static class Program
    {
        private static void Main() => StartAsync().GetAwaiter().GetResult();

        private static async Task StartAsync()
        {
            Credentials credentials;

            try
            {
                credentials = JsonConvert.DeserializeObject<Credentials>(await File.ReadAllTextAsync(AppContext.BaseDirectory + "../../../Credentials.json"));
            }
            catch
            {
                Console.WriteLine("The credentials file is missing!");
                return;
            }

            if (credentials == null)
            {
                Console.WriteLine("The credentials file is missing data!");
                return;
            }
            
            var client = new DiscordSocketClient();
            var commandService = new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async
            });
            var serviceManager = new ServiceManager(client, commandService, credentials);
            var serviceProvider = serviceManager.ServiceProvider;

            serviceManager.InitialiseTimersAndEvents();

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
            await client.LoginAsync(TokenType.Bot, credentials.Token);
            await client.StartAsync();
            await Task.Delay(-1);
        }
    }
}