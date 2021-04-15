using CoupBot.Common.Structures;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace CoupBot.Services
{
    public sealed class ServiceManager
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly Credentials _credentials;

        public IServiceProvider ServiceProvider { get; }

        public ServiceManager(DiscordSocketClient client, CommandService commandService, Credentials credentials)
        {
            _client = client;
            _commandService = commandService;
            _credentials = credentials;

            var database = ConfigureDatabase();

            var services = new ServiceCollection()
             // Parameters
             .AddSingleton(_client)
             .AddSingleton(_commandService)
             .AddSingleton(_credentials);
             // DB Models
             //.AddSingleton(database.GetCollection<Guild>("guilds"))
             //.AddSingleton(database.GetCollection<User>("users"))
             // DB Repositories
             //.AddSingleton<GuildRepository>()
             //.AddSingleton<UserRepository>()
             // Events
             //.AddSingleton<MessageReceived>()
             //.AddSingleton<Ready>()
             //.AddSingleton<UserJoined>()
             //.AddSingleton<UserLeft>()
             // Handlers
             //.AddSingleton<ErrorHandler>()
             // Services
             //.AddSingleton<Text>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public IMongoDatabase ConfigureDatabase()
        {
            var mongoClient = new MongoClient(_credentials.DatabaseConnectionString);

            return mongoClient.GetDatabase(_credentials.DatabaseName);
        }

        public void InitialiseTimersAndEvents()
        {
            //new MessageReceived(_commandService, ServiceProvider);
            //new Ready(_client);
            //new UserJoined(_client, ServiceProvider);
            //.AddSingleton<UserLeft>()
        }
    }
}
