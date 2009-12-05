using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;
using System.Threading;

namespace Hosts
{
    public class Message
    {
        public Message( NetBuffer buffer, NetConnection sender)
        {
            Sender = sender;
            Data = buffer;
            Name = Data.ReadInt32();
        }

        public int Name;
        public NetConnection Sender;
        public NetBuffer Data;
    }

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
    public delegate void AllowEvent(object sender, NetConnection connection, out bool allow, out String reason);

    public class Host
    {
        public event MonitoringEvent Connect;
        public event MonitoringEvent Disconnect;
        public event MonitoringEvent DebugMessage;
        public event MonitoringEvent Quiting;
        public event AllowEvent AllowConnection;

        public static NetChannel GeneralChannel = NetChannel.ReliableInOrder1;

        protected NetConfiguration netConfig;
        protected NetServer           server;
        Thread              netThread;
        protected object locker = new object();
        NetBuffer           buffer;

        protected List<Message> PendingMessages = new List<Message>();
        protected List<NetConnection> NewConnections = new List<NetConnection>();
        protected List<NetConnection> NewDeletions = new List<NetConnection>();

        private List<NetConnection> connectedUsers = new List<NetConnection>();

        protected class PendingSend
        {
            public NetBuffer message;
            public NetChannel channel;
            public NetConnection to;
        }

        List<PendingSend> PendingSends = new List<PendingSend>();

        List<NetConnection> PendingDeletes = new List<NetConnection>();

        public Host (int port)
        {
            Init(port);
        }

        public Host()
        {
            Init(2501);
        }

        protected virtual int GetConnections ()
        {
            return 128;
        }

        protected virtual void Init (int port)
        {
            netConfig = new NetConfiguration("GameApp");
            netConfig.MaxConnections = GetConnections();
            netConfig.Port = port;

            netThread = new Thread(new ThreadStart(Run));
            netThread.Start();
        }

        protected virtual void Run()
        {
            // create server and start listening for connections
            server = new NetServer(netConfig);
            server.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
            server.Start();

            buffer = server.CreateBuffer();

            while (true)
            {
                CheckUpdates();
                Thread.Sleep(1);
            }
        }

        protected virtual NetBuffer ProcessOutboundMessage ( NetConnection to, NetBuffer message)
        {
            return message;
        }

        protected virtual NetBuffer ProcessInboundMessage ( NetConnection from, NetBuffer message)
        {
            return message;
        }

        public virtual void SendMessage ( NetConnection to, NetBuffer message, NetChannel channel )
        {
            PendingSend msg = new PendingSend();
            msg.message = ProcessOutboundMessage(to,message);
            msg.to = to;
            msg.channel = channel;
            lock(PendingSends)
            {
                PendingSends.Add(msg);
            }
        }

        public virtual void Broadcast(NetBuffer data, NetChannel channel)
        {
            PendingSend msg = new PendingSend();
            msg.message = ProcessOutboundMessage(null, data);
            msg.to = null;
            msg.channel = channel;
            lock (PendingSends)
            {
                PendingSends.Add(msg);
            }
        }

        public virtual void DisconnectUser ( NetConnection user )
        {
            lock(PendingDeletes)
            {
                PendingDeletes.Add(user);
            }
        }

        public virtual Message GetPentMessage()
        {
            Message msg;
            lock(locker)
            {
                if (PendingMessages.Count == 0)
                    return null;

                msg = PendingMessages[0];
                PendingMessages.RemoveAt(0);
            }
            return msg;
        }

        public virtual NetConnection GetPentConnection()
        {
            NetConnection user;
            lock (locker)
            {
                if (NewConnections.Count == 0)
                    return null;

                user = NewConnections[0];
                NewConnections.RemoveAt(0);
            }
            return user;
        }

        public virtual NetConnection GetPentDisconnection()
        {
            NetConnection user;
            lock (locker)
            {
                if (NewDeletions.Count == 0)
                    return null;

                user = NewDeletions[0];
                NewDeletions.RemoveAt(0);
            }
            return user;
        }

        protected virtual void UserConnected ( NetConnection sender )
        {
            if (Connect != null)
                Connect(this, new MonitoringEventArgs("Connection from " + sender.ToString()));

            NewConnections.Add(sender);
        }

        protected virtual void UserDisconnected (NetConnection sender)
        {
            if (Disconnect != null)
                Disconnect(this, new MonitoringEventArgs(sender.ToString() + " disconnected"));
            NewDeletions.Add(sender);
        }

        protected virtual void UserData ( NetConnection sender, NetBuffer buffer )
        {
            lock (sender)
            {
                NetBuffer data = ProcessInboundMessage(sender,buffer);
                if (data != null)
                    PendingMessages.Add(new Message(data, sender));
            }
        }

        protected virtual void CheckUpdates()
        {
            NetMessageType type;
            NetConnection sender;
            bool gotOne = false;
            lock(locker)
            { 
                gotOne = server.ReadMessage(buffer, out type, out sender);
            }

            while (gotOne)
            {
                lock (locker)
                {
                    switch (type)
                    {
                        case NetMessageType.DebugMessage:
                            if (DebugMessage != null)
                                DebugMessage(this, new MonitoringEventArgs(buffer.ReadString()));
                            break;

                        case NetMessageType.ConnectionApproval:
                            {
                                bool allow = true;
                                String reason = String.Empty;
                                if (AllowConnection != null)
                                    AllowConnection(this, sender, out allow, out reason);

                                if (allow)
                                    sender.Approve();
                                else
                                    sender.Disapprove(reason);
                            }
                            break;

                        case NetMessageType.StatusChanged:
                            lock(sender)
                            {
                                switch(sender.Status)
                                {
                                    case NetConnectionStatus.Connected:
                                        if (!connectedUsers.Contains(sender))
                                        {
                                            connectedUsers.Add(sender);
                                            UserConnected(sender);
                                        }
                                        break;
                                        
                                    case NetConnectionStatus.Disconnected:
                                        if (connectedUsers.Contains(sender))
                                        {
                                            connectedUsers.Remove(sender);
                                            UserDisconnected(sender);
                                        }
                                        break;
                                }
                            }

                            break;

                        case NetMessageType.Data:
                            UserData(sender, buffer);
                            break;
                    }
                    gotOne = server.ReadMessage(buffer, out type, out sender);
                }
            }

            lock (PendingSends)
            {
                foreach(PendingSend send in PendingSends)
                {
                    lock (locker)
                    {
                        if (send.message != null)
                        {
                            if (send.to == null)
                                server.SendToAll(send.message, send.channel);
                            else
                                server.SendMessage(send.message, send.to, send.channel);
                        }
                    }
                }

                PendingSends.Clear();
            }

            lock(PendingDeletes)
            {
                foreach (NetConnection con in PendingDeletes)
                {
                    con.Disconnect("Disconnected", 1);
                }

                PendingDeletes.Clear();
            }
        }

        public virtual void Kill()
        {
            lock(locker)
            {
                lock(PendingSends)
                {
                    PendingSends.Clear();
                }
                lock(PendingMessages)
                {
                    PendingMessages.Clear();
                }
                lock (NewConnections)
                {
                    NewConnections.Clear();
                }
                lock (NewDeletions)
                {
                    NewDeletions.Clear();
                }

                connectedUsers.Clear();
                server.Shutdown("Quiting");
                netThread.Abort();
                server = null;
                netThread = null;

                if (Quiting != null)
                    Quiting(this, new MonitoringEventArgs("Kill"));
            }
        }
    }
}
