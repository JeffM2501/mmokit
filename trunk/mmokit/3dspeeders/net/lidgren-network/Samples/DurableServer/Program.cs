using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using System.Threading;
using System.IO;

namespace DurableServer
{
	class Program
	{
		static void Main(string[] args)
		{
			NetConfiguration config = new NetConfiguration("durable");
			config.MaxConnections = 128;
			config.Port = 14242;
			NetServer server = new NetServer(config);

			server.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
			server.SetMessageTypeEnabled(NetMessageType.DebugMessage, true);
			//server.SetMessageTypeEnabled(NetMessageType.VerboseDebugMessage, true);
			server.SetMessageTypeEnabled(NetMessageType.StatusChanged, true);

			server.SimulatedMinimumLatency = 0.05f;
			server.SimulatedLatencyVariance = 0.025f;
			server.SimulatedLoss = 0.03f;

			server.Start();

			FileStream fs = new FileStream("./serverlog.txt", FileMode.Create, FileAccess.Write, FileShare.Read);
			StreamWriter wrt = new StreamWriter(fs);
			Output(wrt, "Log started at " + DateTime.Now);
			wrt.Flush();

			NetBuffer buffer = server.CreateBuffer();

			int expected = 1;

			Console.WriteLine("Press any key to quit");
			while (!Console.KeyAvailable)
			{
				NetMessageType type;
				NetConnection sender;
				if (server.ReadMessage(buffer, out type, out sender))
				{
					switch (type)
					{
						case NetMessageType.StatusChanged:
							if (sender.RemoteHailData != null)
								Output(wrt, "New status: " + sender.Status + " (" + buffer.ReadString() + ") Remote hail is: " + Encoding.ASCII.GetString(sender.RemoteHailData));
							else
								Output(wrt, "New status: " + sender.Status + " (" + buffer.ReadString() + ") Remote hail hasn't arrived.");
							break;
						case NetMessageType.BadMessageReceived:
						case NetMessageType.ConnectionRejected:
						case NetMessageType.DebugMessage:
							//
							// All these types of messages all contain a single string in the buffer; display it
							//
							Output(wrt, buffer.ReadString());
							break;
						case NetMessageType.VerboseDebugMessage:
							wrt.WriteLine(buffer.ReadString()); // don't output to console
							break;
						case NetMessageType.ConnectionApproval:
							if (sender.RemoteHailData != null &&
								Encoding.ASCII.GetString(sender.RemoteHailData) == "Hail from client")
							{
								Output(wrt, "Hail ok!");
								sender.Approve(Encoding.ASCII.GetBytes("Hail from server"));
							}
							else
							{
								sender.Disapprove("Wrong hail!");
							}
							break;
						case NetMessageType.Data:
							string str = buffer.ReadString();

							// parse it
							int nr = Int32.Parse(str.Substring(9));

							if (nr != expected)
							{
								Output(wrt, "Warning! Expected " + expected + "; received " + nr + " str is ---" + str + "---");
							}
							else
							{
								expected++;
								Console.Title = "Server; received " + nr + " messages";
							}

							break;
						default:
							Output(wrt, "Unhandled: " + type + " " + buffer.ToString());
							break;
					}
				}
				Thread.Sleep(1);
			}

			// clean shutdown
			wrt.Close();
			server.Shutdown("Application exiting");
		}

		private static void Output(StreamWriter wrt, string str)
		{
			Console.WriteLine(str);
			wrt.WriteLine(str);
		}
	}
}
