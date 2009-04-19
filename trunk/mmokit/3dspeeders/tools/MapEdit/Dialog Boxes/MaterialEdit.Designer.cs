namespace MapEdit.Dialog_Boxes
{
    partial class MaterialEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.MatName = new System.Windows.Forms.TextBox();
            this.BaseColor = new System.Windows.Forms.Panel();
            this.ColorCode = new System.Windows.Forms.Label();
            this.AlphaLabel = new System.Windows.Forms.Label();
            this.Alpha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextureName = new System.Windows.Forms.TextBox();
            this.BrowseTexture = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // MatName
            // 
            this.MatName.Location = new System.Drawing.Point(54, 13);
            this.MatName.Name = "MatName";
            this.MatName.Size = new System.Drawing.Size(298, 20);
            this.MatName.TabIndex = 1;
            // 
            // BaseColor
            // 
            this.BaseColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BaseColor.Location = new System.Drawing.Point(16, 48);
            this.BaseColor.Name = "BaseColor";
            this.BaseColor.Size = new System.Drawing.Size(49, 39);
            this.BaseColor.TabIndex = 2;
            this.BaseColor.DoubleClick += new System.EventHandler(this.BaseColor_DoubleClick);
            // 
            // ColorCode
            // 
            this.ColorCode.AutoSize = true;
            this.ColorCode.Location = new System.Drawing.Point(72, 48);
            this.ColorCode.Name = "ColorCode";
            this.ColorCode.Size = new System.Drawing.Size(56, 13);
            this.ColorCode.TabIndex = 3;
            this.ColorCode.Text = "ColorCode";
            // 
            // AlphaLabel
            // 
            this.AlphaLabel.AutoSize = true;
            this.AlphaLabel.Location = new System.Drawing.Point(72, 73);
            this.AlphaLabel.Name = "AlphaLabel";
            this.AlphaLabel.Size = new System.Drawing.Size(34, 13);
            this.AlphaLabel.TabIndex = 4;
            this.AlphaLabel.Text = "Alpha";
            // 
            // Alpha
            // 
            this.Alpha.Location = new System.Drawing.Point(112, 70);
            this.Alpha.Name = "Alpha";
            this.Alpha.Size = new System.Drawing.Size(51, 20);
            this.Alpha.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(180, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Texture";
            // 
            // TextureName
            // 
            this.TextureName.Location = new System.Drawing.Point(229, 45);
            this.TextureName.Name = "TextureName";
            this.TextureName.Size = new System.Drawing.Size(123, 20);
            this.TextureName.TabIndex = 7;
            // 
            // BrowseTexture
            // 
            this.BrowseTexture.Location = new System.Drawing.Point(277, 73);
            this.BrowseTexture.Name = "BrowseTexture";
            this.BrowseTexture.Size = new System.Drawing.Size(75, 23);
            this.BrowseTexture.TabIndex = 8;
            this.BrowseTexture.Text = "Browse";
            this.BrowseTexture.UseVisualStyleBackColor = true;
            this.BrowseTexture.Click += new System.EventHandler(this.BrowseTexture_Click);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(277, 118);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 9;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(196, 118);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 10;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // MaterialEdit
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(364, 153);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.BrowseTexture);
            this.Controls.Add(this.TextureName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Alpha);
            this.Controls.Add(this.AlphaLabel);
            this.Controls.Add(this.ColorCode);
            this.Controls.Add(this.BaseColor);
            this.Controls.Add(this.MatName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MaterialEdit";
            this.Text = "MaterialEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MatName;
        private System.Windows.Forms.Panel BaseColor;
        private System.Windows.Forms.Label ColorCode;
        private System.Windows.Forms.Label AlphaLabel;
        private System.Windows.Forms.TextBox Alpha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextureName;
        private System.Windows.Forms.Button BrowseTexture;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}