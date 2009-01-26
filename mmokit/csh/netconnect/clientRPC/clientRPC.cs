using System;
using System.Collections.Generic;
using System.Text;
using NetworkCallers;

namespace ClientRPCSystem
{
    public delegate void ClientCallback( string n, CallingParam p );

    public class ClientRPCManager
    {
        Dictionary<string, ClientCallback> callbacks = new Dictionary<string,ClientCallback>();

        public void register(string name, ClientCallback callback)
        {
            if (callbacks.ContainsKey(name))
        }
    }
}
