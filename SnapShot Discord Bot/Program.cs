using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SnapShot_Discord_Bot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IConfigurationRoot _config;
        private ServiceCollection _services;
        private CommandHandlingService CommandHandler;

        static void Main(string[] args)
        {
            // Call the Program constructor, followed by the 
            //setExecutionString();
            // MainAsync method and wait until it finishes (which should be never).
            //TODO get closing handler setup to run on ARM
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(closingHandlerAsync);
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        /*
        //Listener for Ctrl+C Ctrl+x in debug terminal
        private static async void closingHandlerAsync(object sender, ConsoleCancelEventArgs args)
        {   
            Console.WriteLine("CANCEL command received! Cleaning up. please wait...");
            //await _client.StopAsync;
            //_client.SetStatusAsync(UserStatus.Offline);
            Environment.Exit(0);
        }
        */

        public async Task MainAsync()
        {
        _client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            LogLevel = LogSeverity.Verbose,
            MessageCacheSize = 500
        });

        _commands = new CommandService(new CommandServiceConfig()
            {
            LogLevel = LogSeverity.Verbose,
            // Case-insensitive commands.
            CaseSensitiveCommands = false,
            });
        
        _services = new ServiceCollection();
        _services.AddSingleton(_client);
        _services.AddSingleton(_commands);
        _services.AddSingleton<CommandHandlingService>();
        _services.AddSingleton<LoggingService>();
        //_services.AddSingleton<LoggingService>();
        var _provider = _services.BuildServiceProvider();

        _client.Log += LoggingService.Log;
        _commands.Log += LoggingService.Log;

        string token_data = File.ReadAllText(Services.FileIOHandler.executionPath("//Resources//Token.txt"));
        //string token_data = Properties.Resources.Token;
        await _client.LoginAsync(TokenType.Bot, token_data);
        //await _client.LoginAsync(TokenType.Bot, SnapShot_Discord_Bot.Properties.Resources.Token);
        await _client.StartAsync();
        CommandHandler = new CommandHandlingService(_client, _commands, _config, _provider);
        await CommandHandler.InstallCommandsAsync();
        // Block this task until the program is closed.
        await Task.Delay(-1);
        }
    }
}
