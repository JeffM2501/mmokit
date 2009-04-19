namespace MapEdit.Dialog_Boxes
{
    partial class MapInfoDlog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.XSize = new System.Windows.Forms.TextBox();
            this.YSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ZSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GroundMatList = new System.Windows.Forms.ComboBox();
            this.WallMatList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.WallHeight = new System.Windows.Forms.TextBox();
            this.UVSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(217, 202);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(136, 202);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "X";
            // 
            // XSize
            // 
            this.XSize.Location = new System.Drawing.Point(32, 26);
            this.XSize.Name = "XSize";
            this.XSize.Size = new System.Drawing.Size(68, 20);
            this.XSize.TabIndex = 4;
            // 
            // YSize
            // 
            this.YSize.Location = new System.Drawing.Point(126, 26);
            this.YSize.Name = "YSize";
            this.YSize.Size = new System.Drawing.Size(68, 20);
            this.YSize.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // ZSize
            // 
            this.ZSize.Location = new System.Drawing.Point(220, 26);
            this.ZSize.Name = "ZSize";
            this.ZSize.Size = new System.Drawing.Size(68, 20);
            this.ZSize.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Ground Material";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UVSize);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.WallHeight);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.WallMatList);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.GroundMatList);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(11, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 144);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ground";
            // 
            // GroundMatList
            // 
            this.GroundMatList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GroundMatList.FormattingEnabled = true;
            this.GroundMatList.Location = new System.Drawing.Point(98, 16);
            this.GroundMatList.Name = "GroundMatList";
            this.GroundMatList.Size = new System.Drawing.Size(173, 21);
            this.GroundMatList.TabIndex = 10;
            // 
            // WallMatList
            // 
            this.WallMatList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WallMatList.FormattingEnabled = true;
            this.WallMatList.Location = new System.Drawing.Point(97, 43);
            this.WallMatList.Name = "WallMatList";
            this.WallMatList.Size = new System.Drawing.Size(173, 21);
            this.WallMatList.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Wall Material";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Wall Height";
            // 
            // WallHeight
            // 
            this.WallHeight.Location = new System.Drawing.Point(97, 73);
            this.WallHeight.Name = "WallHeight";
            this.WallHeight.Size = new System.Drawing.Size(37, 20);
            this.WallHeight.TabIndex = 14;
            // 
            // UVSize
            // 
            this.UVSize.Location = new System.Drawing.Point(97, 101);
            this.UVSize.Name = "UVSize";
            this.UVSize.Size = new System.Drawing.Size(37, 20);
            this.UVSize.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "UV Size";
            // 
            // MapInfoDlog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(304, 237);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ZSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.YSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.XSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MapInfoDlog";
            this.Text = "MapInfoDlog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox XSize;
        private System.Windows.Forms.TextBox YSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ZSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox WallMatList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox GroundMatList;
        private System.Windows.Forms.TextBox UVSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox WallHeight;
    }
}