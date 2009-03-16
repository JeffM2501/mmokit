using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace _3dSpeeders
{
    public class Game 
    {
        GameWindow window;
        NetClient client;

        public Game (GameWindow w)
        {
            NetConfiguration netConfig = new NetConfiguration("3dSpeeders");
            client = new NetClient(netConfig);
            window = w;
        }

        public void shutdown ()
        {
            client.Disconnect("Client Exit");
            client = null;
        }
    }
}
