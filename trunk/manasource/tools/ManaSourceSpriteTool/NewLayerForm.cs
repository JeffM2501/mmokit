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
    public partial class NewLayerForm : Form
    {
        public bool SpriteOnly = false;

        public string SpriteName = string.Empty;
        public string SpriteDefaultAction = string.Empty;
        public string XMLFileLocation = string.Empty;
        public string ImageSetName = string.Empty;
        public string ImageSetPath = string.Empty;
        public string ImageSetFile= string.Empty;
        public Size ImageSetGridSize = new Size(64, 64);

        public NewLayerForm()
        {
            InitializeComponent();
            ImageSetGroup.Enabled = !SpriteOnly;
        }

        private void NewLayerForm_Load(object sender, EventArgs e)
        {
            NameItem.Text = SpriteName;
            DefaultActionItem.Text = SpriteDefaultAction;
            XMLFileItem.Text = XMLFileLocation;
            ImageSetNameItem.Text = ImageSetName;
            ImageLocationItem.Text = ImageSetFile;
            GridX.Value = ImageSetGridSize.Width;
            GridY.Value = ImageSetGridSize.Height;
            ImagePathItem.Text = ImageSetPath;
        }

        private void BrowseXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML Document |*.XML|All Files|*.*";
            if (XMLFileItem.Text != string.Empty)
            {
                sfd.FileName = Path.GetFileName(XMLFileItem.Text);
                sfd.InitialDirectory = Path.GetDirectoryName(XMLFileItem.Text);
            }
            else
                sfd.FileName = "New_Sprite.xml";

            if (sfd.ShowDialog(this) == DialogResult.OK)
                XMLFileItem.Text = sfd.FileName;
        }

        private void BrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image|*.PNG|All Files|*.*";
            if (ImageLocationItem.Text != string.Empty)
            {
                ofd.FileName = Path.GetFileName(ImageLocationItem.Text);
                ofd.InitialDirectory = Path.GetDirectoryName(ImageLocationItem.Text);
            }

            if (ofd.ShowDialog(this) == DialogResult.OK)
                ImageLocationItem.Text = ofd.FileName;
        }

        private void ImageLocationItem_TextChanged(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(ImageLocationItem.Text);
            if (file.Exists)
            {
                try
                {
                    ImageBox.Image = new Bitmap(file.FullName);
                }
                catch (System.Exception /*ex*/)
                {
                    ImageBox.Image = null;
                }

                if (ImageBox.Image != null)
                {
                    if (GridY.Value > ImageBox.Image.Height)
                        GridY.Value = ImageBox.Image.Height;
                    if (GridX.Value > ImageBox.Image.Width)
                        GridX.Value = ImageBox.Image.Width;
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (SpriteOnly)
            {
                if (NameItem.Text == string.Empty || XMLFileItem.Text == string.Empty)
                {
                    MessageBox.Show(this, "Fields must not be empty");
                    DialogResult = DialogResult.None;
                    return;
                }

            }
            else
            {
                if (NameItem.Text == string.Empty || XMLFileItem.Text == string.Empty || ImagePathItem.Text == string.Empty || ImageLocationItem.Text == string.Empty)
                {
                    MessageBox.Show(this, "Fields must not be empty");
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            SpriteName = NameItem.Text;
            if (DefaultActionItem.Text == string.Empty)
                SpriteDefaultAction = "stand";
            else
                SpriteDefaultAction = DefaultActionItem.Text;

            XMLFileLocation = XMLFileItem.Text;
            ImageSetName = ImageSetNameItem.Text;
            ImageSetFile = ImageLocationItem.Text;

            ImageSetGridSize = new Size((int)GridX.Value, (int)GridY.Value);
            ImageSetPath = ImagePathItem.Text;
        }
    }
}
