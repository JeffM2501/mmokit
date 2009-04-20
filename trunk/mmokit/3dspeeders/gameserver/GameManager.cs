using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using NetworkMessages;
using GameStates;

namespace gameserver
{
    public enum PlayerConState
    {
        None,
        Connected,
        Credentialed,
        Authenticated,
        Joined
    }

    public class Player
    {
        static int lastGUID = 0;
        static int newGUID()
        {
            lastGUID++;
            return lastGUID;
        }

        public PlayerConState conState = PlayerConState.None;
        public string username = string.Empty;
        public string callsign = string.Empty;
        public NetConnection connection;
        public int id = 0;

        public Player ( NetConnection con )
        {
            connection = con;
            id = Player.newGUID();
        }
    }

    class GameManager
    {
        List<Player> players = new List<Player>();
        NetServer server;

        GameState state;
        GameWorld world;

        public GameManager(NetServer s)
        {
            server = s;
            world = new GameWorld();
            state = new GameState(world.world);
        }

        void sendPlayerPart ( Player player )
        {
            server.SendToAll(new RemovePlayerMessage().pack(server.CreateBuffer(),player.id,player.callsign), Channels.PlayerControll, player.connection);
        }

        public void PlayerQuit (NetConnection sender)
        {
            if (sender.Tag == null)
                return;
            Player player = (Player)sender.Tag;
            players.Remove(player);

            // if he' hasn't fully joined, no one has been told yet so keep it to yourself.
            if (player.conState == PlayerConState.Joined)
                sendPlayerPart(player);
        }

        public void PlayerJoin(NetConnection sender)
        {
            Player player = new Player(sender);
            sender.Tag = (object)player;
            players.Add(player);
        }

        public void PlayerMessage(NetConnection sender, MessageTypes type, NetBuffer buffer )
        {

        }
    }
}
