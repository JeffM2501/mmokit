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
    public partial class VideoOptionsDialog : Form
    {
        ClientConfig config;

        string xval = string.Empty;
        string yval = string.Empty;

        public VideoOptionsDialog( ClientConfig cfg )
        {
            config = cfg;
            InitializeComponent();

            xval = config.resolutionX.ToString();
            yval = config.resolutionY.ToString();
            XRes.Text = string.Copy(xval);
            YRes.Text = string.Copy(yval);
            if (config.fullscreen)
            {
                Fullscreen.Checked = true;
                XRes.Text = Screen.PrimaryScreen.Bounds.Width.ToString();
                YRes.Text = Screen.PrimaryScreen.Bounds.Height.ToString();
            }   
        }

        void checkFields ()
        {
            if (Fullscreen.Checked)
            {
                XRes.Text = Screen.PrimaryScreen.Bounds.Width.ToString();
                YRes.Text = Screen.PrimaryScreen.Bounds.Height.ToString();

                XRes.Enabled = false;
                Xlabel.Enabled = false;
                YRes.Enabled = false;
                YLabel.Enabled = false;
            }
            else
            {
                XRes.Enabled = true;
                Xlabel.Enabled = true;
                YRes.Enabled = true;
                YLabel.Enabled = true;

                XRes.Text = xval;
                YRes.Text = yval;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
            config.fullscreen = Fullscreen.Checked;
            config.resolutionX = int.Parse(XRes.Text);
            config.resolutionY = int.Parse(YRes.Text);
        }

        private void Fullscreen_CheckedChanged(object sender, EventArgs e)
        {
            if (Fullscreen.Checked)
            {
                xval = XRes.Text;
                yval = YRes.Text;
            }
            checkFields();
        }
    }
}
