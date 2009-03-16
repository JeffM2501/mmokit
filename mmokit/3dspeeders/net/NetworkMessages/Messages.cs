using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace NetworkMessages
{
    public class RemovePlayerMessage
    {
        public List<KeyValuePair<int, string>> players;

        public NetBuffer pack ( NetBuffer buffer, int id, string name )
        {
            MessageBuffer.SetType(buffer, MessageTypes.RemoveUser);

            buffer.Write((UInt16)1);
            buffer.Write(id);
            buffer.Write(name);

            return buffer;
        }

        public NetBuffer pack (NetBuffer buffer )
        {
            if (players == null || players.Count == 0)
                return null;

            MessageBuffer.SetType(buffer, MessageTypes.RemoveUser);
            buffer.Write((UInt16)players.Count);
            foreach(KeyValuePair<int, string> k in players)
            {
                buffer.Write(k.Key);
                buffer.Write(k.Value);
            }

            return buffer;
        }

        public void add ( int id, string name )
        {
            if (players == null)
                players = new List<KeyValuePair<int, string>>();

            players.Add(new KeyValuePair<int,string>(id,name));
        }

        public bool unpack (NetBuffer buffer)
        {
            if (buffer.LengthBytes < 2 + 4)
                return false;

            Int16 count = buffer.ReadInt16();
            players = new List<KeyValuePair<int, string>>();

            try
            {
                for (int i = 0; i < count; i++)
                    players.Add(new KeyValuePair<int,string>(buffer.ReadInt32(),buffer.ReadString()));

                return true;
            }
            finally{}
            return false;
        }

    }
}
