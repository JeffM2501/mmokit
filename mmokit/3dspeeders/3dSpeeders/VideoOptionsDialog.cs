using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Axiom;
using Axiom.Graphics;
using Axiom.Configuration;
using Axiom.Core;

namespace _3dSpeeders
{
    public partial class VideoOptionsDialog : Form
    {
        ClientConfig config;
        ConnectionInfo info;

        bool hasFSAA = false;
        bool hasFullscreen = false;
        bool hasVSync = false;

        string fullFSSA = string.Empty;

        string xval = string.Empty;
        string yval = string.Empty;

        public VideoOptionsDialog( ClientConfig cfg, ConnectionInfo cinfo )
        {
            config = cfg;
            info = cinfo;

            InitializeComponent();

            int currentRenderer = 0;

            foreach (RenderSystem r in Root.Instance.RenderSystems)
            {
                RendererList.Items.Add(r.Name);
                if (r == info.renderSystem)
                {
                    currentRenderer = RendererList.Items.Count - 1;
                }
            }
            RendererList.SelectedIndex = currentRenderer;
            RendererList.SelectedItem = RendererList.Items[RendererList.SelectedIndex];

            xval = config.resolutionX.ToString();
            yval = config.resolutionY.ToString();

            setFieldsForCurrentRenderer();
        }

        RenderSystem getRS ( string name )
        {
            foreach (RenderSystem r in Root.Instance.RenderSystems)
            {
                if (r.Name == name)
                    return r;
            }

            return null;
        }

        void setFieldsForCurrentRenderer ()
        {
            if (RendererList.SelectedItem == null)
                return;

            RenderSystem rs = getRS(RendererList.SelectedItem.ToString());

            FSAAList.Items.Clear();
            FullscreenList.Items.Clear();

            fullFSSA = string.Empty;
            hasFSAA = false;
            hasFullscreen = false;
            hasVSync = false;

            foreach (ConfigOption c in rs.ConfigOptions)
            {
                if (c.Name == "FSAA" || c.Name == "Anti aliasing")
                {
                    hasFSAA = true;
                    int cur = 0;
                    foreach( string item in c.PossibleValues)
                    {
                        FSAAList.Items.Add(item);
                        if (item == c.Value)
                            cur = FSAAList.Items.Count -  1;
                    }

                    fullFSSA = c.PossibleValues[c.PossibleValues.Count - 1];

                    FSAAList.SelectedIndex = cur;
                }
                else if (c.Name == "Full Screen")
                {
                    hasFullscreen = true;
                    Fullscreen.Checked = c.Value == "Yes";
                }
                else if (c.Name == "Video Mode")
                {
                    string[] nugs = c.Value.Split(" ".ToCharArray());

                    XRes.Text = nugs[0];
                    YRes.Text = nugs[2];

                    bool filter16bit = false;

                    foreach (string v in c.PossibleValues)
                    {
                        if (v.Contains("32-bit"))
                        {
                            filter16bit = true;
                            break;
                        }
                    }

                    // now fill out the video modes
                    int item = -1;
                    foreach (string v in c.PossibleValues)
                    {
                        if (v.Contains("16-bit") && filter16bit)
                            continue;

                        FullscreenList.Items.Add(v);
                        if (v == c.Value)
                            item = FullscreenList.Items.Count - 1;
                    }

                    if (item >= 0)
                        FullscreenList.SelectedIndex = item;
                }
                else if (c.Name == "VSync")
                {
                    hasVSync = true;
                    VSync.Enabled = c.Value == "Yes";
                }
            }

            FullscreenList.Enabled = hasFullscreen;
            Fullscreen.Enabled = hasFullscreen;

            FSAA.Enabled = hasFSAA;
            FSAAList.Enabled = hasFSAA;

            checkFields();
        }

        void checkFields ()
        {
            if (FullscreenList.SelectedItem == null)
                return;

            FSAA.Enabled = hasFSAA;
            FSAAList.Enabled = hasFSAA;

            VSync.Enabled = hasVSync;

            if (hasFullscreen && Fullscreen.Checked)
            {
                string[] nugs = FullscreenList.SelectedItem.ToString().Split(" ".ToCharArray());

                XRes.Text = nugs[0];
                YRes.Text = nugs[2];

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
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {

            string rendName = RendererList.SelectedItem.ToString();

            RenderSystem rs = null;

            foreach (RenderSystem r in Root.Instance.RenderSystems)
            {
                if (r.Name == rendName)
                    rs = r;
            }
            if (rs == null)
            {
                // something went bad, bail
                return;
            }

            info.renderSystem = rs;

            config.renderer = rs.Name;

            config.fullscreen = Fullscreen.Checked;
            config.resolutionX = int.Parse(XRes.Text);
            config.resolutionY = int.Parse(YRes.Text);

            if (hasFSAA)
            {
                if (FSAAList.SelectedIndex == FSAAList.Items.Count - 1)
                    config.FSAA = -1;
                else
                    config.FSAA = int.Parse(FSAAList.SelectedItem.ToString());
            }
            else
                config.FSAA = 0;

            if (hasVSync)
                config.vsync = VSync.Checked;
            else
                config.vsync = false;


            foreach (ConfigOption c in rs.ConfigOptions)
            {
                if (c.Name == "FSAA" || c.Name == "Anti aliasing")
                    c.Value = FSAAList.SelectedItem.ToString();
                else if (c.Name == "Video Mode")
                {
                    if (config.fullscreen)
                        c.Value = FullscreenList.SelectedItem.ToString();
                    else
                        c.Value = XRes.Text + " x " + YRes.Text + " @ 32-bit colour";
                }
                else if (c.Name == "Full Screen")
                {
                    if (config.fullscreen)
                        c.Value = "Yes";
                    else
                        c.Value = "No";
                }
                else if (c.Name == "VSync")
                {
                    if (VSync.Checked)
                        c.Value = "Yes";
                    else
                        c.Value = "No";
                }
            }          
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

        private void RendererList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFieldsForCurrentRenderer();
        }

        private void FullscreenList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] nugs = FullscreenList.SelectedItem.ToString().Split(" ".ToCharArray());

            XRes.Text = nugs[0];
            YRes.Text = nugs[2];
        }

        private void FSAAList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
