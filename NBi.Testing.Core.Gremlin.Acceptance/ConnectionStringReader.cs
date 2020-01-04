using System;
using System.Xml;

namespace NBi.Testing.Core.Gremlin
{
    class ConnectionStringReader
    {
        public static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }

        
        public static string GetCosmosDb() => Get("CosmosDb");
        public static string GetGremlin() => Get("Gremlin");

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
                return "ConnectionString.user.config";
            else if (System.IO.File.Exists("ConnectionString.config"))
                return "ConnectionString.config";
            return "";
        }
    }
}
