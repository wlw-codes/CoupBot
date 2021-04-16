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
    internal class Program
    {
        private static void Main() => StartAsync().GetAwaiter().GetResult();

        private static async Task StartAsync()
        {
            Credentials credentials;

            try
            {
                credentials = JsonConvert.DeserializeObject<Credentials>(await File.ReadAllTextAsync(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("src")) + "src/Credentials.json"));
            }
            catch (IOException exception)
            {
                Console.WriteLine("The Credentials file was broken or missing!\n\n" + exception);
                return;
            }
            
            var client = new DiscordSocketClient();
            var commandService = new CommandService();
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