using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace ocstart
{
    public partial class Launcher : Form
    {
        int state = -1;
        Object threadLock = new Object();
        bool hasDev = false;

        string patcherURL = "http://patch.opencombat.net:8888";

        public Launcher()
        {
            InitializeComponent();
            state = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewsFrame.AllowWebBrowserDrop = false;
            NewsFrame.Navigate("http://www.opencombat.net/oc_game/news");
            PatchInfo.Visible = false;
            PatchStatus.Visible = false;
            PathcProgress.Visible = false;
            PlayDev.Visible = false;
            FullCheck.Visible = false;

            Register.Enabled = true;
            Login.Enabled = false;
            Play.Enabled = false;
            SaveCred.Enabled = false;

            Application.Idle += new EventHandler(checkState);

            new Thread(new ThreadStart(checkForUpdates)).Start();
        }

        public void checkForUpdates ( )
        {
            WebRequest request = WebRequest.Create(patcherURL);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.Accepted)
                return false;

            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            string line = reader.ReadLine();
            while (line != null && line != string.Empty)
            {

            }
            reader.Close();
            dataStream.Close();
            response.Close();


        }

        public void checkState ( Object sender, EventArgs e )
        {
            lock(threadLock)
            {
                // do crap on the state
                if (state == 1)
                {
                    if (!PatchStatus.Visible)
                        PatchStatus.Visible = true;

                    if (!PathcProgress.Visible)
                        PathcProgress.Visible = true;

                    if (!PatchInfo.Visible)
                        PatchInfo.Visible = true;
                }
                else
                {

                }
            }
        }

        private void checkLogin ( )
        {
            bool patching = false;
            lock(threadLock)
            {
                if (state == 1)
                    patching = true;
            }

            if (Username.Text != string.Empty && Password.Text != string.Empty)
                Login.Enabled = true;
            else
                Login.Enabled = false;

            Play.Enabled = Login.Enabled || !patching;
            Register.Enabled = !Login.Enabled;

            PlayDev.Enabled = Play.Enabled && hasDev;
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            if (Username.Text != string.Empty)
                SaveCred.Enabled = true;
            else
                SaveCred.Enabled = false;

            checkLogin();
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            checkLogin();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hey.. Play!", "Play", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hey.. Login!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Register_Click(object sender, EventArgs e)
        {
            string URL = "http://www.opencombat.net/oc_game/register/";
            if (Username.Text != string.Empty)
                URL += "?username=" + Username.Text;
            Process.Start(URL);
        }

        private void PlayDev_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hey.. PlayDev!", "Play Dev", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void FullCheck_Click(object sender, EventArgs e)
        {

        }
    }
}