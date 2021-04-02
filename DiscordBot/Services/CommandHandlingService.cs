﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _services = services;
            _commandService = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();

            // Hook CommandExecuted to handle post-command-execution logic
            _commandService.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see if it qualifies as a command
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            _ = Task.Run(async () =>
            {
                // Ignore system message, or message from other bots
                if (rawMessage is not SocketUserMessage message) return;
                //if (message.Source != MessageSource.User) return;

                // This value holds the offset where the prefix ends
                var argPos = 0;
                // Perform prefix check
                if (!message.HasCharPrefix('!', ref argPos)) return;

                var context = new SocketCommandContext(_discord, message);
                // Perform the execution of the command. In this method, the command service will perform precodition
                // and parsing check then execute the command if one is matched
                await _commandService.ExecuteAsync(context, argPos, _services);
                // Note that normally a result will be returned by this format, but here
                // we will handle the result in CommandExecutedAsync
            });
            return Task.CompletedTask;
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found) -> dont care about these ERR
            if (!command.IsSpecified) return;

            // the command was successful, dont care about this result, unless want to log that a command succeeded
            if (result.IsSuccess) return;

            // the command failed, let's notify the user that something happened
            await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}
