using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Xml;
using System.IO;
using System.Net;
using System.Linq;

public class XML_Typereader : TypeReader
{
    public override Task<TypeReaderResult> ReadAsync(ICommandContext Context, string input, IServiceProvider services)
    {
        try
        {
            var attachmentURL = Context.Message.Attachments.First().Url;
            //if (attachmentURL != "" && attachmentURL != null)
            if (String.IsNullOrEmpty(attachmentURL))
            {
                WebClient myWebClient = new WebClient();
                // Download the resource and load the bytes into a buffer.
                byte[] buffer = myWebClient.DownloadData(attachmentURL);
                // Encode the buffer into UTF-8
                string download = Encoding.UTF8.GetString(buffer);

                //Todo, Test the type reader
                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.LoadXml(download);
                //XMLDoc.Load(xmlAttachment);
                return Task.FromResult(TypeReaderResult.FromSuccess(XMLDoc));
            }
            else
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, "XML Attachment not found."));
        }
        catch (System.Xml.XmlException e)
        {
            return Task.FromResult(TypeReaderResult.FromError(CommandError.Exception, "XML Exception : " + e.Message));
        }
        catch (WebException e)
        {
            return Task.FromResult(TypeReaderResult.FromError(CommandError.Exception, "WebException : " + e.Message));
        }
        catch (IOException e)
        {
            return Task.FromResult(TypeReaderResult.FromError(CommandError.Exception, "IO exception : " + e.Message));
        }
        catch
        { 
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as XML."));
        }
    }
}

