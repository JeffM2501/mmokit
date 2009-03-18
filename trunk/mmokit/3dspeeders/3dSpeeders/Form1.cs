using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

using Axiom;
using Axiom.Configuration;
using Axiom.Graphics;
using Axiom.Core;



namespace _3dSpeeders
{
    public partial class Form1 : Form
    {
        public ConnectionInfo connectionInfo = new ConnectionInfo();

        public ClientConfig config;

        public Form1()
        {
            InitializeComponent();
            connectionInfo.root = new Root("3dSpeedersConfig.xml", "3dSpeeders.log");
            Play.Enabled = false;
            Login.Enabled = false;

            ResizeRedraw = true;

            loadConfig();

            setupRenderSystem();
        }

        void checkButtons ()
        {
            Play.Enabled = Username.Text != string.Empty && ServerHost.Text != string.Empty;
        }

        void setServerFromHostString ( string host )
        {
            if (host == string.Empty)
            {
                ServerHost.Text = string.Empty;
                ServerPort.Text = string.Empty;
                return;
            }

            if (host.Contains(":"))
            {
                string[] chunks = host.Split(":".ToCharArray());
                ServerHost.Text = chunks[0];
                if (chunks.Length > 1)
                    ServerPort.Text = chunks[1];
                else
                    ServerPort.Text = "6088";
            }
            else
            {
                ServerHost.Text = host;
                ServerPort.Text = "6088";
            }
        }

        void loadConfig ()
        {
            DirectoryInfo specialDir = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
            DirectoryInfo configDir = specialDir.CreateSubdirectory("3dSpeeders");
            FileInfo configFile = new FileInfo(Path.Combine(configDir.FullName, "config.xml"));

            if (!configFile.Exists)
            {
                config = new ClientConfig();
                saveConfig();
            }
            else
            {
                FileStream fs = configFile.OpenRead();
                StreamReader reader = new StreamReader(fs);

                XmlSerializer xml = new XmlSerializer(typeof(ClientConfig));
                config = (ClientConfig)xml.Deserialize(reader);
                reader.Close();
                fs.Close();
            }

            setServerFromHostString(config.lastServer);

            Username.Text = config.username;
            Password.Text = config.password;

            loadServerList();

            checkButtons();
        }

        void setupRenderSystem()
        {
            connectionInfo.renderSystem = null;
            foreach( RenderSystem r in Root.Instance.RenderSystems)
            {
                if (r.Name.Contains(config.renderer))
                {
                    connectionInfo.renderSystem = r;
                    break;
                }
            }

            if (connectionInfo.renderSystem == null)
            {
                connectionInfo.renderSystem = Root.Instance.RenderSystems[0];
                config.renderer = connectionInfo.renderSystem.Name;
            }

            foreach( ConfigOption c in connectionInfo.renderSystem.ConfigOptions)
            {
                if (c.Name == "Full Screen")
                {
                    if (config.fullscreen)
                        c.Value = "Yes";
                    else
                        c.Value = "No";
                }

                if (c.Name == "FSAA")
                {
                    if (config.FSAA == -1)
                        c.Value = c.PossibleValues[c.PossibleValues.Count - 1];
                    else if (config.FSAA == 0)
                        c.Value = c.PossibleValues[0];
                    else
                    {
                        // see if what we have is in the possible values
                        if (c.PossibleValues.Contains(config.FSAA.ToString()))
                            c.Value = config.FSAA.ToString();
                        else
                            c.Value = c.PossibleValues[0];
                    }
                }

                if (c.Name == "Video Mode")
                {
                    string mode = config.resolutionX.ToString() + " x " + config.resolutionY.ToString() + " @ 32-bit colour";
                    if (c.PossibleValues.Contains(mode))
                        c.Value = mode;
                    else
                    {
                        mode = string.Empty;
                        foreach(string m in c.PossibleValues)
                        {
                            if (m.Contains(config.resolutionX.ToString()) && m.Contains(config.resolutionY.ToString()) && m.Contains("32"))
                            {
                                mode = m;
                                break;
                            }
                        }

                        if (mode == string.Empty)
                        {
                            foreach (string m in c.PossibleValues)
                            {
                                if (m.Contains(config.resolutionX.ToString()) && m.Contains(config.resolutionY.ToString()))
                                {
                                    mode = m;
                                    break;
                                }
                            }
                        }

                        if (mode == string.Empty)
                            mode = c.PossibleValues[c.PossibleValues.Count - 1];

                        c.Value = mode;
                    }
                }
            }
        }

        void saveConfig ()
        {
            if (!config.saveUsername)
                config.username = string.Empty;

            if (!config.savePassword)
                config.password = string.Empty;

            DirectoryInfo specialDir = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
            DirectoryInfo configDir = specialDir.CreateSubdirectory("3dSpeeders");
            FileInfo configFile = new FileInfo(Path.Combine(configDir.FullName, "config.xml"));

            FileStream fs = configFile.OpenWrite();
            StreamWriter writer = new StreamWriter(fs);

            XmlSerializer xml = new XmlSerializer(typeof(ClientConfig));
            xml.Serialize(writer, config);
            writer.Close();
            fs.Close();
        }

        void loadServerList ()
        {
            ServerList.Items.Clear();
            ServerList.Items.Add("localhost");
            ServerList.Items.Add("game.opencombat.net:6090");
        }

        private void NewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string URL = "http://www.opencombat.net/oc_game/am/login.php?action=register";
            if (Username.Text != string.Empty)
                URL += "?username=" + Username.Text;
            Process.Start(URL);
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if (ServerHost.Text == string.Empty)
            {
                MessageBox.Show("Please Pick A Valid Serve(WTF!)");
                return;
            }
            connectionInfo.connect = true;
            connectionInfo.server = ServerHost.Text;
            if (ServerPort.Text == string.Empty)
                connectionInfo.server += ":6088";
            else
                connectionInfo.server += ":" + ServerPort.Text;

            Close();
        }

        private void ServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selection = ServerList.SelectedIndex;
            if (selection >= 0)
                setServerFromHostString(ServerList.SelectedItem.ToString());
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            checkButtons();
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            checkButtons();
        }

        private void ServerHost_TextChanged(object sender, EventArgs e)
        {
            checkButtons();
        }

        private void ServerPort_TextChanged(object sender, EventArgs e)
        {
            checkButtons();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connectionInfo.username = Username.Text;
            connectionInfo.password = Password.Text;
            config.username = Username.Text;
            config.password = Password.Text;

            config.lastServer = ServerHost.Text + ":" + ServerPort.Text;
            saveConfig();
        }

        private void videoSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new VideoOptionsDialog(config,connectionInfo).ShowDialog() == DialogResult.OK)
                saveConfig();
        }

        private void audioSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new AudioOptionsDialog(config).ShowDialog() == DialogResult.OK)
                saveConfig();

        }

        private void accountSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new AccountSettingsDialog(config).ShowDialog() == DialogResult.OK)
                saveConfig();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
        }
    }
}
