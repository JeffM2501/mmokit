using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace modeler
{
    public partial class SingleItemQuery : Form
    {
        public string Title = string.Empty;
        public string MessageLabel = string.Empty;
        public string Value = string.Empty;

        public SingleItemQuery()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Value = textBox1.Text;
        }

        private void SingleItemQuery_Shown(object sender, EventArgs e)
        {
            if (Title != string.Empty)
                this.Text = Title;
            Label.Text = MessageLabel;
            textBox1.Text = Value;
        }
    }
}
