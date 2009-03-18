namespace _3dSpeeders
{
    partial class VideoOptionsDialog
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
            this.Fullscreen = new System.Windows.Forms.CheckBox();
            this.Xlabel = new System.Windows.Forms.Label();
            this.XRes = new System.Windows.Forms.TextBox();
            this.YRes = new System.Windows.Forms.TextBox();
            this.YLabel = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.FullscreenList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RendererList = new System.Windows.Forms.ComboBox();
            this.FSAA = new System.Windows.Forms.Label();
            this.FSAAList = new System.Windows.Forms.ComboBox();
            this.VSync = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Fullscreen
            // 
            this.Fullscreen.AutoSize = true;
            this.Fullscreen.Location = new System.Drawing.Point(12, 44);
            this.Fullscreen.Name = "Fullscreen";
            this.Fullscreen.Size = new System.Drawing.Size(74, 17);
            this.Fullscreen.TabIndex = 0;
            this.Fullscreen.Text = "Fullscreen";
            this.Fullscreen.UseVisualStyleBackColor = true;
            this.Fullscreen.CheckedChanged += new System.EventHandler(this.Fullscreen_CheckedChanged);
            // 
            // Xlabel
            // 
            this.Xlabel.AutoSize = true;
            this.Xlabel.Location = new System.Drawing.Point(12, 74);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Size = new System.Drawing.Size(14, 13);
            this.Xlabel.TabIndex = 2;
            this.Xlabel.Text = "X";
            // 
            // XRes
            // 
            this.XRes.Location = new System.Drawing.Point(33, 71);
            this.XRes.Name = "XRes";
            this.XRes.Size = new System.Drawing.Size(52, 20);
            this.XRes.TabIndex = 3;
            // 
            // YRes
            // 
            this.YRes.Location = new System.Drawing.Point(113, 71);
            this.YRes.Name = "YRes";
            this.YRes.Size = new System.Drawing.Size(52, 20);
            this.YRes.TabIndex = 5;
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(92, 74);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(14, 13);
            this.YLabel.TabIndex = 4;
            this.YLabel.Text = "Y";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(156, 142);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 6;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(75, 142);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // FullscreenList
            // 
            this.FullscreenList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FullscreenList.FormattingEnabled = true;
            this.FullscreenList.Location = new System.Drawing.Point(92, 42);
            this.FullscreenList.Name = "FullscreenList";
            this.FullscreenList.Size = new System.Drawing.Size(179, 21);
            this.FullscreenList.TabIndex = 8;
            this.FullscreenList.SelectedIndexChanged += new System.EventHandler(this.FullscreenList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Renderer";
            // 
            // RendererList
            // 
            this.RendererList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RendererList.FormattingEnabled = true;
            this.RendererList.Location = new System.Drawing.Point(66, 9);
            this.RendererList.Name = "RendererList";
            this.RendererList.Size = new System.Drawing.Size(205, 21);
            this.RendererList.TabIndex = 10;
            this.RendererList.SelectedIndexChanged += new System.EventHandler(this.RendererList_SelectedIndexChanged);
            // 
            // FSAA
            // 
            this.FSAA.AutoSize = true;
            this.FSAA.Location = new System.Drawing.Point(12, 109);
            this.FSAA.Name = "FSAA";
            this.FSAA.Size = new System.Drawing.Size(64, 13);
            this.FSAA.TabIndex = 11;
            this.FSAA.Text = "Anti Aliasing";
            // 
            // FSAAList
            // 
            this.FSAAList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FSAAList.FormattingEnabled = true;
            this.FSAAList.Location = new System.Drawing.Point(83, 106);
            this.FSAAList.Name = "FSAAList";
            this.FSAAList.Size = new System.Drawing.Size(67, 21);
            this.FSAAList.TabIndex = 12;
            this.FSAAList.SelectedIndexChanged += new System.EventHandler(this.FSAAList_SelectedIndexChanged);
            // 
            // VSync
            // 
            this.VSync.AutoSize = true;
            this.VSync.Location = new System.Drawing.Point(165, 110);
            this.VSync.Name = "VSync";
            this.VSync.Size = new System.Drawing.Size(57, 17);
            this.VSync.TabIndex = 13;
            this.VSync.Text = "VSync";
            this.VSync.UseVisualStyleBackColor = true;
            // 
            // VideoOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 180);
            this.Controls.Add(this.VSync);
            this.Controls.Add(this.FSAAList);
            this.Controls.Add(this.FSAA);
            this.Controls.Add(this.RendererList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FullscreenList);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.YRes);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XRes);
            this.Controls.Add(this.Xlabel);
            this.Controls.Add(this.Fullscreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "VideoOptionsDialog";
            this.Text = "VideoOptions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Fullscreen;
        private System.Windows.Forms.Label Xlabel;
        private System.Windows.Forms.TextBox XRes;
        private System.Windows.Forms.TextBox YRes;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ComboBox FullscreenList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox RendererList;
        private System.Windows.Forms.Label FSAA;
        private System.Windows.Forms.ComboBox FSAAList;
        private System.Windows.Forms.CheckBox VSync;
    }
}