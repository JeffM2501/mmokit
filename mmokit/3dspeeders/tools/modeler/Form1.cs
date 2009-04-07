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
        Dictionary<string, MaterialOverride> skins = new Dictionary<string, MaterialOverride>();

        string docName = string.Empty;

        bool showNormals = false;
        bool showGrid = true;
        bool showWireframe = false;
        bool useHeadlight = false;

        public ModelerDialog()
        {
            InitializeComponent();

            gridToolStripMenuItem.Checked = showGrid;
            normalsToolStripMenuItem.Checked = showNormals;
            wireframeToolStripMenuItem.Checked = showWireframe;
            headlightToolStripMenuItem.Checked = useHeadlight;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            setupDisplay();
            camera.move(new Vector3(0, 0, 0));
            camera.pushpull(5);
            camera.pan(45, 15);
        }

        protected void invalidateView()
        {
            glControl1.Invalidate(true);
        }

        void clearModel()
        {
            model.clear();
            foreach (KeyValuePair<string, MaterialOverride> m in skins)
                m.Value.Invalidate();
            skins.Clear();
        }

        void setupDisplay()
        {
            GL.ClearColor(Color.DarkGray);
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

            if (showGrid)
                grid.Execute();

            if(!useHeadlight)
                GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(-10, -15, 20, 0.0f));
            GL.Enable(EnableCap.Lighting);
        //    GL.Color4(Color.Red);

            GL.PushMatrix();
          //  GL.Rotate(90, 1, 0, 0);
            MaterialOverride ovd = getMatOverride();
            if (ovd == null)
                model.drawAll(showNormals, showWireframe);
            else
                model.drawAll(showNormals, showWireframe, ovd);

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
            showGrid = !showGrid;
            gridToolStripMenuItem.Checked = showGrid;
            invalidateView();
        }

        private void normalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNormals = !showNormals;
            normalsToolStripMenuItem.Checked = showNormals;
            invalidateView();
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showWireframe = !showWireframe;
            wireframeToolStripMenuItem.Checked = showWireframe;
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
            SkinView.Nodes.Clear();

            if (!model.valid())
                return;

            // add the default node
            TreeNode defaultNode = SkinView.Nodes.Add(docName);
            defaultNode.Tag = "dr";
            foreach (Mesh m in model.meshes)
            {
                string name = m.material.name;
                if (m.material.texture != string.Empty)
                    name += " (" + Path.GetFileName(m.material.texture) + ")";
                TreeNode matNode = defaultNode.Nodes.Add(name);
                matNode.Tag = "dm";

                foreach (MeshGroup g in m.groups)
                    matNode.Nodes.Add(g.name).Tag = "dg";

            }

            foreach(KeyValuePair<string,MaterialOverride> s in skins)
            {
                TreeNode rootNode = SkinView.Nodes.Add(s.Key);
                rootNode.Tag = "sr";
                MaterialOverride ovd = s.Value;
                foreach (Mesh m in model.meshes)
                {
                    MeshOverride meshOvd = ovd.getOverride(m.material);

                    string name = m.material.name;
                    if (meshOvd.newMaterial != null && meshOvd.newMaterial.texture != string.Empty)
                        name += " (" + Path.GetFileName(meshOvd.newMaterial.texture) + ")";
                    TreeNode matNode = rootNode.Nodes.Add(name);

                    matNode.Tag = "sm";
                    foreach (MeshGroup g in m.groups)
                    {
                        TreeNode groupNode = matNode.Nodes.Add(g.name);
                        groupNode.Tag = "sg";
                        if (meshOvd.hiddenGroups.Contains(g.name))
                            groupNode.ForeColor = Color.Gray;
                    }
                }
            }
        }

        string currentOverideName()
        {
            string tag = (string)SkinView.SelectedNode.Tag;

            if (tag == "sr" && skins.ContainsKey(SkinView.SelectedNode.Text))
                return SkinView.SelectedNode.Text;

            if (tag == "sm")
            {
                TreeNode root = SkinView.SelectedNode.Parent;
                if (skins.ContainsKey(root.Text))
                    return root.Text;
            }

            if (tag == "sg")
            {
                TreeNode root = SkinView.SelectedNode.Parent.Parent;
                if (skins.ContainsKey(root.Text))
                    return root.Text;
            }
            return string.Empty;
        }

        MaterialOverride currentOveride()
        {
            string name = currentOverideName();
            if (skins.ContainsKey(name))
                return skins[name];
            return null;
        }

        MaterialOverride getMatOverride()
        {
            if (SkinView.SelectedNode == null)
                return null;

            if (SkinView.SelectedNode.Tag == null)
                return null;

            string tag = (string)SkinView.SelectedNode.Tag;
            if (tag == "dg" || tag == "dr" || tag == "dm")
                return null;

            return currentOveride();
        }

        private void NewSkin_Click(object sender, EventArgs e)
        {
            MaterialOverride ovd = new MaterialOverride();
            foreach(Mesh m in model.meshes)
                ovd.addMaterial(m.material.name, m.material);

            skins.Add("New Skin" + skins.Count.ToString(), ovd);

            setupSkins();
        }

        private void RemoveSkin_Click(object sender, EventArgs e)
        {
            string name = currentOverideName();
            if (name == string.Empty)
                return;

            skins.Remove(name);

            setupSkins();
        }

        private void SkinView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        string getSelectedMatName()
        {
            string tag = (string)SkinView.SelectedNode.Tag;

            if (tag == "sm")
            {
                string[] nubs = SkinView.SelectedNode.Text.Split(" ".ToCharArray());
                if (nubs.Length > 0)
                    return nubs[0];
                return string.Empty;
            }
            else if ( tag == "sg")
            {
                string[] nubs = SkinView.SelectedNode.Parent.Text.Split(" ".ToCharArray());
                if (nubs.Length > 0)
                    return nubs[0];
                return string.Empty;
            }
            return string.Empty;
        }

        private void SkinView_DoubleClick(object sender, EventArgs e)
        {
            MaterialOverride ovd = currentOveride();

            if (ovd == null)
                return;

            string tag = (string)SkinView.SelectedNode.Tag;
            string name = SkinView.SelectedNode.Text;

            if (tag == "dr" || tag == "dr" || tag == "dg")
                return;

            if (tag == "sr")
            {
                SingleItemQuery siq = new SingleItemQuery();
                siq.Title = "Group Name";
                siq.MessageLabel = "New Group Name";
                siq.Value = SkinView.SelectedNode.Text;

                if (siq.ShowDialog() == DialogResult.OK)
                {
                    if (skins.ContainsKey(siq.Value))
                    {
                        MessageBox.Show("Name Exists");
                        return;
                    }
                    skins[siq.Value] = skins[name];
                    skins.Remove(name);
                    SkinView.SelectedNode.Text = siq.Value;
                }
            }
            else if (tag == "sm")
            {
                SingleItemQuery siq = new SingleItemQuery();
                siq.Title = "Texture File";
                siq.MessageLabel = "Texture Path";
                string matName = getSelectedMatName();
                MeshOverride movd = ovd.findOverride(matName);
                if (movd == null)
                    return;

                if (movd.newMaterial != null)
                    siq.Value = movd.newMaterial.texture;

                if (siq.ShowDialog() == DialogResult.OK)
                {
                    if (movd.newMaterial == null)
                        movd.newMaterial = new Material();

                    movd.newMaterial.texture = siq.Value;
                    movd.newMaterial.Invalidate();

                    SkinView.SelectedNode.Text = matName + " (" + Path.GetFileName(siq.Value) + ")";

                    invalidateView();
                }
            }
            else if (tag == "sg")
            {
                string matName = getSelectedMatName();

                MeshOverride movd = ovd.findOverride(matName);
                if (movd == null)
                    return;

                ovd.Invalidate();
                if (movd.hiddenGroups.Contains(name))
                {
                    ovd.showGroup(matName, name);
                    SkinView.SelectedNode.ForeColor = Color.Black;
                }
                else
                {
                    ovd.hideGroup(matName, name);
                    SkinView.SelectedNode.ForeColor = Color.Gray;
                }

                invalidateView();
            }
        }
    }
}
