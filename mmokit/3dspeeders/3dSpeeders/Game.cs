using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace _3dSpeeders
{
    public class Game 
    {
        public GameWindow window;
        public ClientConfig config;
        public ConnectionInfo conInfo;

        NetClient client;

        public Game (ClientConfig _clientConfig, ConnectionInfo _conInfo )
        {
            config = _clientConfig;
            conInfo = _conInfo;
            NetConfiguration netConfig = new NetConfiguration("3dSpeeders");
            client = new NetClient(netConfig);
        }

        public bool init ( )
        {
            return true;
        }

        public void shutdown ()
        {
            client.Disconnect("Client Exit");
            client = null;
        }
    }
}
