namespace _3dSpeeders
{
    partial class AccountSettingsDialog
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
            this.components = new System.ComponentModel.Container();
            this.SavePassword = new System.Windows.Forms.CheckBox();
            this.SaveUsername = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Avatar = new System.Windows.Forms.CheckBox();
            this.AvatarImage = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SavePassword
            // 
            this.SavePassword.AutoSize = true;
            this.SavePassword.Location = new System.Drawing.Point(7, 48);
            this.SavePassword.Name = "SavePassword";
            this.SavePassword.Size = new System.Drawing.Size(100, 17);
            this.SavePassword.TabIndex = 0;
            this.SavePassword.Text = "Save Password";
            this.SavePassword.UseVisualStyleBackColor = true;
            this.SavePassword.CheckedChanged += new System.EventHandler(this.SavePassword_CheckedChanged);
            // 
            // SaveUsername
            // 
            this.SaveUsername.AutoSize = true;
            this.SaveUsername.Location = new System.Drawing.Point(7, 25);
            this.SaveUsername.Name = "SaveUsername";
            this.SaveUsername.Size = new System.Drawing.Size(102, 17);
            this.SaveUsername.TabIndex = 1;
            this.SaveUsername.Text = "Save Username";
            this.SaveUsername.UseVisualStyleBackColor = true;
            this.SaveUsername.CheckedChanged += new System.EventHandler(this.SaveUsername_CheckedChanged);
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(93, 351);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 351);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SavePassword);
            this.groupBox1.Controls.Add(this.SaveUsername);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 75);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Account Info";
            // 
            // Avatar
            // 
            this.Avatar.AutoSize = true;
            this.Avatar.Location = new System.Drawing.Point(12, 87);
            this.Avatar.Name = "Avatar";
            this.Avatar.Size = new System.Drawing.Size(89, 17);
            this.Avatar.TabIndex = 5;
            this.Avatar.Text = "Avatar Image";
            this.Avatar.UseVisualStyleBackColor = true;
            this.Avatar.CheckedChanged += new System.EventHandler(this.Avatar_CheckedChanged);
            // 
            // AvatarImage
            // 
            this.AvatarImage.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.AvatarImage.GridLines = true;
            this.AvatarImage.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.AvatarImage.HideSelection = false;
            this.AvatarImage.LargeImageList = this.imageList1;
            this.AvatarImage.Location = new System.Drawing.Point(12, 110);
            this.AvatarImage.MultiSelect = false;
            this.AvatarImage.Name = "AvatarImage";
            this.AvatarImage.Size = new System.Drawing.Size(255, 226);
            this.AvatarImage.TabIndex = 6;
            this.AvatarImage.TileSize = new System.Drawing.Size(64, 64);
            this.AvatarImage.UseCompatibleStateImageBehavior = false;
            this.AvatarImage.SelectedIndexChanged += new System.EventHandler(this.AvatarImage_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(64, 64);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // AccountSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 386);
            this.Controls.Add(this.AvatarImage);
            this.Controls.Add(this.Avatar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AccountSettingsDialog";
            this.Text = "Account Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox SavePassword;
        private System.Windows.Forms.CheckBox SaveUsername;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox Avatar;
        private System.Windows.Forms.ListView AvatarImage;
        private System.Windows.Forms.ImageList imageList1;
    }
}