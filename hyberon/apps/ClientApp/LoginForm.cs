using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

using Login;

namespace ClientApp
{
    public partial class LoginForm : Form
    {
        public Int64 Token = -1;
        public UInt64 UID = 0;
        bool logingIn = false;
        Authenticator auth;

        public class LoginConfig
        {
            public bool SaveAuthentication = false;
            public String Username = string.Empty;
            public String Password = string.Empty;
        }

        LoginConfig config;

        FileInfo prefsFile;
        public LoginForm()
        {
            InitializeComponent();
            NewsBrowser.Navigate("http://www.awesomelaser.com");

            DirectoryInfo AppSettingsDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"MMOKIT"));
            if (!AppSettingsDir.Exists)
                AppSettingsDir.Create();

            DirectoryInfo configDir = AppSettingsDir.CreateSubdirectory("ClientApp");

            prefsFile = new FileInfo(Path.Combine(configDir.FullName, "loginInfo.xml"));

            if (prefsFile.Exists)
            {
                XmlSerializer XML = new XmlSerializer(typeof(LoginConfig));
                Stream fs = prefsFile.OpenRead();
                StreamReader sr = new StreamReader(fs);
                config = (LoginConfig)XML.Deserialize(sr);
                sr.Close();
                fs.Close();
            }
            else
                config = new LoginConfig();

            SaveLogin.Checked = config.SaveAuthentication;
            if (config.SaveAuthentication)
            {
                Username.Text = config.Username;
                Password.Text = config.Password;
            }
            else
            {
                config.Username = string.Empty;
                config.Password = string.Empty;
            }

            logingIn = false;
            CheckLogin();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.awesomelaser.com");
        }

        private void SaveLogin_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            CheckLogin();
        }

        protected void CheckLogin()
        {
            LoginButton.Enabled = (Username.Text.Length > 3 && Password.Text.Length > 3);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginButton.Enabled = false;

            if (SaveLogin.Checked)
            {
                config.SaveAuthentication = true;
                config.Username = Username.Text;
                config.Password = Password.Text;
            }
            else
            {
                config.SaveAuthentication = false;
                config.Username = string.Empty;
                config.Password = string.Empty;
            }

            if (prefsFile.Exists)
                prefsFile.Delete();

            XmlSerializer XML = new XmlSerializer(typeof(LoginConfig));
            Stream fs = prefsFile.OpenWrite();
            StreamWriter sw = new StreamWriter(fs);
            XML.Serialize(sw,config);
            sw.Close();
            fs.Close();

            auth = new Authenticator("localhost", 2500, Username.Text, Password.Text);

            LoginChecker.Interval = 100;
            LoginChecker.Start();
        }

        private void LoginChecker_Tick(object sender, EventArgs e)
        {
            if (auth == null)
                return;

            if (auth.Authenticated())
            {
                LoginChecker.Stop();
                Token = auth.GetToken();
                UID = auth.GetUID();
                auth.Kill();
                auth = null;
                if (Token < 0)
                {
                    MessageBox.Show("Login Failure");
                    LoginButton.Enabled = true;
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (auth == null)
                return;

            auth.Kill();
            auth = null;
        }
    }
}
