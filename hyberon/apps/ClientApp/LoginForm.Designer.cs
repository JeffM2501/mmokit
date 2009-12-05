namespace ClientApp
{
    partial class LoginForm
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
            this.LoginSpliter = new System.Windows.Forms.SplitContainer();
            this.LoginButton = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SaveLogin = new System.Windows.Forms.CheckBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NewsBrowser = new System.Windows.Forms.WebBrowser();
            this.LoginChecker = new System.Windows.Forms.Timer(this.components);
            this.LoginSpliter.Panel1.SuspendLayout();
            this.LoginSpliter.Panel2.SuspendLayout();
            this.LoginSpliter.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginSpliter
            // 
            this.LoginSpliter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginSpliter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.LoginSpliter.Location = new System.Drawing.Point(1, 1);
            this.LoginSpliter.Name = "LoginSpliter";
            // 
            // LoginSpliter.Panel1
            // 
            this.LoginSpliter.Panel1.BackgroundImage = global::ClientApp.Properties.Resources._453681587_f035139cab_b;
            this.LoginSpliter.Panel1.Controls.Add(this.LoginButton);
            this.LoginSpliter.Panel1.Controls.Add(this.linkLabel1);
            this.LoginSpliter.Panel1.Controls.Add(this.SaveLogin);
            this.LoginSpliter.Panel1.Controls.Add(this.Password);
            this.LoginSpliter.Panel1.Controls.Add(this.label2);
            this.LoginSpliter.Panel1.Controls.Add(this.Username);
            this.LoginSpliter.Panel1.Controls.Add(this.label1);
            // 
            // LoginSpliter.Panel2
            // 
            this.LoginSpliter.Panel2.Controls.Add(this.NewsBrowser);
            this.LoginSpliter.Size = new System.Drawing.Size(607, 310);
            this.LoginSpliter.SplitterDistance = 213;
            this.LoginSpliter.TabIndex = 0;
            // 
            // LoginButton
            // 
            this.LoginButton.AutoEllipsis = true;
            this.LoginButton.BackColor = System.Drawing.Color.Silver;
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.ForeColor = System.Drawing.Color.Black;
            this.LoginButton.Location = new System.Drawing.Point(126, 126);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(84, 34);
            this.LoginButton.TabIndex = 6;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.linkLabel1.Location = new System.Drawing.Point(4, 102);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 15);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Register";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // SaveLogin
            // 
            this.SaveLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveLogin.AutoSize = true;
            this.SaveLogin.BackColor = System.Drawing.Color.Transparent;
            this.SaveLogin.ForeColor = System.Drawing.Color.White;
            this.SaveLogin.Location = new System.Drawing.Point(62, 103);
            this.SaveLogin.Name = "SaveLogin";
            this.SaveLogin.Size = new System.Drawing.Size(148, 17);
            this.SaveLogin.TabIndex = 4;
            this.SaveLogin.Text = "Remember Authentication";
            this.SaveLogin.UseVisualStyleBackColor = false;
            this.SaveLogin.CheckedChanged += new System.EventHandler(this.SaveLogin_CheckedChanged);
            // 
            // Password
            // 
            this.Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Password.BackColor = System.Drawing.Color.DarkGray;
            this.Password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Password.ForeColor = System.Drawing.SystemColors.Window;
            this.Password.Location = new System.Drawing.Point(7, 77);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '•';
            this.Password.Size = new System.Drawing.Size(194, 20);
            this.Password.TabIndex = 3;
            this.Password.TextChanged += new System.EventHandler(this.Username_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(3, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // Username
            // 
            this.Username.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Username.BackColor = System.Drawing.Color.DarkGray;
            this.Username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Username.ForeColor = System.Drawing.SystemColors.Window;
            this.Username.Location = new System.Drawing.Point(7, 31);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(194, 20);
            this.Username.TabIndex = 1;
            this.Username.TextChanged += new System.EventHandler(this.Username_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // NewsBrowser
            // 
            this.NewsBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NewsBrowser.Location = new System.Drawing.Point(3, 0);
            this.NewsBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.NewsBrowser.Name = "NewsBrowser";
            this.NewsBrowser.Size = new System.Drawing.Size(384, 307);
            this.NewsBrowser.TabIndex = 0;
            // 
            // LoginChecker
            // 
            this.LoginChecker.Tick += new System.EventHandler(this.LoginChecker_Tick);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 311);
            this.Controls.Add(this.LoginSpliter);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.LoginSpliter.Panel1.ResumeLayout(false);
            this.LoginSpliter.Panel1.PerformLayout();
            this.LoginSpliter.Panel2.ResumeLayout(false);
            this.LoginSpliter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer LoginSpliter;
        private System.Windows.Forms.WebBrowser NewsBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox SaveLogin;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Timer LoginChecker;
    }
}

