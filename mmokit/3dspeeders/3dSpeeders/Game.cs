using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Input;

using NetworkMessages;
using GameLogic;

namespace _3dSpeeders
{
    public delegate void NetworkMessageCallback( NetBuffer buffer, MessageTypes type );

    public class Game 
    {
        public bool quit = false;
        public bool disconnected = false;

        public GameVisual visual;
        public ClientConfig config;
        public ConnectionInfo conInfo;

        public KeyboardDevice Keyboard;

        Logic logic = new Logic();

        NetClient client;

        Dictionary<MessageTypes, List<NetworkMessageCallback>> messageHanders = new Dictionary<MessageTypes, List<NetworkMessageCallback>>();

       LoadingScreen loadingScreen;

        public Game (ClientConfig _clientConfig, ConnectionInfo _conInfo )
        {
            config = _clientConfig;
            conInfo = _conInfo;

            NetConfiguration netConfig = new NetConfiguration("3dSpeeders");
            client = new NetClient(netConfig);
        }

        public bool run ()
        {
            using (visual = createVisual())
            {
                visual.Run();
            }

            return true;
        }

        GameVisual createVisual ()
        {
            int device = 0;
            if (config.device != string.Empty)
                device = int.Parse(config.device);

            if (device < 0 && device >= DisplayDevice.AvailableDisplays.Length)
                device = 0;

            DisplayDevice dev = DisplayDevice.AvailableDisplays[device];

            if (config.fullscreen)
            {
                DisplayResolution resolution = null;
                foreach (DisplayResolution dr in dev.AvailableResolutions)
                {
                    if ( dr.Width == config.resolutionX && dr.Height == config.resolutionY )
                    {
                        if (config.refresh < 1)
                        {
                            if (resolution == null)
                                resolution = dr;
                            else if (resolution.RefreshRate < dr.RefreshRate)
                                resolution = dr;
                        }
                        else if (config.refresh == dr.RefreshRate)
                            resolution = dr;
                    }
                }

                if (resolution == null)
                    resolution = dev.AvailableResolutions[dev.AvailableResolutions.Length - 1];

                dev.SelectResolution(resolution.Width, resolution.Height, 32, resolution.RefreshRate);
            }

            GraphicsMode gMode = new GraphicsMode(new ColorFormat(8),32,0,0,new ColorFormat(8),2,false);
            GameWindowFlags flags = 0;
            if (config.fullscreen)
                flags = GameWindowFlags.Fullscreen;

            return new GameVisual(config.resolutionX, config.resolutionY, gMode, "3dSpeeders", flags, dev, config.vsync, this);
        }

        public bool init ()
        {
            connectToServer();

            return true;
        }

        public void load ()
        {
            if (!init())
                visual.Exit();

            loadingScreen = new LoadingScreen(client);
            visual.setScene(loadingScreen);
        }

        public void unload ()
        {
            shutdown();
        }

        public bool update ()
        {
            if (Keyboard[Key.Escape])
                return true;

            checkNetwork();

            return false;
        }

        public void shutdown ()
        {
            if (client != null)
            {
                client.Disconnect("Client Exit");
                client.Dispose();
                client = null;
            }
        }

        void connectToServer ()
        {
            client.Connect(conInfo.server, conInfo.port);
        }

        void exit()
        {
            visual.Exit();
        }

        void checkNetwork ()
        {
            NetBuffer buffer = client.CreateBuffer();
            NetMessageType type;

            while (client.ReadMessage(buffer, out type))
            {
                switch(type)
                {
                    case NetMessageType.StatusChanged:
                         if (client.Status == NetConnectionStatus.Connected)
                         {
                             visual.clearScene();
                             loadingScreen = null;
                         }
                         else if (loadingScreen != null)
                             loadingScreen.reload();

                         if (client.Status == NetConnectionStatus.Disconnected)
                         {
                             disconnected = true;
                             exit();
                         }

                        break;

                    case NetMessageType.Data:
                        callHandlers(buffer);
                        break;
                }
            }
        }

        void callHandlers(NetBuffer buffer)
        {
            MessageTypes type = MessageBuffer.GetType(buffer);
            if (messageHanders.ContainsKey(type))
            {
                foreach (NetworkMessageCallback n in messageHanders[type])
                    n(buffer, type);
            }
        }
    }
}
