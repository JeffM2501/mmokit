using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using Axiom;
using Axiom.Core;

namespace _3dSpeeders
{
    public class Game 
    {
        public GameVisual visual;
        public ClientConfig config;
        public ConnectionInfo conInfo;

        NetClient client;

        public Game (ClientConfig _clientConfig, ConnectionInfo _conInfo )
        {
            visual = new GameVisual(_conInfo);

            config = _clientConfig;
            conInfo = _conInfo;

            NetConfiguration netConfig = new NetConfiguration("3dSpeeders");
            client = new NetClient(netConfig);
        }

        public bool run ()
        {
            if (!init())
                return false;

            Root.Instance.StartRendering();

            shutdown();

            return true;
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
