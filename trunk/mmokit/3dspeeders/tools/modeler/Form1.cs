﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    public partial class ModelerDialog : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();

        public ModelerDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            setupDisplay();
            camera.move(new Vector3(0, 0, 0));
            camera.pushpull(25);
            camera.pan(45, 15);
        }

        void setupDisplay()
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front,PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);

            Vector4 lightInfo = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            GL.Lightv(LightName.Light0, LightParameter.Ambient, lightInfo);
            GL.Lightv(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Lightv(LightName.Light0, LightParameter.Specular, lightInfo);

            glControl1_Resize(this, EventArgs.Empty);   // Ensure the Viewport is set up correctly
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            glControl1.MakeCurrent();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Glu.Perspective(60, glControl1.AspectRatio, 0.5f, 10000.0f);
            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            Invalidate();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            camera.Execute();
            GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(-10, -10, 10, 0.0f));
            grid.Execute();

            GL.Enable(EnableCap.Light0);

            GL.Enable(EnableCap.Lighting);
            GL.Color4(Color.Red);

            IntPtr quadric = Glu.NewQuadric();
            Glu.Sphere(quadric, 2, 24, 24);
            Glu.DeleteQuadric(quadric);

            glControl1.SwapBuffers();

        }
    }
}
