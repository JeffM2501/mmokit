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

using Drawables.Models;
using Drawables.Models.OBJ;
using Drawables.Materials;

namespace modeler
{
    public partial class ModelerDialog : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        Point mousePos = new Point();
        bool noDrag = false;

        Model model = new Model();

        Prefrences prefs = new Prefrences();

        string docName = string.Empty;

        bool useHeadlight = false;

        public ModelerDialog()
        {
            loadPrefs();

            InitializeComponent();

            gridToolStripMenuItem.Checked = prefs.showGrid;
            normalsToolStripMenuItem.Checked = prefs.showNormals;
            wireframeToolStripMenuItem.Checked = prefs.showWireframe;
            headlightToolStripMenuItem.Checked = useHeadlight;
        }

        protected void loadPrefs()
        {
            DirectoryInfo configDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3dModeler"));
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
            camera.move(new Vector3(0, 0, 0));
            camera.pushpull(5);
            camera.pan(45, 15);
        }

        private void ModelerDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            savePrefs();
        }

        protected void invalidateView()
        {
            glControl1.Invalidate(true);
        }

        void clearModel()
        {
            model.clear();
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

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Glu.Perspective(60, glControl1.AspectRatio, 0.5f, 10000.0f);
            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
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
            if (useHeadlight)
            {
                GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(0, 0, 1, 1));
                GL.Lightv(LightName.Light0, LightParameter.Diffuse, new Vector4(0.6f, 0.6f, 0.6f, 1));
            }

            camera.Execute();

            if (prefs.showGrid)
                grid.Execute();

            if (!useHeadlight)
                GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(-10, -15, 20, 0.0f));
            GL.Enable(EnableCap.Lighting);
            //    GL.Color4(Color.Red);

            GL.PushMatrix();
            //  GL.Rotate(90, 1, 0, 0);
            MaterialOverride ovd = getMatOverride();
            if (ovd == null)
                model.drawAll(prefs.showNormals, prefs.showWireframe);
            else
                model.drawAll(prefs.showNormals, prefs.showWireframe, ovd);

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
                //                 float moveFactor = 10.0f;
                float zoomFactor = 2.0f;

                if (e.Delta != 0)
                    camera.pushpull(e.Delta / zoomFactor);
                if (e.Button == MouseButtons.Right)
                    camera.pan(delta.Y / rotFactor, -delta.X / rotFactor);
                //  if (e.Button == MouseButtons.Left)
                //       camera.move(-delta.X / moveFactor, delta.Y / moveFactor,0);

                invalidateView();
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

        void setDocName(string text)
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
                clearModel();

                OBJFile objReader = new OBJFile();
                objReader.read(new FileInfo(fd.FileName), model);

                if (model.meshes.Count > 0)
                    setDocName(Path.GetFileNameWithoutExtension(fd.FileName));

                setupSkins();
                invalidateView();
            }
            noDrag = false;
        }

        private void swapYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            model.swapYZ();
            invalidateView();
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

                // free the old lists and materials
                clearModel();
                model = (Model)xml.Deserialize(sr);
                sr.Close();

                // setup the new model to draw
                model.Invalidate();

                if (model.meshes.Count > 0)
                    setDocName(Path.GetFileNameWithoutExtension(ofd.FileName));

                setupSkins();

                invalidateView();
            }
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScaleDialog scale = new ScaleDialog();
            if (scale.ShowDialog() == DialogResult.OK && scale.ScaleItem.Text != string.Empty)
            {
                model.scale(float.Parse(scale.ScaleItem.Text));
                invalidateView();
            }
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prefs.showGrid = !prefs.showGrid;
            gridToolStripMenuItem.Checked = prefs.showGrid;
            invalidateView();
        }

        private void normalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prefs.showNormals = !prefs.showNormals;
            normalsToolStripMenuItem.Checked = prefs.showNormals;
            invalidateView();
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prefs.showWireframe = !prefs.showWireframe;
            wireframeToolStripMenuItem.Checked = prefs.showWireframe;
            invalidateView();
        }

        private void headlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useHeadlight = !useHeadlight;
            headlightToolStripMenuItem.Checked = useHeadlight;
            invalidateView();
        }

        void setupSkinNode(MaterialOverride ovd)
        {
            TreeNode skinNode = SkinView.Nodes.Add(ovd.name);
            skinNode.Tag = ovd;

            foreach (Mesh m in model.meshes)
            {
                MeshOverride meshOvd = ovd.getOverride(m.material);

                string name = m.material.name;
                if (meshOvd.newMaterial != null && meshOvd.newMaterial.textureName != string.Empty)
                    name += " (" + Path.GetFileName(meshOvd.newMaterial.textureName) + ")";
                TreeNode matNode = skinNode.Nodes.Add(name);

                matNode.Tag = meshOvd;
                foreach (MeshGroup g in m.groups)
                {
                    TreeNode groupNode = matNode.Nodes.Add(g.name);
                    groupNode.Tag = g;
                    if (meshOvd.hiddenGroups.Contains(g.name))
                        groupNode.ForeColor = Color.Gray;
                }
            }
        }

        void setupSkins()
        {
            SkinView.Nodes.Clear();

            if (!model.valid())
                return;

            // add the default node
            TreeNode defaultNode = SkinView.Nodes.Add(docName);
            defaultNode.Tag = null;
            foreach (Mesh m in model.meshes)
            {
                string name = m.material.name;
                if (m.material.textureName != string.Empty)
                    name += " (" + Path.GetFileName(m.material.textureName) + ")";
                TreeNode matNode = defaultNode.Nodes.Add(name);
                matNode.Tag = null;

                foreach (MeshGroup g in m.groups)
                    matNode.Nodes.Add(g.name).Tag = null;
            }

            foreach (MaterialOverride s in model.skins)
            {
                setupSkinNode(s);
            }
        }

        string currentOverideName()
        {
            if (SkinView.SelectedNode.Tag == null)
                return string.Empty;

            Type tagType = SkinView.SelectedNode.Tag.GetType();

            if (tagType == typeof(MaterialOverride))
                return ((MaterialOverride)SkinView.SelectedNode.Tag).name;

            if (tagType == typeof(MeshOverride))
            {
                TreeNode root = SkinView.SelectedNode.Parent;
                return ((MaterialOverride)root.Tag).name;
            }

            if (tagType == typeof(MeshGroup))
            {
                TreeNode root = SkinView.SelectedNode.Parent.Parent;
                return ((MaterialOverride)root.Tag).name;
            }

            return string.Empty;
        }

        MaterialOverride currentOveride()
        {
            return model.findSkin(currentOverideName());
        }

        MaterialOverride getMatOverride()
        {
            if (SkinView.SelectedNode == null)
                return null;

            if (SkinView.SelectedNode.Tag == null)
                return null;

            return currentOveride();
        }

        private void NewSkin_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = model.newSkin("New Skin" + model.skins.Count.ToString());
            foreach (Mesh m in model.meshes)
                ovd.addMaterial(m.material.name, m.material);

            setupSkinNode(ovd);
        }

        private void RemoveSkin_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = currentOveride();
            if (ovd == null)
                return;

            foreach (TreeNode t in SkinView.Nodes)
            {
                if (t.Tag != null && t.Tag == ovd)
                {
                    SkinView.Nodes.Remove(t);
                    break;
                }
            }

            model.removeSkin(ovd.name);
        }

        private void SkinView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void SkinView_DoubleClick(object sender, EventArgs e)
        {
            if (SkinView.SelectedNode.Tag == null)
            {
                invalidateView();
                return;
            }

            Type tagType = SkinView.SelectedNode.Tag.GetType();
            if (tagType == typeof(MaterialOverride))
            {
                MaterialOverride ovd = (MaterialOverride)SkinView.SelectedNode.Tag;

                SingleItemQuery siq = new SingleItemQuery();
                siq.Title = "Group Name";
                siq.MessageLabel = "New Group Name";
                siq.Value = SkinView.SelectedNode.Text;

                if (siq.ShowDialog() == DialogResult.OK)
                {
                    if (model.findSkin(siq.Value) != null)
                    {
                        MessageBox.Show("Name Exists");
                        return;
                    }
                    ovd.name = siq.Value;
                    ovd.name = siq.Value;
                    SkinView.SelectedNode.Text = siq.Value;
                }
            }
            else if (tagType == typeof(MeshOverride))
            {
                MeshOverride movd = (MeshOverride)SkinView.SelectedNode.Tag;

                SingleItemQuery siq = new SingleItemQuery();
                siq.Title = "Texture File";
                siq.MessageLabel = "Texture Path";
                string matName = string.Empty;
                if (movd == null)
                    return;

                if (movd.newMaterial != null)
                    siq.Value = movd.newMaterial.textureName;

                if (siq.ShowDialog() == DialogResult.OK)
                {
                    if (movd.newMaterial == null)
                        movd.newMaterial = new Material();

                    movd.newMaterial.Invalidate();
                    movd.newMaterial.textureName = siq.Value;

                    SkinView.SelectedNode.Text = matName + " (" + Path.GetFileName(siq.Value) + ")";

                    invalidateView();
                }
            }
            else if (tagType == typeof(MeshGroup))
            {
                MeshGroup mgroup = (MeshGroup)SkinView.SelectedNode.Tag;

                if (mgroup == null)
                    return;

                MeshOverride movd = (MeshOverride)SkinView.SelectedNode.Parent.Tag;
                MaterialOverride ovd = (MaterialOverride)SkinView.SelectedNode.Parent.Parent.Tag;

                movd.Invalidate();

                if (movd.hiddenGroups.Contains(mgroup.name))
                {
                    ovd.showGroup(movd.origonalMatName, mgroup.name);
                    SkinView.SelectedNode.ForeColor = Color.Black;
                }
                else
                {
                    ovd.hideGroup(movd.origonalMatName, mgroup.name);
                    SkinView.SelectedNode.ForeColor = Color.Gray;
                }

                invalidateView();
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = prefs.bgColor.ToColor();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                prefs.bgColor = new GLColor(dlg.Color);
                GL.ClearColor(dlg.Color);
                invalidateView();
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
