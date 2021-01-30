﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BackendAPI.Utils
{
    public class XMLUtils
    {
      //  public static List<PluginLoader> PluginList =  new List<PluginLoader>();
        protected static Type[] extraTypes;
 
    
       

        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml)) {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder())) {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
       


    }
}
