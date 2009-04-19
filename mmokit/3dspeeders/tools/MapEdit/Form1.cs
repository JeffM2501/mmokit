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
using Drawables.Textures;
using Drawables.Models;
using Drawables.Models.XMDL;
using Drawables.DisplayLists;

using World;
using GraphicWorlds;

using Grids;

using Utilities.Paths;

namespace MapEdit
{
    public partial class Form1 : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        Point mousePos = new Point();
        bool noDrag = false;

        public Prefrences prefs = new Prefrences();
        string docName = string.Empty;

        Editor editor;

        public Form1()
        {
            editor = new Editor(this);
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

            DirectoryInfo configDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3dMapTool"));
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
            editor.world.AddDrawables();
            updateMaterialsList();
            updateMeshList();
            updateObjectList();

            if (prefs.dataDir == string.Empty)
                prefsToolStripMenuItem_Click(this, EventArgs.Empty);

            TextureSystem.system.rootDir = new DirectoryInfo(prefs.dataDir);

            setupGrid();
        }

        private void setupGrid ()
        {
            grid.gridSize = editor.world.world.size.X;
            if (editor.world.world.size.Y > grid.gridSize)
                grid.gridSize = editor.world.world.size.Y;

            grid.majorSpacing = editor.world.world.groundUVSize * 4;
            grid.minorSpacing = editor.world.world.groundUVSize;
            grid.alpha = 0.25f;
            grid.Invalidate();
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

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
       
            camera.Execute();

            GL.PushMatrix();

            DrawablesSystem.system.Execute();

            GL.PopMatrix();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            if (prefs.showGrid)
                grid.Execute();

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
                {
                    float realTilt = -delta.Y / rotFactor;
                    if (camera.Tilt + realTilt > 89.99f || camera.Tilt + realTilt < -89.99f)
                        realTilt = 0;

                    camera.turn(realTilt, -delta.X / rotFactor);
                }

                invalidateView();
            }
            mousePos = e.Location;
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            float moveFactor = 0.0125f;
            if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
                moveFactor *= 5f;
            if (e.Delta != 0)
            {
                camera.move(camera.Forward() * (e.Delta*moveFactor));
                Invalidate(true);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*.xmp";
            ofd.Filter = "XML Map File (*.xmp)|*.xmp|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (editor.OpenWorldFile(ofd.FileName))
                {
                    updateMaterialsList();
                    updateMeshList();
                    updateObjectList();
                    invalidateView();
                }
            }
        }    

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "*.xmp";
            sfd.Filter = "XML Map File (*.xmp)|*.xmp|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
                editor.SaveWorldFile(sfd.FileName);
        }

        private void prefsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog_Boxes.Prefs d = new Dialog_Boxes.Prefs(prefs);
            if (d.ShowDialog() == DialogResult.OK)
            {
                TextureSystem.system.rootDir = new DirectoryInfo(prefs.dataDir);
                savePrefs();
            }
        }

        public void updateMaterialsList()
        {
            int selection = MaterialsList.SelectedIndex;
            MaterialsList.Items.Clear();
            if (editor.world.materials == null)
                return;

            foreach (KeyValuePair<string, Material> m in editor.world.materials)
                MaterialsList.Items.Add(m.Key);

            MaterialsList.SelectedIndex = selection;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.AddMaterial();
            updateMaterialsList();
            invalidateView();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MaterialsList.SelectedIndex < 0)
                return;
            editor.EditMaterial(MaterialsList.SelectedItem.ToString());
            updateMaterialsList();
            invalidateView();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MaterialsList.SelectedIndex < 0)
                return;

            updateMaterialsList();
            invalidateView();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (editor.EditMapInfo())
           {
               setupGrid();
               invalidateView();
           }
        }

        private void updateMeshList()
        {
            int selection = MeshList.SelectedIndex;
            MeshList.Items.Clear();
            if (editor.world.models == null)
                return;

            foreach (KeyValuePair<string, Model> m in editor.world.models)
                MeshList.Items.Add(m.Key);

            MeshList.SelectedIndex = selection;
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "*.xmdl";
            ofd.Filter = "XML Model File (*.xmdl)|*.xmdl|All files (*.*)|*.*";
            ofd.InitialDirectory = prefs.dataDir;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                editor.AddModel(ofd.FileName);
                updateMaterialsList();
                updateMeshList();
                invalidateView();
            }
        }

        private void updateObjectList()
        {
            int selection = ObjectList.SelectedIndex;
            ObjectList.Items.Clear();

            foreach (WorldObject o in editor.world.world.objects)
            {
                ObjectList.Items.Add(o.objectName);
            }

            ObjectList.SelectedIndex = selection;
        }

        private void ObjectAdd_Click(object sender, EventArgs e)
        {
            int selection = MeshList.SelectedIndex;
            if (selection < 0)
                return;

            editor.AddObject(MeshList.SelectedItem.ToString());
            invalidateView();
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

        public string dataDir = string.Empty;
    }
}
