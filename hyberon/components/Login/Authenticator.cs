using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Clients;
using Lidgren.Network;

namespace Login
{
    public class Authenticator
    {
        CryptoClient client;
        Int64 token = 0;
        UInt64 UID = 0;

        static int LoginMessage = 2;
        static int LoginAccept = 3;
        static int LoginDeny = 0;

        protected string username = string.Empty;
        protected string password = string.Empty;

        public Authenticator( string host, int port, string user, string pass )
        {
            username = user;
            password = pass;
            client = new CryptoClient(host, port);
            client.Connect += new MonitoringEvent(Connected);
            client.Disconnect += new MonitoringEvent(client_Disconnect);
        }

        void client_Disconnect(object sender, MonitoringEventArgs args)
        {
            token = -2;
            Kill();
        }

        public void Kill()
        {
            if (client != null)
                client.Kill();

            client = null;
        }

        void Connected(object sender, MonitoringEventArgs args)
        {
            NetBuffer buffer = new NetBuffer();
            buffer.Write(LoginMessage);
            buffer.Write(username);
            buffer.Write(password);
            client.SendMessage(buffer, NetChannel.ReliableInOrder2);
        }

        public bool Authenticated ()
        {
            if (token != 0)
                return true;

            NetBuffer buffer = client.GetPentMessage();
            while (buffer != null)
            {
                int code = buffer.ReadInt32();
                if (code == LoginAccept)
                {
                    token = buffer.ReadInt64();
                    UID = buffer.ReadUInt64();
                }
                else if (code == LoginDeny)
                    token = -1;

                buffer = client.GetPentMessage();
            }

            if (token != 0)
            {
                client.Kill();
                client = null;
            }

            return (token != 0);
        }

        public Int64 GetToken ()
        {
            return token;
        }

        public UInt64 GetUID()
        {
            return UID;
        }
    }
}
