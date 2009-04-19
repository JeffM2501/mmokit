using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEdit.Dialog_Boxes
{
    public partial class Prefs : Form
    {
        Prefrences prefs;
        public Prefs( Prefrences p)
        {
            prefs = p;
            InitializeComponent();

            DataDir.Text = prefs.dataDir;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            prefs.dataDir = DataDir.Text;
        }
    }
}
