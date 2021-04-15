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
    class Program
    {
        static void Main() => new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            Credentials credentials;

            try
            {
                credentials = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("src")) + "src/Credentials.json"));
            }
            catch (IOException exception)
            {
                Console.WriteLine("The Credentials file was broken or missing!\n\n" + exception.ToString());
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