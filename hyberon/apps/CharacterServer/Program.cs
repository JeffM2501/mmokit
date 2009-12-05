using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Hosts;
using CharacterDatabse;
using Lidgren.Network;

namespace CharacterServer
{
    class Program
    {
        public class ConnectedUser
        {
            UInt64 UID = 0;
            Int64 Token = -5;
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

        }
    }
}
