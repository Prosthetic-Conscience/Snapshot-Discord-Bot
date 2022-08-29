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
using System.Xml;
using System.Net.Http;
using System.Net;

namespace SnapShot_Discord_Bot.Commands
{
    public class Parse : ModuleBase<SocketCommandContext>
    {

        [Command("Roles")]
        [Alias("Role")]
        [Summary("Lists Server Roles")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task getRolesAsync()
        {
            string filePath = Services.FileIOHandler.executionPath("//Resources//Roles.xml");
            //filePath = filePath + "\\Resources\\Roles.xml";
            XmlWriterSettings settings = Services.XML_Settings.XMLSettingsLocal();
            XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
            var serverRoles = Context.Guild.Roles;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Roles");
            foreach (var role in serverRoles)
            {
                xmlWriter.WriteStartElement("Role");
                xmlWriter.WriteAttributeString("Name", role.Name);
                xmlWriter.WriteAttributeString("ID", role.Id.ToString());
                xmlWriter.WriteAttributeString("Color", role.Color.ToString());
                xmlWriter.WriteAttributeString("Position", role.Position.ToString());
                xmlWriter.WriteAttributeString("Created", role.CreatedAt.ToString());
                var roleMembers = role.Members;
                xmlWriter.WriteStartElement("Members");
                foreach (var user in roleMembers)
                {
                    xmlWriter.WriteElementString("Member", user.Username);

                }
                //TODO Role Permissions
                
                var rolePermissions = role.Permissions;
                xmlWriter.WriteStartElement("Permissions");
                xmlWriter.WriteAttributeString("AddReactions", rolePermissions.AddReactions.ToString());
                xmlWriter.WriteAttributeString("Administrator", rolePermissions.Administrator.ToString());
                xmlWriter.WriteAttributeString("AttachFiles", rolePermissions.AttachFiles.ToString());
                xmlWriter.WriteAttributeString("BanMembers", rolePermissions.BanMembers.ToString());
                xmlWriter.WriteAttributeString("ChangeNickname", rolePermissions.ChangeNickname.ToString());
                xmlWriter.WriteAttributeString("Connect", rolePermissions.Connect.ToString());
                xmlWriter.WriteAttributeString("CreateInstantInvite", rolePermissions.CreateInstantInvite.ToString());
                xmlWriter.WriteAttributeString("DeafenMembers", rolePermissions.DeafenMembers.ToString());
                xmlWriter.WriteAttributeString("EmbedLinks", rolePermissions.EmbedLinks.ToString());
                xmlWriter.WriteAttributeString("ManageChannel", rolePermissions.ManageChannels.ToString());
                xmlWriter.WriteAttributeString("ManageMessages", rolePermissions.ManageMessages.ToString());
                xmlWriter.WriteAttributeString("ManageRoles", rolePermissions.ManageRoles.ToString());
                xmlWriter.WriteAttributeString("ManageWebhooks", rolePermissions.ManageWebhooks.ToString());
                xmlWriter.WriteAttributeString("MentionEveryone", rolePermissions.MentionEveryone.ToString());
                xmlWriter.WriteAttributeString("MoveMembers", rolePermissions.MoveMembers.ToString());
                xmlWriter.WriteAttributeString("MuteMembers", rolePermissions.MuteMembers.ToString());
                xmlWriter.WriteAttributeString("PrioritySpeaker", rolePermissions.PrioritySpeaker.ToString());
                xmlWriter.WriteAttributeString("ReadMessageHistory", rolePermissions.ReadMessageHistory.ToString());
                xmlWriter.WriteAttributeString("SendMessages", rolePermissions.SendMessages.ToString());
                xmlWriter.WriteAttributeString("SendTTSMessages", rolePermissions.SendTTSMessages.ToString());
                xmlWriter.WriteAttributeString("Speak", rolePermissions.Speak.ToString());
                xmlWriter.WriteAttributeString("Stream", rolePermissions.Stream.ToString());
                xmlWriter.WriteAttributeString("UseExternalEmojis", rolePermissions.UseExternalEmojis.ToString());
                xmlWriter.WriteAttributeString("UseVAD", rolePermissions.UseVAD.ToString());
                xmlWriter.WriteAttributeString("ViewChannel", rolePermissions.ViewChannel.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            await Context.Channel.SendFileAsync(filePath);
        }


        [Command("Channels")]
        [Alias("Channel")]
        [Summary("Generates xml Channel listing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task getChannelsAsync()
        {
            string filePath = Services.FileIOHandler.executionPath("//Resources//Channels.xml");
            //filePath = filePath + "\\Resources\\Channels.xml";
            XmlWriterSettings settings = Services.XML_Settings.XMLSettingsLocal();
            XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Channels");
            //var guildTextChannels = Context.Guild.Channels.Where(x => !(x is ICategoryChannel || x is IVoiceChannel));
            var guildTextChannels = Context.Guild.Channels;
            foreach (var channel in guildTextChannels)
            {
                xmlWriter.WriteStartElement("Channel");
                xmlWriter.WriteAttributeString("Name", channel.Name);
                var xmlChannelType = channel.GetType();
                xmlWriter.WriteAttributeString("Type", xmlChannelType.Name);
                xmlWriter.WriteAttributeString("ID", channel.Id.ToString());
                xmlWriter.WriteAttributeString("Position", channel.Position.ToString());
                //var permissionOveridesList = channel.PermissionOverwrites.ToList();
                //string combinedString = string.Join(",", permissionOveridesList);
                //xmlWriter.WriteAttributeString("Permissions", permissionOveridesList.First().ToString());
                xmlWriter.WriteStartElement("Users");
                var users = channel.Users;
                foreach(var user in users)
                {
                    xmlWriter.WriteStartElement("User");
                    xmlWriter.WriteAttributeString("UserName", user.Username);
                    xmlWriter.WriteAttributeString("Nickname" , user.Nickname);
                    xmlWriter.WriteAttributeString("ID", user.Id.ToString());
                    var permissionList = user.GetPermissions(channel);
                    //string combinedString = string.Join(",", permissionList);
                    xmlWriter.WriteStartElement("Permissions");
                    xmlWriter.WriteAttributeString("AddReactions", permissionList.AddReactions.ToString());
                    xmlWriter.WriteAttributeString("AttachFiles", permissionList.AttachFiles.ToString());
                    xmlWriter.WriteAttributeString("Connect", permissionList.Connect.ToString());
                    xmlWriter.WriteAttributeString("CreateInstantInvite", permissionList.CreateInstantInvite.ToString());
                    xmlWriter.WriteAttributeString("DeafenMembers", permissionList.DeafenMembers.ToString());
                    xmlWriter.WriteAttributeString("EmbedLinks", permissionList.EmbedLinks.ToString());
                    xmlWriter.WriteAttributeString("ManageChannel", permissionList.ManageChannel.ToString());
                    xmlWriter.WriteAttributeString("ManageMessages", permissionList.ManageMessages.ToString());
                    xmlWriter.WriteAttributeString("ManageRoles", permissionList.ManageRoles.ToString());
                    xmlWriter.WriteAttributeString("ManageWebhooks", permissionList.ManageWebhooks.ToString());
                    xmlWriter.WriteAttributeString("MentionEveryone", permissionList.MentionEveryone.ToString());
                    xmlWriter.WriteAttributeString("MoveMembers", permissionList.MoveMembers.ToString());
                    xmlWriter.WriteAttributeString("MuteMembers", permissionList.MuteMembers.ToString());
                    xmlWriter.WriteAttributeString("PrioritySpeaker", permissionList.PrioritySpeaker.ToString());
                    xmlWriter.WriteAttributeString("ReadMessageHistory", permissionList.ReadMessageHistory.ToString());
                    xmlWriter.WriteAttributeString("SendMessages", permissionList.SendMessages.ToString());
                    xmlWriter.WriteAttributeString("SendTTSMessages", permissionList.SendTTSMessages.ToString());
                    xmlWriter.WriteAttributeString("Speak", permissionList.Speak.ToString());
                    xmlWriter.WriteAttributeString("Stream", permissionList.Stream.ToString());
                    xmlWriter.WriteAttributeString("UseExternalEmojis", permissionList.UseExternalEmojis.ToString());
                    xmlWriter.WriteAttributeString("UseVAD", permissionList.UseVAD.ToString());
                    xmlWriter.WriteAttributeString("ViewChannel", permissionList.ViewChannel.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            await Context.Channel.SendFileAsync(filePath);
        }


        [Command("RemoveDupes")]
        [Summary("Echoes a message.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        //public async Task removeDuplicateUsersAsync(XmlDocument xmlUsers)
        public async Task removeDuplicateUsersAsync()
        {
            string inputString = "";
            var attachmentURL = Context.Message.Attachments.First().Url;
            WebClient myWebClient = new WebClient();
            // Download the resource and load the bytes into a buffer.
            byte[] buffer = myWebClient.DownloadData(attachmentURL);
            // Encode the buffer into UTF-8
            string download = Encoding.UTF8.GetString(buffer);
            XmlDocument xmlUsers = new XmlDocument();
            xmlUsers.PreserveWhitespace = true;
            //xmlUsers.Load(new StringReader(attachments));
            xmlUsers.LoadXml(download);
            foreach (XmlNode node in xmlUsers.GetElementsByTagName("User"))
            //foreach (XmlNode node in xmlUsers.SelectNodes("/Duplicate_Usernames/User"))
            {
                inputString += node.Attributes["UserName"].Value;
                inputString += " : ";
                inputString += node.Attributes["Duplicate_Count"].Value;
            }
            //TODO remove/ban users in XML list
            await Context.Channel.SendMessageAsync(inputString);
        }

        // _logger.LogCommandUsed(Context.Guild?.Id, Context.Client.ShardId, Context.Channel.Id, Context.User.Id, "BotInfo");     

        [Command("Users")]
        [Summary("Lists limited user info")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ListUsersAsync([Remainder]string arguments ="")
        {            
        //If no argument dump list of users and roles
        if (String.IsNullOrEmpty(arguments))
            {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithThumbnailUrl(Context.Guild.CurrentUser.GetAvatarUrl());
            //embed.WithTitle("Bot info for " + Context.User.Username);
            embed.WithTitle("Help information for" + Context.User.Username);
            embed.AddField("!List", "Lists Users and Roles on a server");
            embed.AddField("Arguments:", "None yet");
            embed.AddField("!Duplicates", "Lists duplicate usernames on a server");
            embed.AddField("Arguments:", "None yet");
            //embed.AddField("Bot id:", Context.Guild.CurrentUser.Id, true);
            embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, embed.Build()).ConfigureAwait(false);
            return;
            }
        else
        arguments = arguments.ToLower();
        char[] charSeparators = new char[] {'=', ';', '|' , ' '};
        string[] arg = arguments.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);            
        if (arg[0] == "list")
            {
            //TODO add usernick argument
            string filePath = Services.FileIOHandler.executionPath("//Resources//Users.xml");
            //filePath = filePath + "\\Resources\\Users.xml";

            XmlWriterSettings settings = Services.XML_Settings.XMLSettingsLocal();
            XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("UserList");
            var guildUsers = Context.Guild.GetUsersAsync(RequestOptions.Default);
            foreach (var user in Context.Guild.Users)
            {
                xmlWriter.WriteStartElement("User_Info");
                xmlWriter.WriteAttributeString("Name", user.Username);
                xmlWriter.WriteAttributeString("ID", user.Id.ToString());
                xmlWriter.WriteAttributeString("NickName", user.Nickname);
                xmlWriter.WriteAttributeString("Heirarchy", user.Hierarchy.ToString());
                xmlWriter.WriteAttributeString("Is_Bot", user.IsBot.ToString());
                xmlWriter.WriteStartElement("User_Roles");
                var userRoles = user.Roles;
                foreach (var role in userRoles)
                    {
                    //if (role.ToString() != "@everyone")
                    //    {
                            //xmlWriter.WriteString(role.ToString() + ",");
                            //xmlWriter.WriteAttributeString("Role" , role.ToString());
                            xmlWriter.WriteElementString("Role", role.ToString());
                     //   }
                    }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                }
            //}
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            await Context.Channel.SendFileAsync(filePath);
            }
            if(arg[0] == "duplicates")
                {
                Console.WriteLine("Searching for duplicated usernames");
                var guildUsers = Context.Guild.GetUsersAsync(RequestOptions.Default);
                Dictionary<ulong, string> guildList = new Dictionary<ulong, string>();
                foreach (var user in Context.Guild.Users)
                    {
                    guildList.Add(user.Id, user.Username);
                    }
                //guildList.Add(65465435135416541, "bob");
                //guildList.Add(6546543435351416541, "bob");
                //guildList.Add(342344351387854651, "bob");
                var duplicateValues = guildList.GroupBy(x => x.Value).Where(g => g.Count() > 1).Select( y => new { Username = y.Key , Counter = y.Count() }).ToList();
                if(duplicateValues.Count > 0)
                {
                    string filePath = Services.FileIOHandler.executionPath("//Resources//Users.xml");
                    //filePath = filePath + "\\Resources\\Users.xml";
                    XmlWriterSettings settings = Services.XML_Settings.XMLSettingsLocal();
                    XmlWriter xmlWriter = XmlWriter.Create(filePath, settings);
                    xmlWriter.WriteStartDocument();
                    //xmlWriter.Sch
                    xmlWriter.WriteStartElement("Duplicate_Usernames");
                    foreach (var dupe in duplicateValues)
                        {
                        xmlWriter.WriteStartElement("User");
                        xmlWriter.WriteAttributeString("Duplicate_Count", dupe.Counter.ToString());
                        xmlWriter.WriteAttributeString("UserName", dupe.Username);
                        xmlWriter.WriteEndElement();
                        //xmlWriter.WriteEndAttribute();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    
                    xmlWriter.Close();
                    await Context.Channel.SendFileAsync(filePath);
                }
            else 
                {
                    await Context.Channel.SendMessageAsync("No duplicates found");
                }
            }
        }
    }
}
/*
 *public static void Main()
	{
		Dictionary<ulong, string> guildList = new Dictionary<ulong, string>();
		guildList.Add(15235345, "Bob");
		guildList.Add(155235355, "Bob");
		guildList.Add(152465345, "bot");
		guildList.Add(1645535345, "Bob");
		guildList.Add(15245645, "dave");
		guildList.Add(14568235345, "Bob");
		guildList.Add(1529657345, "Bob");
		guildList.Add(15273756345, "Bob");
		guildList.Add(1523756345, "Bill");
		guildList.Add(1523867956345, "Bob");
		guildList.Add(152375567876345, "Bill");
		var duplicateValues = guildList.GroupBy(x => new { x.Value, x.Key}).Where(g => g.Count() > 1)
		//var duplicateValues = guildList.GroupBy(x => x.Value).Where(g => g.Count() > 1).Select(y => new { Nick = y.Key, Counter = y.Count() }).ToList();
		 //var duplicateValues = guildList.GroupBy(x => x.Value).Where(g => g.Count() > 1).Select( y => new { Nick = y.Key , Counter = y.Count() }).ToList();		
		//var duplicateValues = guildList.ToLookup(x => x.Value, x => x.Key).Where(x => x.Count() > 1);
		foreach (var dupe in duplicateValues)
                {
                Console.WriteLine(dupe.
				//Console.WriteLine(dupe.Nick.ToString() +" " + dupe.Counter.ToString() +" " + dupe.);
				//Console.WriteLine(dupe.Key);
                }
	}
}
*/