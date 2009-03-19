using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Configuration;
using Axiom.Math;
using Axiom.Input;
using Axiom.Overlays;

namespace _3dSpeeders
{
    public delegate void NewSceeneCallback();
    public delegate void NextFrameCallback(double frameTime);

    public class GameVisual
    {
        public List<NewSceeneCallback> newSceeneCallbacks = new List<NewSceeneCallback>();
        public List<NextFrameCallback> nextFrameCallbacks = new List<NextFrameCallback>();

        double lastCycleTime, lastFrameTime, lastFrameDrawStart;

        ConnectionInfo conInfo;
      
        SceneManager sceneManager;
      
        Root root;
        RenderWindow window;
        Camera camera;

        bool rebuildSceene = true;

        long lastOverlayUpdate = -1000;

        public GameVisual (ConnectionInfo info)
        {
            root = info.root;
            info.root = null;

            root.RenderSystem = info.renderSystem;
            info.renderSystem = null;

            conInfo = info;
        }

        public void shutdown()
        {
            if (root == null)
                return;

            Root.Instance.QueueEndRendering();

            if (sceneManager != null)
            {
                sceneManager.RemoveAllCameras();
                sceneManager.RemoveCamera(camera);
            }
            camera = null;
            if (window != null)
            {
                Root.Instance.RenderSystem.DetachRenderTarget(window);
                window.Dispose();
            }

            Root.Instance.Dispose();

            root = null;
            sceneManager = null;
            window = null;
        }

        public bool init (out InputReader inputReader)
        {
            window = root.Initialize(true, "3dSpeeders");
            inputReader = PlatformManager.Instance.CreateInputReader();
            inputReader.Initialize(window, true, true, false, true);

            sceneManager = root.SceneManagers.GetSceneManager(SceneType.Generic);

            camera = sceneManager.CreateCamera("PlayerCam");
            camera.Position = new Vector3(0, 0, 500);
            camera.LookAt(new Vector3(0, 0, -300));
            camera.Near = 5;

            Viewport viewport = window.AddViewport(camera);
            viewport.BackgroundColor = ColorEx.AliceBlue;

            camera.AspectRatio = viewport.ActualWidth / viewport.ActualHeight;

            TextureManager.Instance.DefaultNumMipMaps = 5;

            rebuildSceene = true;

            setupEvents();

            return true;
        }

        public void rebuild ()
        {
            rebuildSceene = true;
        }

        double getNow()
        {
            return Root.Instance.Timer.Milliseconds * 1000.0;
        }

        void callSceeenCallbacks ()
        {
            foreach (NewSceeneCallback c in newSceeneCallbacks)
                c();

            rebuildSceene = false;
        }

        void callNextFrameCallbacks(double frameTime)
        {
            foreach (NextFrameCallback c in nextFrameCallbacks)
                c(frameTime);
        }

        // called after the resources are loaded
        public void setupVisuals()
        {
            // Create debug overlay
            initOverlay();

            callSceeenCallbacks();

            lastCycleTime = getNow();
        }

        public void moveCamera (Vector2 rotateVector, Vector3 translateVector)
        {
            camera.Yaw(-rotateVector.x);
            camera.Pitch(-rotateVector.y);
            camera.MoveRelative(translateVector);
        }

        void setupEvents()
        {
            root.FrameStarted += updateOverlay;
            root.FrameStarted += frameStarted;
            root.FrameEnded += frameEnded;
        }

        void frameStarted(object source, FrameEventArgs e)
        {
            if (rebuildSceene)
                callSceeenCallbacks();

            double now = getNow();
            callNextFrameCallbacks(lastCycleTime - now);
            lastCycleTime = now;

            lastFrameDrawStart = getNow();
        }

        void frameEnded(object source, FrameEventArgs e)
        {
            // total time for just drawing
            lastFrameTime = getNow() - lastFrameDrawStart;
        }

        void initOverlay()
        {
            Overlay o = OverlayManager.Instance.GetByName("Core/DebugOverlay");
            if (o == null)
                throw new Exception("Could not find overlay named 'Core/DebugOverlay'.");
            o.Show();
        }

        void updateOverlay(object source, FrameEventArgs e)
        {
            if (Root.Instance.Timer.Milliseconds - lastOverlayUpdate >= 1000)
            {
                lastOverlayUpdate = Root.Instance.Timer.Milliseconds;

                OverlayElement element =
                   OverlayElementManager.Instance.GetElement("Core/DebugText");
                element.Text = window.DebugText;

                element = OverlayElementManager.Instance.GetElement("Core/CurrFps");
                element.Text = string.Format("Current FPS: {0}", Root.Instance.CurrentFPS);

                element = OverlayElementManager.Instance.GetElement("Core/BestFps");
                element.Text = string.Format("Best FPS: {0}", Root.Instance.BestFPS);

                element = OverlayElementManager.Instance.GetElement("Core/WorstFps");
                element.Text = string.Format("Worst FPS: {0}", Root.Instance.WorstFPS);

                element = OverlayElementManager.Instance.GetElement("Core/AverageFps");
                element.Text = string.Format("Average FPS: {0}", Root.Instance.AverageFPS);

                element = OverlayElementManager.Instance.GetElement("Core/NumTris");
                element.Text = string.Format("Triangle Count: {0}", sceneManager.TargetRenderSystem.FacesRendered);
            }
        }
    }
}
