using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Drawables.Materials;
using Utilities.Paths;

namespace MapEdit.Dialog_Boxes
{
    public partial class MaterialEdit : Form
    {
        Material material;
        string rootPath;

        public MaterialEdit( Material mat, string path)
        {
            material = mat;
            rootPath = path;
            InitializeComponent();

            MatName.Text = material.name;
            TextureName.Text = material.textureName;

            BaseColor.BackColor = material.baseColor.ToColor();
            Alpha.Text = material.baseColor.a.ToString();

        }

        private void BrowseTexture_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlog = new OpenFileDialog();

            dlog.Multiselect = false;
            dlog.ShowReadOnly = false;
            dlog.Filter = "Portable Network Graphics files (*.PNG)|*.PNG|Joint Photographic Experts Group files (*.JPG)|*.JPG|All files (*.*)|*.*";
            if (TextureName.Text != string.Empty)
            {
                dlog.InitialDirectory = Path.GetDirectoryName(Path.Combine(rootPath, TextureName.Text));
                dlog.FileName = Path.GetFileName(TextureName.Text);
            }

            if (dlog.ShowDialog() == DialogResult.OK)
                TextureName.Text = PathUtils.MakePathRelitive(rootPath, dlog.FileName);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
            material.name = MatName.Text;
            material.textureName = TextureName.Text;

            material.baseColor = new GLColor(BaseColor.BackColor);

            if (Alpha.Text != string.Empty)
                material.baseColor.a = float.Parse(Alpha.Text);

            material.Invalidate();
        }

        private void BaseColor_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog dlog = new ColorDialog();
            dlog.Color = BaseColor.BackColor;
            if (dlog.ShowDialog() == DialogResult.OK)
                BaseColor.BackColor = dlog.Color;
        }
    }
}
