using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Audio;
using OpenTK.Math;
using OpenTK.Input;
using OpenTK.Platform;

namespace _3dSpeeders
{
    public delegate void NewSceeneCallback();
    public delegate void NextFrameCallback(double frameTime);

    public class GameVisual : GameWindow
    {
        Game game;

        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 32.0f);

        RenderStateArgs renderState = new RenderStateArgs();

        Scene scene = null;

        public GameVisual(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, bool vsync, Game _game) : base(width,height,mode,title,options,device)
        {
            VSync = VSyncMode.Off;

            if (vsync)
                VSync = VSyncMode.On;

            renderState.fullscreen = options == GameWindowFlags.Fullscreen;
            renderState.x = width;
            renderState.y = height;

            game = _game;
            game.Keyboard = Keyboard;
        }

        public Scene setScene(Scene s)
        {
            if (scene != null)
                scene.unload(renderState);
            scene = s;

            if (scene != null)
             scene.load(renderState);

            return scene;
        }

        public void clearScene()
        {
            setScene(null);
        }
     
        public override void OnUnload(EventArgs e)
        {
            game.unload();
            base.OnUnload(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.Perspective(45.0, Width / (double)Height, 1.0, 64.0);

            renderState.x = Width;
            renderState.y = Height;
        }

        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(System.Drawing.Color.SteelBlue);
            GL.Enable(EnableCap.DepthTest);

            game.load();
        }

        public override void OnUpdateFrame(UpdateFrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (game.update())
                Exit();
        }

        public override void OnRenderFrame(RenderFrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit |
                     ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            if (scene != null)
                scene.draw(renderState);
            else
            {
                Glu.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

                GL.Begin(BeginMode.Triangles);

                GL.Color3(Color.LightYellow); GL.Vertex3(-1.0f, -1.0f, 4.0f);
                GL.Color3(Color.LightYellow); GL.Vertex3(1.0f, -1.0f, 4.0f);
                GL.Color3(Color.LightSkyBlue); GL.Vertex3(0.0f, 1.0f, 4.0f);

                GL.End();

                printer.Begin();
                printer.Print(((int)(1 / e.Time)).ToString("F0"), sans_serif, Color.SpringGreen);

                printer.Print("Empty Seene man", sans_serif, Color.Wheat, new RectangleF(200, 200, 250, 250), TextPrinterOptions.Default);

                printer.End();

            }
            SwapBuffers();
        }

    }
}
