﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Xml;

namespace SnapShot_Discord_Bot
{
    public class CommandHandlingService
    {
        private static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        public CommandHandlingService(
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config,
            IServiceProvider provider)
            {
            _client = discord;
            _commands = commands;
            _config = config;
            _provider = provider;      
            }
     
        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            //Starting from Discord.NET 2.0, a service provider is required to be passed into the
            // module registration method to inject the required dependencies.
            _commands.AddTypeReader(typeof(XmlDocument), new XML_Typereader());
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
           _commands.CommandExecuted += OnCommandExecutedAsync;
        }
        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // We have access to the information of the command executed,
            // the context of the command, and the result returned from the
            // execution in this event.

            // We can tell the user what went wrong
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
            // ...or even log the result (the method used should fit into
            // your existing log handler)
            var commandName = command.IsSpecified ? command.Value.Name : "A command";
            await LoggingService.Log(new LogMessage(LogSeverity.Info, "CommandExecution", $"{commandName} was executed at {DateTime.UtcNow}."));
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('&', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot) return;
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);
            // Execute the command with the command context we just created, along with the service provider for precondition checks.
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);           
        }
    }   
}
