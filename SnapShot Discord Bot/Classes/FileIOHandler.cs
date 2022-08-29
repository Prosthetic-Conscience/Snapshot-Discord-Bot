using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SnapShot_Discord_Bot.Services
{
    class FileIOHandler
    {
        public static string executionPath(string extended)
        {
            var filePath = new StringBuilder();
            filePath.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString()));
            filePath.Append(extended);
            return filePath.ToString();

        }

    }


}
