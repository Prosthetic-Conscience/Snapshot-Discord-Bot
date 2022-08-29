using System;
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

namespace SnapShot_Discord_Bot.Commands
{
    public class FunStuff : ModuleBase<SocketCommandContext>
    {
        [Command("Goose")]
        [Summary("HONK")]
        public async Task HonkAsync()
        {
            Console.WriteLine("Command Goose Received");
            //var filePath = new StringBuilder();
            //filePath.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString()));// + "\\Resources\\goose.png";
            //filePath.Append("\\Resources\\goose.png");
            string filePath = Services.FileIOHandler.executionPath("//Resources//Images//goose.png");
            //filePath = filePath + "\\Resources\\Iamges\\goose.png";
            await Context.Channel.SendFileAsync(filePath.ToString(), "HONK");
        }

        [Command("Naptime")]
        [Summary("..snore..")]
        public async Task SleepyAsync()
        {
            Console.WriteLine("Command Naptime Received");
            //var filePath = new StringBuilder();
            //filePath.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString()));// + "\\Resources\\littlefox.png";
            string filePath = Services.FileIOHandler.executionPath("//Resources//Images//littlefox.png");
            //filePath = filePath + "\\Resources\\Images\\littlefox.png";
            await Context.Channel.SendFileAsync(filePath.ToString(), "..shutting down..snore..");
            Environment.Exit(1);
        }
    }
}
