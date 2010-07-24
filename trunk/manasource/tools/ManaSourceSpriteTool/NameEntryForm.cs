using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManaSourceSpriteTool
{
    public partial class NameEntryForm : Form
    {
        public string DialogName = string.Empty;
        public string TextLabel = string.Empty;
        public string TextValue = string.Empty;


        public NameEntryForm()
        {
            InitializeComponent();
            this.Text = DialogName;
            LabelText.Text = TextLabel;
            textBox1.Text = TextValue;
            TextValue = string.Empty;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            TextValue = textBox1.Text;
        }
    }
}
