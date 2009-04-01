using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK.Graphics;

namespace _3dSpeeders
{
    public partial class VideoOptionsDialog : Form
    {
        ClientConfig config;
        ConnectionInfo info;

        bool hasFSAA = false;

        string fullFSSA = string.Empty;

        string xval = string.Empty;
        string yval = string.Empty;

        public VideoOptionsDialog( ClientConfig cfg, ConnectionInfo cinfo )
        {
            config = cfg;
            info = cinfo;

            InitializeComponent();

            int index = 1;
            foreach (DisplayDevice dev in DisplayDevice.AvailableDisplays)
            {
                string name = "Device " + index.ToString();
                if (dev.IsPrimary)
                    name += " (Primary)";

                index++;

                DisplayList.Items.Add(name);
            }

            index = 0;
            if (config.device != string.Empty)
                index = int.Parse(config.device);

            DisplayList.SelectedIndex = index;
            DisplayList.SelectedItem = DisplayList.Items[index];

            xval = config.resolutionX.ToString();
            yval = config.resolutionY.ToString();

            XRes.Text = xval;
            YRes.Text = yval;

            VSync.Checked = config.vsync;

            setFieldsForCurrentDevice();
        }

        DisplayResolution getSelectedRes ( )
        {
            int device = DisplayList.SelectedIndex;

            if (device < 0)
                device = 0;
            DisplayDevice dev = DisplayDevice.AvailableDisplays[device];


            int index = FullscreenList.SelectedIndex;
            if (index < 0)
                index = dev.AvailableResolutions.Length - 1;

            return dev.AvailableResolutions[index];
        }

        void setFieldsForCurrentDevice()
        {
            int device = DisplayList.SelectedIndex;

            if (device < 0)
                device = 0;
            DisplayDevice dev = DisplayDevice.AvailableDisplays[device];

            FullscreenList.Items.Clear();

            foreach(DisplayResolution r in dev.AvailableResolutions)
            {
                int index = FullscreenList.Items.Add(r.ToString());
                if (r.Width.ToString() == xval && r.Height.ToString() == yval)
                    FullscreenList.SelectedIndex = index;
            }

            if ( (Fullscreen.Checked && FullscreenList.SelectedIndex < 0) || !Fullscreen.Checked)
                FullscreenList.SelectedIndex = FullscreenList.Items.Count - 1;

            FullscreenList.SelectedItem = FullscreenList.Items[FullscreenList.SelectedIndex];
            checkFields();
        }

        void checkFields ()
        {
            if (FullscreenList.SelectedItem == null)
                return;

            FSAA.Enabled = hasFSAA;
            FSAAList.Enabled = hasFSAA;

            FullscreenList.Enabled = Fullscreen.Checked;

            if (Fullscreen.Checked)
                setResFromPulldown();

            XRes.Enabled = !Fullscreen.Checked;
            Xlabel.Enabled = !Fullscreen.Checked;
            YRes.Enabled = !Fullscreen.Checked;
            YLabel.Enabled = !Fullscreen.Checked;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
            config.device = DisplayList.SelectedIndex.ToString();
            config.fullscreen = Fullscreen.Checked;
            config.resolutionX = int.Parse(XRes.Text);
            config.resolutionY = int.Parse(YRes.Text);
            config.vsync = VSync.Checked;

            if (config.fullscreen)
            {
                DisplayResolution r = getSelectedRes();

                config.refresh = r.RefreshRate;
            }
            else
                config.refresh = 0;
        }

        private void Fullscreen_CheckedChanged(object sender, EventArgs e)
        {
            FullscreenList.Enabled = Fullscreen.Checked;
            checkFields();
        }

        private void RendererList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFieldsForCurrentDevice();
        }

        void setResFromPulldown ()
        {
            DisplayResolution r = getSelectedRes();

            XRes.Text = r.Width.ToString();
            YRes.Text = r.Height.ToString();
            xval = XRes.Text;
            yval = YRes.Text;
        }

        private void FullscreenList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setResFromPulldown();
        }

        private void FSAAList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
