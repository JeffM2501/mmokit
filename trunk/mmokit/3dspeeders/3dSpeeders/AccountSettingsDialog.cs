using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace _3dSpeeders
{
    public partial class AccountSettingsDialog : Form
    {
        ClientConfig config;

        public AccountSettingsDialog( ClientConfig cfg, DirectoryInfo dataDir )
        {
            config = cfg;
            InitializeComponent();

            SavePassword.Checked = config.savePassword;
            SaveUsername.Checked = config.saveUsername;

            DirectoryInfo avatarDir = new DirectoryInfo(Path.Combine(dataDir.FullName,"Avatars"));
            if (avatarDir.Exists)
            {
                foreach(FileInfo f in avatarDir.GetFiles("*.png"))
                {
                    Image image = Image.FromFile(f.FullName);
                    AvatarImage.LargeImageList.Images.Add(image);

                    string name = f.Name.Replace(f.Extension,string.Empty);
                    ListViewItem item = AvatarImage.Items.Add(name,AvatarImage.LargeImageList.Images.Count-1);

                    if (name == config.avatar)
                        item.Selected = true;
                }
                Avatar.Checked = config.avatar != string.Empty;

            }
            else
            {
                Avatar.Checked = false;
                Avatar.Enabled = false;
                AvatarImage.Enabled = false;
            }
            enableItems();
        }

        void enableItems ()
        {
            SavePassword.Enabled = SaveUsername.Checked;
            if (AvatarImage.Items.Count != 0)
            {
                AvatarImage.Enabled = Avatar.Checked;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            config.savePassword = SavePassword.Checked && SaveUsername.Checked;
            config.saveUsername = SaveUsername.Checked;

            if (!Avatar.Checked || (AvatarImage.Items.Count == 0 || AvatarImage.SelectedItems.Count == 0))
                config.avatar = string.Empty;
            else
                config.avatar = AvatarImage.SelectedItems[0].Text;
        }

        private void SaveUsername_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }

        private void SavePassword_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }

        private void AvatarImage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Avatar_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }
    }
}
