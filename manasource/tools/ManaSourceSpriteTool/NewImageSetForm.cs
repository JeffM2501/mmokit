using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ManaSourceSpriteTool
{
    public partial class NewImageSetForm : Form
    {
        public Bitmap ImageMap = null;
        public string XMLSrc = string.Empty;
        public string XMLName = string.Empty;
        public string XMLRecolor = string.Empty;

        public string ImageFilePath = string.Empty;

        public Size GridSize = new Size(64, 64);

        public NewImageSetForm()
        {
            InitializeComponent();
        }

        private void ImagePath_TextChanged(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(ImagePath.Text);
            if (file.Exists)
            {
                try
                {
                    ImageMap = new Bitmap(file.FullName);
                }
                catch (System.Exception /*ex*/)
                {
                    ImageMap = null;
                }

                if (ImageMap != null)
                {
                    ImageBox.Image = ImageMap;
                    if (GridY.Value > ImageMap.Height)
                        GridY.Value = ImageMap.Height;
                    if (GridX.Value > ImageMap.Width)
                        GridX.Value = ImageMap.Width;
                }
            }
        }

        private void ImageBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image|*.PNG|All Files|*.*";
            if (ImagePath.Text != string.Empty)
            {
                ofd.FileName = Path.GetFileName(ImagePath.Text);
                ofd.InitialDirectory = Path.GetDirectoryName(ImagePath.Text);
            }

            if (ofd.ShowDialog(this) == DialogResult.OK)
                ImagePath.Text = ofd.FileName;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (XMLPath.Text == string.Empty || ImageSetName.Text == string.Empty || ImagePath.Text == string.Empty)
            {
                MessageBox.Show(this, "Fields must not be empty");
                DialogResult = DialogResult.None;
                return;
            }

            XMLSrc = XMLPath.Text;
            XMLRecolor = Recolor.Text;
            XMLName = ImageSetName.Text;
            ImageFilePath = ImagePath.Text;

            GridSize = new Size((int)GridX.Value, (int)GridY.Value);
        }

        private void NewImageSetForm_Load(object sender, EventArgs e)
        {
            ImagePath.Text = ImageFilePath;

            XMLPath.Text = XMLSrc;
            Recolor.Text = XMLRecolor;
            ImageSetName.Text = XMLName;

            GridY.Value = GridSize.Height;
            GridX.Value = GridSize.Width;
        }
    }
}
