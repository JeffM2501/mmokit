using System;
using Lidgren.Network;

// the real one
namespace NetworkMessages
{
    public enum MessageTypes
    {
        None = 0,
        Chat = 1,
        PlayerList = 2,
        RemoveUser = 3,
        AddUser = 4,
    }

    static public class Channels
    {
        public const NetChannel PlayerControll = NetChannel.ReliableInOrder1;
        public const NetChannel WorldControll = NetChannel.ReliableInOrder2;
        public const NetChannel ChatMessages = NetChannel.ReliableInOrder3;
        public const NetChannel ServerMessage = NetChannel.ReliableInOrder4;

        public const NetChannel MovementData = NetChannel.UnreliableInOrder1;
    }

    static public class MessageBuffer
    {
        static public MessageTypes GetType ( NetBuffer buffer )
        {
            return (MessageTypes)buffer.ReadInt16();
        }

        static public void SetType(NetBuffer buffer, MessageTypes type)
        {
            buffer.Write((Int16)type);
        }
    }
}