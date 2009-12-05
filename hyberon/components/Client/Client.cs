using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

using Lidgren.Network;

namespace Clients
{
    public class MonitoringEventArgs : EventArgs
    {
        public String Message;

        public MonitoringEventArgs(string msg)
            : base()
        {
            Message = msg;
        }
    }

    public delegate void MonitoringEvent(object sender, MonitoringEventArgs args);

    public class Client
    {
        public event MonitoringEvent Connect;
        public event MonitoringEvent Reject;
        public event MonitoringEvent Disconnect;
        public event MonitoringEvent DebugMessage;
        public event MonitoringEvent Quiting;

        public static NetChannel GeneralChannel = NetChannel.ReliableInOrder1;

        protected NetConfiguration netConfig;

        protected NetClient     client;

        Thread netThread;

        protected bool connected = false;

        protected List<NetBuffer> PendingMessages = new List<NetBuffer>();

        NetBuffer buffer;

        protected string host = string.Empty;
        protected int port = 2501;

        protected class PendingSend
        {
            public NetBuffer message;
            public NetChannel channel;
        }

        List<PendingSend> PendingSends = new List<PendingSend>();
        
        public Client()
        {
            Init();
        }

        public Client( string _host, int _port )
        {
            host = _host;
            port = _port;
            Init();
        }

        protected virtual void Init()
        {
            netConfig = new NetConfiguration("GameApp");
            netThread = new Thread(new ThreadStart(Run));
            netThread.Start();
        }

        protected virtual void Run()
        {
            connected = false;
            client = new NetClient(netConfig);
            client.Connect(host, port, Encoding.ASCII.GetBytes("Hail"));

            client.SetMessageTypeEnabled(NetMessageType.ConnectionRejected, true);
			client.SetMessageTypeEnabled(NetMessageType.DebugMessage, true);

            buffer = client.CreateBuffer();

            while (true)
            {
                CheckUpdates();
                Thread.Sleep(1);
            }
        }

        protected virtual NetBuffer ProcessOutboundMessage(NetBuffer message)
        {
            return message;
        }

        protected virtual NetBuffer ProcessInboundMessage(NetBuffer message)
        {
            return message;
        }

        public virtual void SendMessage(NetBuffer message, NetChannel channel)
        {
            PendingSend msg = new PendingSend();
            msg.message = ProcessOutboundMessage(message);
            msg.channel = channel;
            lock (PendingSends)
            {
                PendingSends.Add(msg);
            }
        }

        protected virtual void ServerData( NetBuffer message )
        {
            lock(PendingMessages)
            {
                NetBuffer data = ProcessInboundMessage(buffer);
                if (data != null)
                    PendingMessages.Add(data);
            }
        }

        protected virtual void Connected(NetBuffer data)
        {
            if (Connect != null)
                Connect(this, new MonitoringEventArgs("Connection from " + data.ToString()));
        }

        protected virtual void Disconnected(NetBuffer data)
        {
            if (Disconnect != null)
                Disconnect(this, new MonitoringEventArgs(data.ToString() + " disconnected"));
        }

        protected virtual void Rejected(NetBuffer data)
        {
            if (Reject != null)
                Reject(this, new MonitoringEventArgs(data.ToString() + " rejected"));
        }

        public virtual NetBuffer GetPentMessage()
        {
            NetBuffer msg;
            lock (PendingMessages)
            {
                if (PendingMessages.Count == 0)
                    return null;

                msg = PendingMessages[0];
                PendingMessages.RemoveAt(0);
            }
            return msg;
        }

        protected virtual void CheckUpdates()
        {
            NetMessageType type;

            bool gotOne = false;

            lock (client)
            {
                gotOne = client.ReadMessage(buffer, out type);
            }

            while (gotOne)
            {
                switch (type)
                {
                    case NetMessageType.DebugMessage:
                    case NetMessageType.VerboseDebugMessage:
                        if (DebugMessage != null)
                            DebugMessage(this, new MonitoringEventArgs(buffer.ReadString()));
                        break;

                    case NetMessageType.ConnectionRejected:
                        Rejected(buffer);
                        connected = false;
                        break;

                    case NetMessageType.ServerDiscovered:
                        Disconnected(buffer);
                        connected = false;
                        break;

                    case NetMessageType.StatusChanged:
                        lock(client)
                        {
                            if (client.Status == NetConnectionStatus.Connected)
                            {
                                connected = true;
                                Connected(buffer);
                            }
                        }
                        break;

                    case NetMessageType.Data:
                        ServerData(buffer);
                        break;
                }

                lock (client)
                {
                    gotOne = client.ReadMessage(buffer, out type);
                }
            }

            lock (PendingSends)
            {
                foreach (PendingSend send in PendingSends)
                {
                    lock (client)
                    {
                        client.SendMessage(send.message, send.channel);
                    }
                }
                PendingSends.Clear();
            }
        }

        public virtual void Kill()
        {
            lock (client)
            {
                lock (PendingSends)
                {
                    PendingSends.Clear();
                }
                lock (PendingMessages)
                {
                    PendingMessages.Clear();
                }

                client.Disconnect("Quiting");
                connected = false;
                netThread.Abort();
                client = null;
                netThread = null;

                if (Quiting != null)
                    Quiting(this, new MonitoringEventArgs("Kill"));
            }
        }
    }
}
