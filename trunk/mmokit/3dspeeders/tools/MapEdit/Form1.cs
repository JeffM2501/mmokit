using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Drawables;
using Drawables.Cameras;
using Drawables.Materials;
using World;
using GraphicWorlds;

using Grids;

namespace MapEdit
{
    public partial class Form1 : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        Point mousePos = new Point();
        bool noDrag = false;

        GraphicWorld world = new GraphicWorld();

        Prefrences prefs = new Prefrences();
        string docName = string.Empty;

        public Form1()
        {
            loadPrefs();

            InitializeComponent();
        }

        protected void loadPrefs()
        {
            DirectoryInfo configDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3dMapTool"));
            if (!configDir.Exists)
                configDir.Create();
            FileInfo prefsFile = new FileInfo(Path.Combine(configDir.FullName, "prefs.xml"));
            if (prefsFile.Exists)
                prefs = (Prefrences)new XmlSerializer(typeof(Prefrences)).Deserialize(prefsFile.OpenText());
        }

        protected void savePrefs()
        {
            prefs.windowSize = this.Size;
            prefs.windowPos = DesktopLocation;

            DirectoryInfo configDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3dModeler"));
            if (!configDir.Exists)
                configDir.Create();
            FileInfo prefsFile = new FileInfo(Path.Combine(configDir.FullName, "prefs.xml"));
            new XmlSerializer(typeof(Prefrences)).Serialize(prefsFile.CreateText(), prefs);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (prefs.windowSize.Width > 0 && prefs.windowSize.Height > 0)
            {
                SetDesktopLocation(prefs.windowPos.X, prefs.windowPos.Y);
                this.Height = prefs.windowSize.Height;
                this.Width = prefs.windowSize.Width;
            }

            setupDisplay();
            camera.move(new Vector3(-5, 0, 2));

            DrawablesSystem.system.removeAll();
            world.AddDrawables();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            savePrefs();
        }

        protected void invalidateView()
        {
            glControl1.Invalidate(true);
        }

        void setupDisplay()
        {
            GL.ClearColor(prefs.bgColor.r, prefs.bgColor.g, prefs.bgColor.b, 1);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);

            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Lightv(LightName.Light0, LightParameter.Ambient, lightInfo);
            lightInfo = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            GL.Lightv(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Lightv(LightName.Light0, LightParameter.Specular, lightInfo);

            glControl1_Resize(this, EventArgs.Empty);   // Ensure the Viewport is set up correctly
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            glControl1.MakeCurrent();
            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            camera.Resize(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            invalidateView();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Light0);
       
            camera.Execute();

            if (prefs.showGrid)
                grid.Execute();

            GL.Enable(EnableCap.Lighting);
            GL.PushMatrix();

            DrawablesSystem.system.Execute();

            GL.PopMatrix();
            glControl1.SwapBuffers();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Point delta = new Point(e.Location.X - mousePos.X, e.Location.Y - mousePos.Y);

            if (!noDrag)
            {
                float rotFactor = 2.0f;

                if (e.Button == MouseButtons.Right)
                    camera.turn(-delta.Y / rotFactor, -delta.X / rotFactor);

                invalidateView();
            }
            mousePos = e.Location;
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            float moveFactor = 0.0125f;
            if (e.Delta != 0)
            {
                camera.move(camera.Forward() * (e.Delta*moveFactor));
                Invalidate(true);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "*.xmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(sfd.FileName);
                GraphicWorldIO.write(world, new FileInfo(sfd.FileName));
            }
        }    
    }

    public class Prefrences
    {
        public Size windowSize = new Size();
        public Point windowPos = new Point();

        public GLColor bgColor = new GLColor(Color.DarkGray);
        public bool showGrid = true;
        public bool showNormals = false;
        public bool showWireframe = false;
    }
}
