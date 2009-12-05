using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Hosts;
using UserDatabase;
using Lidgren.Network;

namespace LoginServer
{
    class Program
    {
        public class ConnectedUser
        {
            UInt64 UID = 0;
            Int64 Token = -5;
        }

        static int LoginMessage = 2;
        static int LoginAccept = 3;
        static int LoginDeny = 0;

        static void Main(string[] args)
        {
            string configPath = "./config.xml";
            if (args.Length > 0)
                configPath = args[0];

            Dictionary<NetConnection, ConnectedUser> ConnectedUsers = new Dictionary<NetConnection, ConnectedUser>();

            Users users = new Users(configPath);
            if (!users.Valid())
            {
                Console.WriteLine("Database connection failed");
                return;
            }
            Console.WriteLine("User database startup " + users.UserCount().ToString() + " users found");

            CryptoHost host = new CryptoHost(2500);

            while (true)
            {
                NetConnection connection = host.GetPentConnection();
                while (connection != null)
                {
                    ConnectedUsers.Add(connection, new ConnectedUser());
                    connection = host.GetPentConnection();
                }

                connection = host.GetPentDisconnection();
                while (connection != null)
                {
                    ConnectedUsers.Remove(connection);
                    connection = host.GetPentDisconnection();
                }

                Message msg = host.GetPentMessage();

                while (msg != null)
                {
                    if (msg.Name == LoginMessage)
                    {
                        // it's a login attempt, check the data and see what's up
                        string username = msg.Data.ReadString();
                        string password = msg.Data.ReadString();
                        UInt64 UID = 0;

                        Int64 token = users.AuthUser(username, password, ref UID);
                        if (token < 0)
                        {
                            NetBuffer buffer = new NetBuffer();
                            buffer.Write(LoginDeny);
                            host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder2);
                            host.DisconnectUser(msg.Sender);
                        }
                        else
                        {
                            NetBuffer buffer = new NetBuffer();
                            buffer.Write(LoginAccept);
                            buffer.Write(UID);
                            buffer.Write(token);
                            host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder2);
                        }
                    }
                    msg = host.GetPentMessage();
                }

                users.CheckDeadUsers(300.0);

                Thread.Sleep(100);
            }
        }
    }
}
