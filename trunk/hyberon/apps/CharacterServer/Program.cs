using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Hosts;
using Characters;
using CharacterDatabse;
using UserDatabase;
using Lidgren.Network;

namespace CharacterServer
{
    class Program
    {
        public class ConnectedUser
        {
            public UInt64 UID = 0;
            public Int64 Token = -5;
        }

        static int ConnectDeny = 0;
        static int ConnectMessage = 2;
        static int ConnectAccept = 3;
        static int ListCharacters = 4;
        static int ActivateCharacter = 5;
        static int DeleteCharacter = 6;
        static int AddCharacter = 7;
        static int CharacterOK = 8;
        static int CharacterFail = 9;
        static int ZoneServerRedirect = 10;
        static int CharacterInfo = 11;

        static void Main(string[] args)
        {
            string configPath = "./config.xml";
            if (args.Length > 0)
                configPath = args[0];

            Dictionary<NetConnection, ConnectedUser> ConnectedUsers = new Dictionary<NetConnection, ConnectedUser>();

            CharacterDB characterDB = new CharacterDB(configPath);
            if (!characterDB.Valid())
            {
                Console.WriteLine("Character Database connection failed");
                return;
            }

            Users userDB = new Users(configPath);
            if (!userDB.Valid())
            {
                Console.WriteLine("User Database connection failed");
                return;
            }

            Console.WriteLine("User database startup " + characterDB.ChracterCount().ToString() + " characters found");

            CryptoHost host = new CryptoHost(2502);

            bool done = false;
            while (!done)
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
                    if (ConnectedUsers.ContainsKey(msg.Sender))
                    {
                        ConnectedUser user = ConnectedUsers[msg.Sender];

                        if (msg.Name == ConnectMessage)
                        {
                            user.UID = msg.Data.ReadUInt64();
                            user.Token = msg.Data.ReadInt64();
                            if (!userDB.CheckToken(user.UID, user.Token))
                            {
                                user.Token = -10;
                                NetBuffer buffer = new NetBuffer();
                                buffer.Write(ConnectDeny);
                                host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                host.DisconnectUser(msg.Sender);
                            }
                            else
                            {
                                NetBuffer buffer = new NetBuffer();
                                buffer.Write(ConnectAccept);
                                host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                            }
                        }
                        else if (user.Token > 0)
                        {
                            if (msg.Name == ListCharacters)
                            {
                                List<UInt64> characters = characterDB.GetCharacterList(user.UID);
                                NetBuffer buffer = new NetBuffer();
                                buffer.Write(ListCharacters);
                                buffer.Write(characters.Count);
                                foreach (UInt64 CID in characters)
                                    buffer.Write(CID);

                                host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                host.DisconnectUser(msg.Sender);
                            }
                            else if (msg.Name == CharacterInfo)
                            {
                                UInt64 CID = msg.Data.ReadUInt64();
                                Character character = characterDB.GetCharacter(CID,user.UID);
                                if (character.CharacterID != CID)
                                {
                                    NetBuffer buffer = new NetBuffer();
                                    buffer.Write(CharacterFail);
                                    host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                }
                                else
                                {
                                    byte[] d = Character.ToArray(character);
                                    NetBuffer buffer = new NetBuffer();
                                    buffer.Write(CharacterInfo);
                                    buffer.Write(d.Length);
                                    buffer.Write(d);
                                    host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                }
                            }
                            else if (msg.Name == DeleteCharacter)
                            {
                                UInt64 CID = msg.Data.ReadUInt64();
                                if(characterDB.DeleteCharacter(CID,user.UID))
                                {
                                    NetBuffer buffer = new NetBuffer();
                                    buffer.Write(DeleteCharacter);
                                    buffer.Write(CID);
                                    host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                }
                                else
                                {
                                    NetBuffer buffer = new NetBuffer();
                                    buffer.Write(CharacterFail);
                                    host.SendMessage(msg.Sender, buffer, NetChannel.ReliableInOrder3);
                                }
                            }
                            else if (msg.Name == AddCharacter)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}
