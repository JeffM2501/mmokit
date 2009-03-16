using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using Lidgren.Network;
using NetworkMessages;


namespace gameserver
{
    public enum LogLevel
    {
        Silent,
        Major,
        Moderate,
        Minor,
        Verbose
    }

    public class ServerConfig
    {
        public static int rootVersion = 1;
        public int version = rootVersion;
        public int port = 6088; // 6088 - 6099 unassigned in iana
        public int maxCon = 128;
        public string name = string.Empty;
        public string description = string.Empty;
        public string map = string.Empty;
        public string host = string.Empty;
        public LogLevel logLevel = LogLevel.Major;
    }

    class GameServer
    {
        static string getParamaterAfter ( string[] args, int index )
        {
            int i = index;
            if ( (i < 0) || (i + 1 >= args.Length) )
                return string.Empty;

            string value = (string)args[i+1].Clone();
            if (value.Contains('"'))
            {
                i += 2;
                while ( i < args.Length && !args[i].Contains('"'))
                    value += " " + args[i];

                if (i >= args.Length)
                    return string.Empty;

               value += " " + args[i];
            }
            return value;
        }

        static ServerConfig getConfig ( string[] args )
        {
            string configFile = getParamaterAfter(args,Array.BinarySearch(args,"-config"));
            if (configFile == string.Empty)
                return new ServerConfig();

            configFile.Replace("\"","");
            FileInfo file = new FileInfo(configFile);

            XmlSerializer xml = new XmlSerializer(typeof(ServerConfig));

            ServerConfig config;
            if (!file.Exists)
            {
                config = new ServerConfig();
                Console.WriteLine("Error reading server config " + configFile + ", using default");
            }
            else
            {
                FileStream fs = file.OpenRead();
                StreamReader reader = new StreamReader(fs);
                try
                {
                    config = (ServerConfig)xml.Deserialize(reader);

                    // TODO, use XML transfoms to upgrade the config version
                    if (config.version != ServerConfig.rootVersion)
                    {
                        Console.WriteLine("Config File is version " + config.version.ToString() + " current version is " + ServerConfig.rootVersion.ToString() + " using default");
                        config = new ServerConfig();
                    }
                }
                catch (System.Exception e)
                {
                    config = new ServerConfig();
                    Console.WriteLine("Error reading server config " + configFile + " ( " + e.ToString() + ") using default");
                }
            }

            string configOutFile = getParamaterAfter(args, Array.BinarySearch(args, "-writeconfig"));

            if (configOutFile != string.Empty)
            {
                configOutFile.Replace("\"", "");
                file = new FileInfo(configOutFile);

                FileStream fs = file.OpenWrite();
                StreamWriter writer = new StreamWriter(fs);
                xml.Serialize(writer, config);
                writer.Close();
                fs.Close();
                Console.WriteLine("Wrote Config file to " + configOutFile);
            }

            return config;
           
        }

        static void Main(string[] args)
        {
            ServerConfig serverConfig = getConfig(args);

            NetConfiguration config = new NetConfiguration("TestApp");
            config.MaxConnections = serverConfig.maxCon;
            config.Port = serverConfig.port;

            ServerApp server = new ServerApp(serverConfig, new NetServer(config));
            try
            {
                while (server.run())
                {
                    // check for exits and other non game stuff here if we want to later
                    Thread.Sleep(1);
                }
            }
            finally
            {
                server.shutdown();
            }
        }
    }


    public class ServerApp
    {
        NetServer server;
        ServerConfig config;

        GameManager game;

        public ServerApp (ServerConfig c, NetServer s)
        {
            server = s;
            config = c;

            game = new GameManager(s);
            server.Start();
        }

        void log ( LogLevel level, string message )
        {
            if (level >= config.logLevel)
                Console.WriteLine(level.ToString() + ":" + message);
        }

        public void shutdown ( )
        {
            game = null;
            server.Shutdown("Server Quit");
        }

        public bool run ( )
        {
            NetMessageType type;
            NetConnection sender;
            NetBuffer buffer = server.CreateBuffer();

            while (server.ReadMessage(buffer, out type, out sender))
            {
                switch (type)
                {
                    case NetMessageType.StatusChanged:
                        statusChange(sender);
                        break;
                    case NetMessageType.Data:
                        dataMessage(sender, buffer);
                        break;
                }
            }
            return true;
        }

        void dissconect ( NetConnection sender )
        {
            if (sender.Tag == null)
                log(LogLevel.Moderate, "Client " + sender.RemoteEndpoint.ToString() + " disconnected and was not in the users list");
            else
                game.PlayerQuit(sender);
        }

        void connect ( NetConnection sender )
        {
            if (sender.Tag != null)
                log(LogLevel.Moderate, "Client " + sender.RemoteEndpoint.ToString() + " connected and was already in the users list");
            else
                game.PlayerQuit(sender);
        }

        void statusChange ( NetConnection sender )
        {
            switch(sender.Status)
            {
                case NetConnectionStatus.Disconnected:
                    dissconect(sender);
                    break;
                case NetConnectionStatus.Connected:
                    connect(sender);
                    break;
            }
        }

        void dataMessage ( NetConnection sender, NetBuffer buffer )
        {
            game.PlayerMessage(sender,MessageBuffer.GetType(buffer),buffer);
        }
    }
}
