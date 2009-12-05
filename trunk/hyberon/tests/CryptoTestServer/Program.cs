using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Lidgren.Network;

using Hosts;

namespace CryptoTestServer
{
    class Program
    {
        static object locker = new object();

        static List<NetConnection> clients = new List<NetConnection>();

        static void Main(string[] args)
        {
            CryptoHost host = new CryptoHost();

            host.Connect +=new MonitoringEvent(host_Connect);
            host.Disconnect +=new MonitoringEvent(host_Disconnect);
            host.DebugMessage +=new MonitoringEvent(host_DebugMessage);
            while (true)
            {
                lock (locker)
                {
                    NetConnection newConnect = host.GetPentConnection();
                    while (newConnect != null)
                    {
                        Console.WriteLine("Connection");
                        clients.Add(newConnect);
                        NetBuffer buffer = new NetBuffer();
                        buffer.Write(1);
                        buffer.Write("hey");
                        host.SendMessage(newConnect, buffer, NetChannel.ReliableInOrder2);
                        Console.WriteLine("sent value");
                        newConnect = host.GetPentConnection();
                    }

                    NetConnection newDisconnect = host.GetPentDisconnection();
                    while (newDisconnect != null)
                    {
                        clients.Remove(newDisconnect);
                        newDisconnect = host.GetPentDisconnection();
                    }

                    Message msg = host.GetPentMessage();
                    while (msg != null)
                    {
                        Console.WriteLine(msg.Data.ToString());
                        msg = host.GetPentMessage();
                    }
                }
                Thread.Sleep(100);
            }
        }

        static void host_Disconnect(object sender, MonitoringEventArgs args)
        {
            lock(locker)
            {
                Console.WriteLine(args.Message + "\r\n");
            }
        }

        static void host_DebugMessage(object sender, MonitoringEventArgs args)
        {
            lock (locker)
            {
                Console.WriteLine(args.Message + "\r\n");
            }
        }

        static void host_Connect(object sender, MonitoringEventArgs args)
        {
            lock (locker)
            {
                Console.WriteLine(args.Message);
            }
        }
    }
}
