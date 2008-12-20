namespace ocstart
{
    partial class Launcher
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
            this.NewsFrame = new System.Windows.Forms.WebBrowser();
            this.Play = new System.Windows.Forms.Button();
            this.PlayDev = new System.Windows.Forms.Button();
            this.PatchStatus = new System.Windows.Forms.Label();
            this.PathcProgress = new System.Windows.Forms.ProgressBar();
            this.PatchInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.Login = new System.Windows.Forms.Button();
            this.Register = new System.Windows.Forms.Button();
            this.SaveCred = new System.Windows.Forms.CheckBox();
            this.LoginGroup = new System.Windows.Forms.GroupBox();
            this.FullCheck = new System.Windows.Forms.Button();
            this.LoginGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // NewsFrame
            // 
            this.NewsFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NewsFrame.Location = new System.Drawing.Point(174, 0);
            this.NewsFrame.MinimumSize = new System.Drawing.Size(20, 20);
            this.NewsFrame.Name = "NewsFrame";
            this.NewsFrame.ScrollBarsEnabled = false;
            this.NewsFrame.Size = new System.Drawing.Size(688, 422);
            this.NewsFrame.TabIndex = 1;
            // 
            // Play
            // 
            this.Play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Play.Location = new System.Drawing.Point(96, 388);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(75, 23);
            this.Play.TabIndex = 2;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // PlayDev
            // 
            this.PlayDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PlayDev.Location = new System.Drawing.Point(15, 388);
            this.PlayDev.Name = "PlayDev";
            this.PlayDev.Size = new System.Drawing.Size(75, 23);
            this.PlayDev.TabIndex = 3;
            this.PlayDev.Text = "Play(dev)";
            this.PlayDev.UseVisualStyleBackColor = true;
            this.PlayDev.Click += new System.EventHandler(this.PlayDev_Click);
            // 
            // PatchStatus
            // 
            this.PatchStatus.AutoSize = true;
            this.PatchStatus.Location = new System.Drawing.Point(9, 210);
            this.PatchStatus.Name = "PatchStatus";
            this.PatchStatus.Size = new System.Drawing.Size(49, 13);
            this.PatchStatus.TabIndex = 4;
            this.PatchStatus.Text = "Patching";
            // 
            // PathcProgress
            // 
            this.PathcProgress.Location = new System.Drawing.Point(12, 226);
            this.PathcProgress.Name = "PathcProgress";
            this.PathcProgress.Size = new System.Drawing.Size(159, 25);
            this.PathcProgress.TabIndex = 5;
            // 
            // PatchInfo
            // 
            this.PatchInfo.AutoSize = true;
            this.PatchInfo.Location = new System.Drawing.Point(9, 252);
            this.PatchInfo.Name = "PatchInfo";
            this.PatchInfo.Size = new System.Drawing.Size(61, 13);
            this.PatchInfo.TabIndex = 6;
            this.PatchInfo.Text = "WorkingOn";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(6, 41);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(134, 20);
            this.Username.TabIndex = 1;
            this.Username.TextChanged += new System.EventHandler(this.Username_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(6, 80);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(134, 20);
            this.Password.TabIndex = 3;
            this.Password.TextChanged += new System.EventHandler(this.Password_TextChanged);
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(65, 130);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(75, 23);
            this.Login.TabIndex = 4;
            this.Login.Text = "Login";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(65, 159);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(75, 23);
            this.Register.TabIndex = 5;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            this.Register.Click += new System.EventHandler(this.Register_Click);
            // 
            // SaveCred
            // 
            this.SaveCred.AutoSize = true;
            this.SaveCred.Location = new System.Drawing.Point(6, 107);
            this.SaveCred.Name = "SaveCred";
            this.SaveCred.Size = new System.Drawing.Size(102, 17);
            this.SaveCred.TabIndex = 6;
            this.SaveCred.Text = "Save Username";
            this.SaveCred.UseVisualStyleBackColor = true;
            // 
            // LoginGroup
            // 
            this.LoginGroup.Controls.Add(this.SaveCred);
            this.LoginGroup.Controls.Add(this.Register);
            this.LoginGroup.Controls.Add(this.Login);
            this.LoginGroup.Controls.Add(this.Password);
            this.LoginGroup.Controls.Add(this.label2);
            this.LoginGroup.Controls.Add(this.Username);
            this.LoginGroup.Controls.Add(this.label1);
            this.LoginGroup.Location = new System.Drawing.Point(12, 12);
            this.LoginGroup.Name = "LoginGroup";
            this.LoginGroup.Size = new System.Drawing.Size(159, 190);
            this.LoginGroup.TabIndex = 0;
            this.LoginGroup.TabStop = false;
            this.LoginGroup.Text = "Login";
            // 
            // FullCheck
            // 
            this.FullCheck.Location = new System.Drawing.Point(96, 359);
            this.FullCheck.Name = "FullCheck";
            this.FullCheck.Size = new System.Drawing.Size(75, 23);
            this.FullCheck.TabIndex = 7;
            this.FullCheck.Text = "Full Check";
            this.FullCheck.UseVisualStyleBackColor = true;
            this.FullCheck.Click += new System.EventHandler(this.FullCheck_Click);
            // 
            // Launcher
            // 
            this.AcceptButton = this.Play;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(862, 423);
            this.Controls.Add(this.FullCheck);
            this.Controls.Add(this.PatchInfo);
            this.Controls.Add(this.PathcProgress);
            this.Controls.Add(this.PatchStatus);
            this.Controls.Add(this.PlayDev);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.NewsFrame);
            this.Controls.Add(this.LoginGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Launcher";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.LoginGroup.ResumeLayout(false);
            this.LoginGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser NewsFrame;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.Button PlayDev;
        private System.Windows.Forms.Label PatchStatus;
        private System.Windows.Forms.ProgressBar PathcProgress;
        private System.Windows.Forms.Label PatchInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.Button Register;
        private System.Windows.Forms.CheckBox SaveCred;
        private System.Windows.Forms.GroupBox LoginGroup;
        private System.Windows.Forms.Button FullCheck;
    }
}

