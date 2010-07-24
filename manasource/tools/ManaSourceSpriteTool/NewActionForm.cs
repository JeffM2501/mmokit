using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManaSourceSpriteTool
{
    public partial class NewActionForm : Form
    {
        public Dictionary<string, ManaSource.Sprites.ImageSet> ImageSets = new Dictionary<string, ManaSource.Sprites.ImageSet>();

        public string SelectdImageSet = string.Empty;
        public string SelectedActionName = string.Empty;
        public bool CardinalDirections = true;

        public NewActionForm()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            CardinalDirections = CardinalRadio.Checked;
            SelectdImageSet = ImageSetList.SelectedItem.ToString();
            SelectedActionName = ActionNameItem.Text;
        }

        private void NewActionForm_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, ManaSource.Sprites.ImageSet> img in ImageSets)
                ImageSetList.Items.Add(img.Key);

            if (SelectdImageSet != string.Empty)
                ImageSetList.SelectedItem = SelectdImageSet;
            else
                ImageSetList.SelectedIndex = 0;

            ActionNameItem.Text = SelectedActionName;
            CardinalRadio.Checked = CardinalDirections;
            AnyRadio.Checked = !CardinalDirections;
        }
    }
}
