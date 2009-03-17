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
    public partial class AccountSettingsDialog : Form
    {
        ClientConfig config;

        public AccountSettingsDialog( ClientConfig cfg )
        {
            config = cfg;
            InitializeComponent();

            SavePassword.Checked = config.savePassword;
            SaveUsername.Checked = config.saveUsername;

            enableItems();
        }

        void enableItems ()
        {
            SavePassword.Enabled = SaveUsername.Checked;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            config.savePassword = SavePassword.Checked && SaveUsername.Checked;
            config.saveUsername = SaveUsername.Checked;
        }

        private void SaveUsername_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }

        private void SavePassword_CheckedChanged(object sender, EventArgs e)
        {
            enableItems();
        }
    }
}
