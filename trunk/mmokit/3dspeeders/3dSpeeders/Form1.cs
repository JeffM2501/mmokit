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

namespace _3dSpeeders
{
    public partial class Form1 : Form
    {
        public ConnectionInfo connectionInfo = new ConnectionInfo();

        public ClientConfig config;

        public Form1( DirectoryInfo dir)
        {
            connectionInfo.dataDir = dir;

            InitializeComponent();
            Play.Enabled = false;
            Login.Enabled = false;

            ResizeRedraw = true;

            loadConfig();
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
                connectionInfo.port = 6088;
            else
                connectionInfo.port = int.Parse(ServerPort.Text);

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
            if (new AccountSettingsDialog(config,connectionInfo.dataDir).ShowDialog() == DialogResult.OK)
                saveConfig();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
        }
    }
}
