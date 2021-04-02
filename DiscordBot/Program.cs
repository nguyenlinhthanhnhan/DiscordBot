using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.EntityFrameworkCore.Design;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            // Read token and start bot
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));
            await client.StartAsync();

            // Here initialize the logic required to register our commands
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await services.GetRequiredService<UserEventService>().InitializeAsync();
            services.GetRequiredService<AutoSendMessageService>().Initialize();
            // Block the program until it is closed
            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices()
            => new ServiceCollection().AddDbContext<DataContext>()
                                      .AddSingleton<DiscordSocketClient>()
                                      .AddSingleton<CommandService>()
                                      .AddSingleton<CommandHandlingService>()
                                      .AddSingleton<UserEventService>()
                                      .AddSingleton<AutoSendMessageService>()
                                      .AddSingleton<HttpClient>()
                                      .AddSingleton<IRepositoryManager, RepositoryManager>()
                                      .AddSingleton(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
                                      .AddSingleton<ILeagueRepository, LeagueRepository>()
                                      .AddSingleton<IUserRepository, UserRepository>()
                                      .AddSingleton<IUserLeagueVouchRepository, UserLeagueVouchRepository>()
                                      .AddSingleton<IVouchUserRepository, VouchUserRepository>()
                                      .AddSingleton<IUserService, UserService>()
                                      .AddSingleton<IUserLeagueVouchService, UserLeagueVouchService>()
                                      .AddSingleton<IUserInformationService, UserInformationService>()
                                      .BuildServiceProvider();

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }

}
