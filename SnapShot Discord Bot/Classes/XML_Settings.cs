using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SnapShot_Discord_Bot.Services
{
    public class XML_Settings
    {
        public static XmlWriterSettings XMLSettingsLocal()
        {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = new UTF8Encoding(false);
        //settings.Encoding = new UnicodeEncoding();
        //settings.Encoding = System.Text.UnicodeEncoding.Unicode;
        settings.ConformanceLevel = ConformanceLevel.Auto;
        settings.NewLineOnAttributes = true;
        return settings;
        }

        //public XmlWriter  XMLWriterLocal(string filepath)
        //{


        //}



    }
}
