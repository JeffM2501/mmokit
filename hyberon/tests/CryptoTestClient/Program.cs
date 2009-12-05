using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Clients;
using Lidgren.Network;

namespace CryptoTestClient
{
    class Program
    {
        static object locker = new object();
        static void Main(string[] args)
        {
            CryptoClient client = new CryptoClient("localhost",2501);

            while (true)
            {
                lock(locker)
                {
                    NetBuffer buffer = client.GetPentMessage();
                    while (buffer != null)
                    {
                        Console.WriteLine("Got message Code: " + buffer.ReadInt32().ToString());
                        Console.WriteLine("Got message size: " + buffer.LengthBytes.ToString());

                        Console.WriteLine(buffer.ReadString());
                        buffer = client.GetPentMessage();
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
