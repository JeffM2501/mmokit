using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GraphicWorlds;
using World;
using Drawables.Materials;

namespace MapEdit.Dialog_Boxes
{
    public partial class MapInfoDlog : Form
    {
        GraphicWorld world;

        public MapInfoDlog( GraphicWorld w )
        {
            world = w;
            InitializeComponent();

            int groundItem = 0;
            int wallItem = 0;
            // fill the mat lists;

            GroundMatList.Items.Add("None");
            WallMatList.Items.Add("None");

            int index = 1;
            if (world.materials != null)
            {
                foreach(KeyValuePair<string,Material> m in world.materials)
                {
                    GroundMatList.Items.Add(m.Key);
                    WallMatList.Items.Add(m.Key);

                    if (m.Key == world.world.groundMaterialName)
                        groundItem = index;
                    if (m.Key == world.world.wallMaterialName)
                        wallItem = index;

                    index++;
                }
            }
            GroundMatList.SelectedIndex = groundItem;
            WallMatList.SelectedIndex = wallItem;

            WallHeight.Text = world.world.wallHeight.ToString();

            UVSize.Text = world.world.groundUVSize.ToString();

            XSize.Text = world.world.size.X.ToString();
            YSize.Text = world.world.size.Y.ToString();
            ZSize.Text = world.world.size.Z.ToString();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (XSize.Text != string.Empty)
                world.world.size.X = float.Parse(XSize.Text);
            if (YSize.Text != string.Empty)
                world.world.size.Y = float.Parse(YSize.Text);
            if (ZSize.Text != string.Empty)
                world.world.size.Z = float.Parse(ZSize.Text);
            if (WallHeight.Text != string.Empty)
                world.world.wallHeight = float.Parse(WallHeight.Text);
            if (UVSize.Text != string.Empty)
                world.world.groundUVSize = float.Parse(UVSize.Text);

            if (GroundMatList.SelectedIndex == 0)
                world.world.groundMaterialName = string.Empty;
            else
                world.world.groundMaterialName = GroundMatList.SelectedItem.ToString();
            if (WallMatList.SelectedIndex == 0)
                world.world.wallMaterialName = string.Empty;
            else
                world.world.wallMaterialName = WallMatList.SelectedItem.ToString();
        }
    }
}
