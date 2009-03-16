using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dSpeeders
{
    public class ConnectionInfo
    {
        public bool connect = false;
        public string server = string.Empty;
        public int port = 6088;
        public string username;
        public string password;
        public int resolutionX = 1024;
        public int resolutionY = 600;
        public bool fullscreen = false;
    }

    public class ClientConfig
    {
        public int resolutionX = 1024;
        public int resolutionY = 600;
        public bool fullscreen = false;
        public string username = string.Empty;
        public string password = string.Empty;
        public string lastServer = "localhost";
    }
}
