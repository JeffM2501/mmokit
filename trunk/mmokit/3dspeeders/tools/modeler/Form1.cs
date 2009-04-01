using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    public partial class ModelerDialog : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        Point mousePos;

        Model model = new Model();

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
            Invalidate(true);
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

            model.drawAll();

            GL.PushMatrix();

            GL.Translate(1, 0, 0);
            IntPtr quadric = Glu.NewQuadric();
            Glu.Sphere(quadric, 2, 24, 24);
            Glu.DeleteQuadric(quadric);

            GL.PopMatrix();

            glControl1.SwapBuffers();

        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Point delta = new Point(e.Location.X - mousePos.X,e.Location.Y - mousePos.Y);

            float rotFactor = 2.0f;
            float moveFactor = 10.0f;
            float zoomFactor = 2.0f;

            if (e.Delta != 0)
             camera.pushpull(e.Delta / zoomFactor);
            if (e.Button == MouseButtons.Right)
                camera.pan(delta.Y / rotFactor, -delta.X / rotFactor);
            if (e.Button == MouseButtons.Left)
                camera.move(-delta.X / moveFactor, delta.Y / moveFactor,0);

            mousePos = e.Location;
            Invalidate(true);
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = 240.0f;
            if (e.Delta != 0)
            {
                camera.pushpull(-e.Delta / zoomFactor);
                Invalidate(true);
            }
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void glControl1_MouseHover(object sender, EventArgs e)
        {
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.AddExtension = true;
            fd.CheckFileExists = true;
            fd.Filter = "Wavefront OBJ files (*.OBJ)|*.OBJ|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                OBJFile objReader = new OBJFile();
                objReader.read(new FileInfo(fd.FileName), model);

                Invalidate(true);
            }
        }
    }
}
