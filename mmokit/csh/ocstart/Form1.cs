using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ocstart
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewsFrame.AllowWebBrowserDrop = false;
            NewsFrame.Navigate("http://www.opencombat.net/oc_game/news");
            PatchInfo.Visible = false;
            PatchStatus.Visible = false;
            PathcProgress.Visible = false;
            PlayDev.Visible = false;

            Register.Enabled = true;
            Login.Enabled = false;
            Play.Enabled = false;
            SaveCred.Enabled = false;
        }

        private void checkLogin ( )
        {
            if (Username.Text != string.Empty && Password.Text != string.Empty)
                Login.Enabled = true;
            else
                Login.Enabled = false;

            Play.Enabled = Login.Enabled;
            Register.Enabled = !Login.Enabled;

            PlayDev.Enabled = Play.Enabled;
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
    }
}