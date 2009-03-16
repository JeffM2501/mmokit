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
            this.SuspendLayout();
            // 
            // Fullscreen
            // 
            this.Fullscreen.AutoSize = true;
            this.Fullscreen.Location = new System.Drawing.Point(12, 12);
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
            this.Xlabel.Location = new System.Drawing.Point(13, 36);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Size = new System.Drawing.Size(14, 13);
            this.Xlabel.TabIndex = 2;
            this.Xlabel.Text = "X";
            // 
            // XRes
            // 
            this.XRes.Location = new System.Drawing.Point(34, 33);
            this.XRes.Name = "XRes";
            this.XRes.Size = new System.Drawing.Size(52, 20);
            this.XRes.TabIndex = 3;
            // 
            // YRes
            // 
            this.YRes.Location = new System.Drawing.Point(114, 33);
            this.YRes.Name = "YRes";
            this.YRes.Size = new System.Drawing.Size(52, 20);
            this.YRes.TabIndex = 5;
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(93, 36);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(14, 13);
            this.YLabel.TabIndex = 4;
            this.YLabel.Text = "Y";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(96, 59);
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
            this.Cancel.Location = new System.Drawing.Point(15, 59);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // VideoOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 95);
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
    }
}