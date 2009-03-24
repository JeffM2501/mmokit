using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Player
    {
        public string name = string.Empty;
        public string token = string.Empty;
        public int guid = 0;

        public Dictionary<string, string> attributes = new Dictionary<string, string>();

        public Player( string n, string t, int id)
        {
            name = n;
            token = t;
            guid = id;
        }
    }

    public class PlayerEventArgs : EventArgs
    {
        Player      player;

        public PlayerEventArgs ( Player p ) : base()
        {
            player = p;
        }
    }

    public delegate void PlayerEventHandler(object sender, PlayerEventArgs e);

    public class Logic
    {
        // properties
        Dictionary<int, Player> players = new Dictionary<int, Player>();
        int lastGUID = 1;

        // methods
        public Player addPlayer ( string name, string token )
        {
            Player player = new Player(name, token, lastGUID++);
            PlayerEventArgs args = new PlayerEventArgs(player);

            OnAllowPlayer(args);
            if (player.guid > 0)
            {
                players.Add(player.guid, player);
                OnNewPlayer(args);
            }
            return player;
        }

        public Player getPlayer ( int id )
        {
            if (players.ContainsKey(id))
                return players[id];

            return null;
        }

        public Player getPlayer ( string name )
        {
            foreach(KeyValuePair<int,Player> p in players)
            {
                if (p.Value.name == name)
                    return p.Value;
            }
            return null;
        }

        public Player[] getPlayers ()
        {
            Player[] l = new Player[players.Count];

            int i = 0;
            foreach (KeyValuePair<int, Player> p in players)
                l[i++] = p.Value;

            return l;
        }

        // events
        public event PlayerEventHandler NewPlayer;
        public event PlayerEventHandler AllowPlayer;

        // derivable
        public virtual void OnNewPlayer ( PlayerEventArgs args )
        {
            if (NewPlayer != null)
                NewPlayer(this,args);
        }

        public virtual void OnAllowPlayer(PlayerEventArgs args)
        {
            if (AllowPlayer != null)
                AllowPlayer(this, args);
        }



    }
}
