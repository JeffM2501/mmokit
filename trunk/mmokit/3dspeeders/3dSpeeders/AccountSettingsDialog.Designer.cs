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
            this.SavePassword = new System.Windows.Forms.CheckBox();
            this.SaveUsername = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SavePassword
            // 
            this.SavePassword.AutoSize = true;
            this.SavePassword.Location = new System.Drawing.Point(12, 49);
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
            this.SaveUsername.Location = new System.Drawing.Point(12, 26);
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
            this.OK.Location = new System.Drawing.Point(118, 72);
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
            this.Cancel.Location = new System.Drawing.Point(37, 72);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // AccountSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 102);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.SaveUsername);
            this.Controls.Add(this.SavePassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AccountSettingsDialog";
            this.Text = "Account Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox SavePassword;
        private System.Windows.Forms.CheckBox SaveUsername;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}