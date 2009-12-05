using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ServerConfigurator
{
    public class ServerConfig
    {
        public class KeyPair
        {
            public string key = string.Empty;
            public string value = string.Empty;

            public KeyPair()
            {}

            public KeyPair (string k, string v)
            {
                key = k;
                value = v;
            }

            public KeyPair(KeyValuePair<string,string> pair)
            {
                key = pair.Key;
                value = pair.Value;
            }
        }

        public List<KeyPair> ConfigItems = new List<KeyPair>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        Dictionary<string, string> items = new Dictionary<string, string>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        FileInfo configFile = null;

        protected ServerConfig ()
        {
        }

        public ServerConfig(string file)
        {
            configFile = new FileInfo(file);
            if (configFile.Exists)
            {
                XmlSerializer XML = new XmlSerializer(typeof(ServerConfig));

                StreamReader reader = configFile.OpenText();
                ServerConfig cfg = (ServerConfig)XML.Deserialize(reader);
                ConfigItems = cfg.ConfigItems;

                reader.Close();

                foreach (KeyPair item in ConfigItems)
                    items.Add(item.key, item.value);

                ConfigItems.Clear();
            }

        }

        public void Save ()
        {
            ConfigItems.Clear();
            foreach (KeyValuePair<string, string> item in items)
                ConfigItems.Add(new KeyPair(item));

            if (configFile != null)
            {
                XmlSerializer XML = new XmlSerializer(typeof(ServerConfig));

                FileStream stream = configFile.OpenWrite();
                StreamWriter writer = new StreamWriter(stream);
                XML.Serialize(writer,this);
                writer.Close();
                stream.Close();
            }

            ConfigItems.Clear();
        }

        public string GetItem ( string name )
        {
            if (items.ContainsKey(name))
                return items[name];

            return string.Empty;
        }

        public void SetItem ( string name, string value )
        {
            if (items.ContainsKey(name))
                items[name] = value;
            else
                items.Add(name, value);
        }
    }
}
