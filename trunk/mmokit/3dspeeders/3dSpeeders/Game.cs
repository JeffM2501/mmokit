using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using Axiom;
using Axiom.Core;
using Axiom.Input;
using Axiom.Math;
using System.IO;

using NetworkMessages;

namespace _3dSpeeders
{
    public delegate void NetworkMessageCallback( NetBuffer buffer, MessageTypes type );

    public class Game 
    {
        public GameVisual visual;
        public ClientConfig config;
        public ConnectionInfo conInfo;

        float moveScale = 0, rotScale = 0, moveSpeed = 100, rotateSpeed = 36;
        Vector2 rotateVector = new Vector2(0, 0);
        Vector3 translateVector = new Vector3(0, 0, 0);

        NetClient client;

        InputReader inputReader;

        Dictionary<MessageTypes, List<NetworkMessageCallback>> messageHanders = new Dictionary<MessageTypes, List<NetworkMessageCallback>>();

        LoadingScreen loadingScreen;

        public Game (ClientConfig _clientConfig, ConnectionInfo _conInfo )
        {
            visual = new GameVisual(_conInfo);

            config = _clientConfig;
            conInfo = _conInfo;

            NetConfiguration netConfig = new NetConfiguration("3dSpeeders");
            client = new NetClient(netConfig);
        }

        public bool run ()
        {
            try
            {
                if (!init())
                {
                    shutdown();
                    return false;
                }

                Root.Instance.StartRendering();
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                // This catches directx missing (or too old) to log :)
                for (int i = 0; i < ex.LoaderExceptions.Length; i++)
                    if (LogManager.Instance != null)
                        LogManager.Instance.Write(ex.LoaderExceptions[i].Message);
            }
            catch (Exception ex)
            {
                if (LogManager.Instance != null)
                    LogManager.Instance.Write(ex.ToString());
            }

            shutdown();

            return true;
        }

        public bool init ()
        {
            if (!new ResourceLoader().setup(conInfo.dataDir))
                return false;

            if (!visual.init(out inputReader) || inputReader == null)
                return false;

            Root.Instance.FrameStarted += updateInput;

            loadingScreen = new LoadingScreen(client);

            visual.newSceeneCallbacks.Add(new NewSceeneCallback(loadingScreen.reload));
            visual.nextFrameCallbacks.Add(new NextFrameCallback(nextFrame));

            connectToServer();

            visual.setupVisuals();

            return true;
        }

        public void shutdown ()
        {
            visual.shutdown();

            client.Disconnect("Client Exit");
            client.Dispose();
            client = null;
        }

        void connectToServer ()
        {
            client.Connect(conInfo.server, conInfo.port);
        }

        void LoadingSceene ()
        {
        }

        void nextFrame ( double frameTime )
        {
            checkNetwork();

            if (loadingScreen != null)
                processFrame(frameTime);
        }

        void processFrame(double frameTime)
        {

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
                            if (loadingScreen != null)
                            {
                                visual.newSceeneCallbacks.Remove(loadingScreen.reload);
                                loadingScreen.unload();
                                loadingScreen = null;

                                visual.newSceeneCallbacks.Add(new NewSceeneCallback(buildGameSceene));
                                visual.rebuild();
                            }
                        }
                        else
                            loadingScreen.reload();
                        break;

                    case NetMessageType.Data:
                        callHandlers(buffer);
                        break;
                }
            }
        }

        void buildGameSceene ()
        {

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

        void updateInput(object source, FrameEventArgs e)
        {
            inputReader.Capture();

            // Reset vectors
            rotateVector.x = translateVector.x = 0;
            rotateVector.y = translateVector.y = 0;
            translateVector.z = 0;

            // Move
            moveScale = moveSpeed * e.TimeSinceLastFrame;

            // Rotate
            rotScale = rotateSpeed * e.TimeSinceLastFrame;

            // Move forward and back
            if (inputReader.IsKeyPressed(KeyCodes.W) || inputReader.IsKeyPressed(KeyCodes.Up))
                translateVector.z = -moveScale;
            else if (inputReader.IsKeyPressed(KeyCodes.S) || inputReader.IsKeyPressed(KeyCodes.Down))
                translateVector.z = moveScale;

            // Move left and right
            if (inputReader.IsKeyPressed(KeyCodes.A))
                translateVector.x = -moveScale;
            else if (inputReader.IsKeyPressed(KeyCodes.D))
                translateVector.x = moveScale;

            // Move up and down
            if (inputReader.IsKeyPressed(KeyCodes.PageUp))
                translateVector.y = moveScale;
            else if (inputReader.IsKeyPressed(KeyCodes.PageDown))
                translateVector.y = -moveScale;

            // Rotate left and right
            if (inputReader.IsKeyPressed(KeyCodes.Left))
                rotateVector.x = -rotScale;
            else if (inputReader.IsKeyPressed(KeyCodes.Right))
                rotateVector.x = rotScale;

            // Right mouse button pressed
            if (inputReader.IsMousePressed(MouseButtons.Right))
            {
                // Translate
                translateVector.x += inputReader.RelativeMouseX * 0.13f;
                translateVector.y -= inputReader.RelativeMouseY * 0.13f;
            }
            else
            {
                // Apply mouse rotation
                rotateVector.x += inputReader.RelativeMouseX * 0.13f;
                rotateVector.y += inputReader.RelativeMouseY * 0.13f;
            }

            visual.moveCamera(rotateVector, translateVector);

            // TODO: what about window-closing-event?
            if (inputReader.IsKeyPressed(KeyCodes.Escape))
            {
                visual.shutdown();
                return;
            }
        }
    }
}
