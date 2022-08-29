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
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace SnapShot_Discord_Bot.Commands
{
    public class Utility : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Show bot options")]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        public async Task Help()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Info for" + Context.User.Username);
            embed.AddField("!Ping", "Shows client ping");
            embed.AddField("!Botinfo", "Lists Bots hardware status");
            embed.AddField("!Info", "Lists Server information");
            //embed.WithDescription($"{Context.Client.Latency} ms");
            embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
        }

        /*
        [Command("ping")]
        [Summary("Show current latency.")]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        public async Task Ping()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Ping information for" + Context.User.Username);
            embed.WithDescription($"{Context.Client.Latency} ms");
            embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
            //await ReplyAsync($"Latency: {Context.Client.Latency} ms");
        }
        */

        [Command("BotInfo")]
        [Summary("Show bots hardware info")]
        public async Task StatsAsync()
        {
            //var ramUsage = Math.Round((decimal)(Process.GetCurrentProcess().PrivateMemorySize64 / 1000000000), 2);
            var ramUsage = Math.Round((decimal)Environment.WorkingSet / 1000000000, 3);

            //var memTotalLine = File.ReadAllLines("/proc/meminfo");
            //var ramTotalUsages = Math.Round((decimal)System.Diagnostics., 2);    
            
            var CPU_start = Process.GetCurrentProcess().TotalProcessorTime;
            var startTime = DateTime.UtcNow;
            await Task.Delay(500);
            var CPU_end = Process.GetCurrentProcess().TotalProcessorTime;
            var endTime = DateTime.UtcNow;
            var cpuUsedMs = (CPU_end - CPU_start).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = Math.Round((decimal)(cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100), 4);
            var upTime = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
            var upTimeString = $"{upTime.Days}D:{upTime.Hours}H:{upTime.Minutes}M:{upTime.Seconds}S";
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithThumbnailUrl(Context.Guild.CurrentUser.GetAvatarUrl());
            //embed.WithTitle("Bot info for " + Context.User.Username);
            embed.WithTitle("Bot info for " + Context.Guild.CurrentUser.Username);
            embed.AddField("Bot:", $"{Context.Guild.CurrentUser} # {Context.Guild.CurrentUser.Discriminator}", true);
            embed.AddField("Bot id:", Context.Guild.CurrentUser.Id, true);
            //embed.AddField("RAM Usage:", $"{ramUsage}GB of {ramTotalUsages} GB", true);
            embed.AddField("RAM Usage:", $"{ramUsage} GB", true);
            embed.AddField("CPU Cores:", $"{Environment.ProcessorCount}", true);
            embed.AddField("CPU Usage:", $"{cpuUsageTotal}", true);
            //embed.AddField("Shards:", _shardedClient.Shards.Count, true);
            //embed.AddField("Servers:", Context.User.Shards.Sum(x => x.Guilds.Count), true);
            //embed.AddField("Members:", _shardedClient.Shards.SelectMany(shard => shard.Guilds).Sum(guild => guild.MemberCount), true);
            embed.AddField("Avg ping:", Context.Client.Latency, true);
            embed.AddField("Up time:", upTimeString, true);
            embed.WithColor(new Color(255, 255, 255));
            embed.WithCurrentTimestamp();
            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
        }

        [Command("info")]
        [Alias("server", "serverinfo")]
        [Summary("Show server information.")]
        [RequireBotPermission(GuildPermission.EmbedLinks)] // Require the bot the have the 'Embed Links' permissions to execute this command.
        public async Task ServerEmbed()
        {
            double botPercentage = Math.Round(Context.Guild.Users.Count(x => x.IsBot) / Context.Guild.MemberCount * 100d, 2);
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(0, 225, 225)
                .WithDescription(
                    $"🏷️\n**Guild name:** {Context.Guild.Name}\n" +
                    $"**Guild ID:** {Context.Guild.Id}\n" +
                    $"**Created at:** {Context.Guild.CreatedAt:dd/M/yyyy}\n" +
                    $"**Owner:** {Context.Guild.Owner}\n\n" +
                    $"💬\n" +
                    $"**Users:** {Context.Guild.MemberCount - Context.Guild.Users.Count(x => x.IsBot)}\n" +
                    $"**Bots:** {Context.Guild.Users.Count(x => x.IsBot)} [ {botPercentage}% ]\n" +
                    $"**Channels:** {Context.Guild.Channels.Count}\n" +
                    $"**Roles:** {Context.Guild.Roles.Count}\n" +
                    $"**Emotes: ** {Context.Guild.Emotes.Count}\n\n" +
                    //$"🌎 **Region:** {Context.Guild.VoiceRegionId}\n\n" +
                    $"🔒 **Security level:** {Context.Guild.VerificationLevel}")
                 .WithImageUrl(Context.Guild.IconUrl);
            await ReplyAsync($":information_source: Server info for **{Context.Guild.Name}**", embed: embed.Build());
        }    
    }
}
