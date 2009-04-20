namespace MapEdit.Dialog_Boxes
{
    partial class ObjectEdit
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
            this.SkinList = new System.Windows.Forms.ComboBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ZPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.YPos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.XPos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ModelBox = new System.Windows.Forms.GroupBox();
            this.ModelName = new System.Windows.Forms.TextBox();
            this.ZRot = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.YRot = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.XRot = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ScaleUV = new System.Windows.Forms.CheckBox();
            this.ScaleZ = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ScaleY = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ScaleX = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ObjectName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.ModelBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SkinList
            // 
            this.SkinList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SkinList.FormattingEnabled = true;
            this.SkinList.Location = new System.Drawing.Point(49, 37);
            this.SkinList.Name = "SkinList";
            this.SkinList.Size = new System.Drawing.Size(121, 21);
            this.SkinList.TabIndex = 0;
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(172, 338);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(91, 338);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Skin";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ZPos);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.YPos);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.XPos);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 54);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // ZPos
            // 
            this.ZPos.Location = new System.Drawing.Point(175, 17);
            this.ZPos.Name = "ZPos";
            this.ZPos.Size = new System.Drawing.Size(48, 20);
            this.ZPos.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(155, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Z";
            // 
            // YPos
            // 
            this.YPos.Location = new System.Drawing.Point(101, 17);
            this.YPos.Name = "YPos";
            this.YPos.Size = new System.Drawing.Size(48, 20);
            this.YPos.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y";
            // 
            // XPos
            // 
            this.XPos.Location = new System.Drawing.Point(27, 17);
            this.XPos.Name = "XPos";
            this.XPos.Size = new System.Drawing.Size(48, 20);
            this.XPos.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "X";
            // 
            // ModelBox
            // 
            this.ModelBox.Controls.Add(this.ModelName);
            this.ModelBox.Controls.Add(this.label1);
            this.ModelBox.Controls.Add(this.SkinList);
            this.ModelBox.Location = new System.Drawing.Point(12, 35);
            this.ModelBox.Name = "ModelBox";
            this.ModelBox.Size = new System.Drawing.Size(237, 69);
            this.ModelBox.TabIndex = 5;
            this.ModelBox.TabStop = false;
            this.ModelBox.Text = "Model";
            // 
            // ModelName
            // 
            this.ModelName.Location = new System.Drawing.Point(49, 11);
            this.ModelName.Name = "ModelName";
            this.ModelName.ReadOnly = true;
            this.ModelName.Size = new System.Drawing.Size(121, 20);
            this.ModelName.TabIndex = 4;
            // 
            // ZRot
            // 
            this.ZRot.Location = new System.Drawing.Point(175, 17);
            this.ZRot.Name = "ZRot";
            this.ZRot.Size = new System.Drawing.Size(48, 20);
            this.ZRot.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ZRot);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.YRot);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.XRot);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(13, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 54);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotation";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Z";
            // 
            // YRot
            // 
            this.YRot.Enabled = false;
            this.YRot.Location = new System.Drawing.Point(101, 17);
            this.YRot.Name = "YRot";
            this.YRot.Size = new System.Drawing.Size(48, 20);
            this.YRot.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(81, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Y";
            // 
            // XRot
            // 
            this.XRot.Enabled = false;
            this.XRot.Location = new System.Drawing.Point(27, 17);
            this.XRot.Name = "XRot";
            this.XRot.Size = new System.Drawing.Size(48, 20);
            this.XRot.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(7, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "X";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ScaleUV);
            this.groupBox3.Controls.Add(this.ScaleZ);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.ScaleY);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.ScaleX);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(13, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(237, 77);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Position";
            // 
            // ScaleUV
            // 
            this.ScaleUV.AutoSize = true;
            this.ScaleUV.Location = new System.Drawing.Point(10, 54);
            this.ScaleUV.Name = "ScaleUV";
            this.ScaleUV.Size = new System.Drawing.Size(127, 17);
            this.ScaleUV.TabIndex = 6;
            this.ScaleUV.Text = "Scale UV with Object";
            this.ScaleUV.UseVisualStyleBackColor = true;
            // 
            // ScaleZ
            // 
            this.ScaleZ.Location = new System.Drawing.Point(175, 17);
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(48, 20);
            this.ScaleZ.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(155, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Z";
            // 
            // ScaleY
            // 
            this.ScaleY.Location = new System.Drawing.Point(101, 17);
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(48, 20);
            this.ScaleY.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Y";
            // 
            // ScaleX
            // 
            this.ScaleX.Location = new System.Drawing.Point(27, 17);
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(48, 20);
            this.ScaleX.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "X";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Name";
            // 
            // ObjectName
            // 
            this.ObjectName.Location = new System.Drawing.Point(59, 9);
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.Size = new System.Drawing.Size(188, 20);
            this.ObjectName.TabIndex = 8;
            // 
            // ObjectEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 373);
            this.Controls.Add(this.ObjectName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ModelBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ObjectEdit";
            this.Text = "ObjectEdit";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ModelBox.ResumeLayout(false);
            this.ModelBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SkinList;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ZPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox YPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox XPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox ModelBox;
        private System.Windows.Forms.TextBox ModelName;
        private System.Windows.Forms.TextBox ZRot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox YRot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox XRot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox ScaleZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ScaleY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ScaleX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox ScaleUV;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox ObjectName;
    }
}