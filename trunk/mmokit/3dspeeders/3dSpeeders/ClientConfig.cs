using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _3dSpeeders
{
    public class ConnectionInfo
    {
        public bool connect = false;
        public string server = string.Empty;
        public int port = 6088;
        public string username;
        public string password;

        public DirectoryInfo dataDir;
    }

    public class ClientConfig
    {
        public string device = string.Empty;
        public int resolutionX = 1024;
        public int resolutionY = 600;
        public float refresh = 0;
        public bool fullscreen = false;
        public int FSAA = -1;
        public bool vsync = false;

        public string username = string.Empty;
        public string password = string.Empty;
        public string lastServer = "localhost";

        public bool enableSound = true;
        public int volume = 5;

        public bool savePassword = false;
        public bool saveUsername = true;

        public string avatar = string.Empty;
    }
}
