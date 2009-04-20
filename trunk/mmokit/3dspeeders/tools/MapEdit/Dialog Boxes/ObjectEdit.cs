using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using World;
using GraphicWorlds;
using Drawables.Materials;
using Drawables.Models;

namespace MapEdit.Dialog_Boxes
{
    public partial class ObjectEdit : Form
    {
        WorldObject obj;
        Model model;

        public ObjectEdit( WorldObject o, Model m)
        {
            obj = o;
            model = m;
            InitializeComponent();

            ModelName.Text = obj.objectName;
            if (obj.name != string.Empty)
                ObjectName.Text = obj.name;
            else
                ObjectName.Text = ModelName.Text;

            if (model != null)
            {
                if (model.skins.Count > 0)
                {
                    SkinList.Items.Add("Default");
                    SkinList.SelectedIndex = 0;
                    foreach (MaterialOverride mat in model.skins)
                    {
                        int index = SkinList.Items.Add(mat.name);
                        if (obj.skin == mat.name)
                            SkinList.SelectedIndex = index;
                    }
                }
            }

            XPos.Text = obj.postion.X.ToString();
            YPos.Text = obj.postion.Y.ToString();
            ZPos.Text = obj.postion.Z.ToString();

            XRot.Text = obj.rotation.X.ToString();
            YRot.Text = obj.rotation.Y.ToString();
            ZRot.Text = obj.rotation.Z.ToString();

            ScaleX.Text = obj.scale.X.ToString();
            ScaleY.Text = obj.scale.Y.ToString();
            ScaleZ.Text = obj.scale.Z.ToString();

            ScaleUV.Checked = obj.scaleSkinToSize;

        }

        private void OK_Click(object sender, EventArgs e)
        {
            obj.name = ObjectName.Text;

            if (model != null)
            {
                model.Invalidate();
                if (model.skins.Count > 0)
                {
                    if (SkinList.SelectedIndex > 0)
                        obj.skin = model.skins[SkinList.SelectedIndex - 1].name;
                    else
                        obj.skin = string.Empty;
                }
            }

            if (XPos.Text != string.Empty)
                obj.postion.X = float.Parse(XPos.Text);
            if (YPos.Text != string.Empty)
                obj.postion.Y = float.Parse(YPos.Text);
            if (ZPos.Text != string.Empty)
                obj.postion.Z = float.Parse(ZPos.Text);

            if (XRot.Text != string.Empty)
                obj.rotation.X = float.Parse(XRot.Text);
            if (YRot.Text != string.Empty)
                obj.rotation.Y = float.Parse(YRot.Text);
            if (ZRot.Text != string.Empty)
                obj.rotation.Z = float.Parse(ZRot.Text);

            if (ScaleX.Text != string.Empty)
                obj.scale.X = float.Parse(ScaleX.Text);
            if (ScaleY.Text != string.Empty)
                obj.scale.Y = float.Parse(ScaleY.Text);
            if (ScaleZ.Text != string.Empty)
                obj.scale.Z = float.Parse(ScaleZ.Text);

            obj.scaleSkinToSize = ScaleUV.Checked;
        }
    }
}
