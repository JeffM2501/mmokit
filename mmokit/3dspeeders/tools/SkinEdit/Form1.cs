using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Drawables.Models;
using Drawables.Models.OBJ;
using Drawables.Models.XMDL;
using Drawables.Materials;
using Drawables.Textures;
using Utilities.Paths;

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

            if (prefs.dataRoot == string.Empty)
                prefsToolStripMenuItem_Click(this, EventArgs.Empty);

            TextureSystem.system.rootDir = new DirectoryInfo(prefs.dataRoot);
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
                model.clear();

                OBJFile objReader = new OBJFile();
                model = objReader.read(new FileInfo(fd.FileName));

                if (model.valid())
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
                XMDLFile writer = new XMDLFile();
                writer.write(new FileInfo(sfd.FileName), model);
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
                model.clear();
                XMDLFile reader = new XMDLFile();
                
                model = reader.read(new FileInfo(ofd.FileName));
                if (model.valid())
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

        void setupSkins()
        {
            if (!model.valid())
                return;

            CurrentSkin.Items.Clear();
            CurrentSkin.Items.Add("Default");

            foreach (MaterialOverride s in model.skins)
                CurrentSkin.Items.Add(s.name);

            CurrentSkin.SelectedIndex = 0;
        }

        MaterialOverride getMatOverride()
        {
            if (CurrentSkin.SelectedIndex < 1)
                return null;

            return model.skins[CurrentSkin.SelectedIndex - 1];
        }

        private void NewSkin_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = model.newSkin("New Skin" + model.skins.Count.ToString());
            foreach (Mesh m in model.meshes)
                ovd.addMaterial(m.material.name, m.material);

            setupSkins();
            CurrentSkin.SelectedIndex = CurrentSkin.Items.Count - 1;
        }

        private void RemoveSkin_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = getMatOverride();
            if (ovd == null)
                return;

            model.removeSkin(ovd.name);

            setupSkins();
        }

        private void SkinView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            buildMaterialView();
            invalidateView();
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

        private void CurrentSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            buildSkinTree();
            buildMaterialView();
            invalidateView();
        }

        private void buildMaterialInteface( Material material, Mesh mesh, MeshOverride meshovd )
        {
            TextureBox.Text = Path.GetFileName(material.textureName);
            ColorPanel.BackColor = material.baseColor.ToColor();
            foreach (MeshGroup g in mesh.groups)
            {
                ListViewItem item = HiddenGroups.Items.Add(g.name);
                if (meshovd != null)
                    item.Checked = !meshovd.hiddenGroups.Contains(g.name);
            }
        }

        private Mesh getSelectedMesh ()
        {
            Mesh mesh = null;
            TreeNode matNode = SkinView.SelectedNode;
            if (matNode == null || matNode.Tag == null)
                return mesh;

            if (matNode.Tag.GetType() == typeof(Mesh))
                mesh = matNode.Tag as Mesh;
            else if (matNode.Tag.GetType() == typeof(MeshGroup))
                mesh = matNode.Parent.Tag as Mesh;
            else if (matNode.Tag.GetType() == typeof(Material))
                mesh = matNode.Parent.Tag as Mesh;

            return mesh;
        }

        private Material getSelectedMaterial()
        {
            MaterialOverride ovd = getMatOverride();
            Mesh mesh = getSelectedMesh();
            if (mesh == null)
                return null;

            if (ovd != null)
            {
                MeshOverride movd = ovd.getOverride(mesh.material);
                if (movd != null && movd.newMaterial != null)
                    return movd.newMaterial;
            }

            return mesh.material;
        }

        private void buildMaterialView ()
        {
            TextureBox.Text = string.Empty;
            ColorPanel.BackColor = Color.White;
            HiddenGroups.Items.Clear();

            MaterialOverride ovd = getMatOverride();
            TreeNode matNode = SkinView.SelectedNode;
            if (matNode == null || matNode.Tag == null)
                return;

            Mesh mesh = getSelectedMesh();
            MeshOverride movd = null;
            Material mat = getSelectedMaterial();

            if (ovd != null && mesh != null)
                movd = ovd.getOverride(mesh.material);

            if (mesh != null && mat != null)
                buildMaterialInteface(mat, mesh, movd);

        }

        private void buildSkinTree ()
        {
            SkinView.Nodes.Clear();

            MaterialOverride ovd = getMatOverride();
            if (ovd == null)
            {
                TreeNode rootNode = SkinView.Nodes.Add("Default");
                rootNode.Tag = null;

                foreach(Mesh m in model.meshes)
                {
                    TreeNode meshNode = rootNode.Nodes.Add(m.material.name);
                    meshNode.Tag = m;
                    meshNode.Nodes.Add(Path.GetFileName(m.material.textureName)).Tag = m.material;

                    foreach(MeshGroup g in m.groups)
                        meshNode.Nodes.Add(g.name).Tag = g;
                }
            }
            else
            {
                TreeNode rootNode = SkinView.Nodes.Add(ovd.name);
                rootNode.Tag = ovd;

                foreach (Mesh m in model.meshes)
                {
                    MeshOverride movd = ovd.getOverride(m.material);
                    TreeNode meshNode = rootNode.Nodes.Add(movd.origonalMatName);
                    meshNode.Tag = m;
                    if (movd.newMaterial != null)
                        meshNode.Nodes.Add(Path.GetFileName(movd.newMaterial.textureName)).Tag = movd.newMaterial;

                    foreach (MeshGroup g in m.groups)
                    {
                        TreeNode node = meshNode.Nodes.Add(g.name);
                        node.Tag = g;
                        if (movd.hiddenGroups.Contains(g.name))
                            node.ForeColor = Color.Gray;
                    }
                }
            }
            SkinView.ExpandAll();
        }

        private void SkinView_DoubleClick_1(object sender, EventArgs e)
        {
            MaterialOverride ovd = getMatOverride();
            if (ovd == null)
                return;

            SingleItemQuery query = new SingleItemQuery();
            query.Value = ovd.name;
            query.Title = "Set Skin Name";
            query.MessageLabel = "Skin Name:";

            if (query.ShowDialog() == DialogResult.OK && query.Value != string.Empty)
            {
                if (model.findSkin(query.Value) != null)
                {
                    MessageBox.Show("Skin " + query.Value + " already exists");
                    return;
                }

                int index = CurrentSkin.SelectedIndex;
                CurrentSkin.Items.Insert(CurrentSkin.SelectedIndex, query.Value);
                CurrentSkin.Items.RemoveAt(CurrentSkin.SelectedIndex);
                CurrentSkin.SelectedIndex = index;

                ovd.name = query.Value;
                SkinView.Nodes[0].Text = ovd.name;
                SkinView.Invalidate(true);
            }

            invalidateView();
        }

        private void BrowseTexture_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = getMatOverride();
            Material mat = getSelectedMaterial();
            OpenFileDialog ofd = new OpenFileDialog();

            if (mat == null)
                return;

            ofd.Multiselect = false;
            ofd.ShowReadOnly = false;
            ofd.Filter = "Portable Network Graphics files (*.PNG)|*.PNG|Joing Phogtographic Experts Group files (*.JPG)|*.JPG|All files (*.*)|*.*";
            if (mat.textureName != string.Empty)
            {
                ofd.InitialDirectory = Path.GetDirectoryName(mat.textureName);
                ofd.FileName = Path.GetFileName(mat.textureName);
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ovd != null)
                {
                    MeshOverride movd = ovd.findOverride(mat.name);
                    if (movd.newMaterial == null)
                    {
                        mat.Invalidate();
                        movd.newMaterial = new Material(mat);
                        mat = movd.newMaterial;
                    }
                }

                mat.Invalidate();
                mat.textureName = PathUtils.MakePathRelitive(prefs.dataRoot,ofd.FileName);

                // find the texture item in the material
                foreach(TreeNode meshNode in SkinView.Nodes[0].Nodes)
                {
                    if (meshNode.Text == mat.name)
                    {
                        if (meshNode.Nodes[0].Tag.GetType() == typeof(Material))
                        {
                            meshNode.Nodes[0].Tag = mat;
                            meshNode.Nodes[0].Text =Path.GetFileName(mat.textureName);
                        }
                        else
                            meshNode.Nodes.Insert(0, Path.GetFileName(mat.textureName)).Tag = mat;
                    }
                }
                TextureBox.Text = Path.GetFileName(mat.textureName);

                invalidateView();
            }

        }

        private void SetColor_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = getMatOverride();
            Material mat = getSelectedMaterial();
            OpenFileDialog ofd = new OpenFileDialog();

            if (mat == null)
                return;

            ColorDialog colorDlog = new ColorDialog();
            colorDlog.Color = mat.baseColor.ToColor();

            if (colorDlog.ShowDialog() == DialogResult.OK)
            {
                if (ovd != null)
                {
                    MeshOverride movd = ovd.findOverride(mat.name);
                    if (movd.newMaterial == null)
                    {
                        mat.Invalidate();
                        movd.newMaterial = new Material(mat);
                        mat = movd.newMaterial;
                    }
                }
                mat.baseColor = new GLColor(colorDlog.Color);
                mat.Invalidate();

                ColorPanel.BackColor = colorDlog.Color;

                invalidateView();
            }
        }

        private void prefsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SingleItemQuery d = new SingleItemQuery();
            d.Title = "Data Root";
            d.MessageLabel = "Data folder";
            d.Value = prefs.dataRoot;

            if (d.ShowDialog() == DialogResult.OK)
            {
                prefs.dataRoot = d.Value;
                savePrefs();

                TextureSystem.system.Invalidate();
                TextureSystem.system.rootDir = new DirectoryInfo(prefs.dataRoot);
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

        public string dataRoot = string.Empty;
    }
}
