using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3dSpeeders
{
    public partial class AudioOptionsDialog : Form
    {
        ClientConfig config;
        public AudioOptionsDialog( ClientConfig cfg )
        {
            config = cfg;
            InitializeComponent();

            EnableSound.Checked = config.enableSound;
            VolumeVal.Text = config.volume.ToString();
            VolumeBar.Value = config.volume;

            enableItems();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
            config.enableSound = EnableSound.Checked;
            if (config.enableSound)
                config.volume = VolumeBar.Value;
        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            VolumeVal.Text = VolumeBar.Value.ToString();
        }

        void enableItems ()
        {
            VolumeBar.Enabled = EnableSound.Checked;
            VolumeLabel.Enabled = EnableSound.Checked;
            VolumeVal.Enabled = EnableSound.Checked;
        }

        private void EnableSound_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }
    }
}
