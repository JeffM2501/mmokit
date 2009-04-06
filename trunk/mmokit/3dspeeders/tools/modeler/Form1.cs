using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    public partial class ModelerDialog : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        Point mousePos = new Point();
        bool noDrag = false;

        Model model = new Model();

        string docName = string.Empty;

        public ModelerDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            setupDisplay();
            camera.move(new Vector3(0, 0, 0));
            camera.pushpull(5);
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

            GL.PushMatrix();
          //  GL.Rotate(90, 1, 0, 0);
            model.drawAll();
            GL.PopMatrix();

            if (false)
            {
                GL.PushMatrix();

                GL.Translate(0, 0, 0);
                IntPtr quadric = Glu.NewQuadric();
                Glu.Sphere(quadric, 2, 24, 24);
                Glu.DeleteQuadric(quadric);

                GL.PopMatrix();
            }

            glControl1.SwapBuffers();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Point delta = new Point(e.Location.X - mousePos.X,e.Location.Y - mousePos.Y);

            if (!noDrag)
            {
                float rotFactor = 2.0f;
                float moveFactor = 10.0f;
                float zoomFactor = 2.0f;

                if (e.Delta != 0)
                 camera.pushpull(e.Delta / zoomFactor);
                if (e.Button == MouseButtons.Right)
                    camera.pan(delta.Y / rotFactor, -delta.X / rotFactor);
                if (e.Button == MouseButtons.Left)
                    camera.move(-delta.X / moveFactor, delta.Y / moveFactor,0);

                Invalidate(true);
            }
            mousePos = e.Location;
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (noDrag)
                return;

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

        void setDocName ( string text )
        {
            docName = text;
            this.Text = "Modeler:" + text;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            noDrag = true;
            fd.AddExtension = true;
            fd.CheckFileExists = true;
            fd.Filter = "Wavefront OBJ files (*.OBJ)|*.OBJ|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                OBJFile objReader = new OBJFile();
                objReader.read(new FileInfo(fd.FileName), model);

                if (model.meshes.Count > 0)
                    setDocName(Path.GetFileNameWithoutExtension(fd.FileName));

                Invalidate(true);
            }
            noDrag = false;
        }

        private void swapYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model.swapYZ();
            Invalidate(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (model.meshes.Count < 1)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.FileName = docName + ".xmdl";
            sfd.Filter = "XML Model files (*.xmdl)|*.xmdl";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer xml = new XmlSerializer(typeof(Model));
                StreamWriter sr = new StreamWriter(sfd.OpenFile());
                xml.Serialize(sr, model);
                sr.Close();
            }            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*.xmdl";
            ofd.Filter = "XML Model files (*.xmdl)|*.xmdl";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer xml = new XmlSerializer(typeof(Model));
                StreamReader sr = new StreamReader(ofd.OpenFile());
                model.Invalidate();
                model = (Model)xml.Deserialize(sr);
                sr.Close();

                if (model.meshes.Count > 0)
                    setDocName(Path.GetFileNameWithoutExtension(ofd.FileName));

                Invalidate(true);
            }
        }
    }
}
