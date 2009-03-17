namespace _3dSpeeders
{
    partial class AudioOptionsDialog
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
            this.EnableSound = new System.Windows.Forms.CheckBox();
            this.VolumeBar = new System.Windows.Forms.TrackBar();
            this.VolumeLabel = new System.Windows.Forms.Label();
            this.VolumeVal = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // EnableSound
            // 
            this.EnableSound.AutoSize = true;
            this.EnableSound.Location = new System.Drawing.Point(2, 12);
            this.EnableSound.Name = "EnableSound";
            this.EnableSound.Size = new System.Drawing.Size(93, 17);
            this.EnableSound.TabIndex = 0;
            this.EnableSound.Text = "Enable Sound";
            this.EnableSound.UseVisualStyleBackColor = true;
            this.EnableSound.CheckedChanged += new System.EventHandler(this.EnableSound_CheckedChanged);
            // 
            // VolumeBar
            // 
            this.VolumeBar.Location = new System.Drawing.Point(-4, 56);
            this.VolumeBar.Name = "VolumeBar";
            this.VolumeBar.Size = new System.Drawing.Size(276, 45);
            this.VolumeBar.TabIndex = 1;
            this.VolumeBar.Scroll += new System.EventHandler(this.VolumeBar_Scroll);
            // 
            // VolumeLabel
            // 
            this.VolumeLabel.AutoSize = true;
            this.VolumeLabel.Location = new System.Drawing.Point(2, 36);
            this.VolumeLabel.Name = "VolumeLabel";
            this.VolumeLabel.Size = new System.Drawing.Size(42, 13);
            this.VolumeLabel.TabIndex = 2;
            this.VolumeLabel.Text = "Volume";
            // 
            // VolumeVal
            // 
            this.VolumeVal.AutoSize = true;
            this.VolumeVal.Location = new System.Drawing.Point(237, 36);
            this.VolumeVal.Name = "VolumeVal";
            this.VolumeVal.Size = new System.Drawing.Size(13, 13);
            this.VolumeVal.TabIndex = 3;
            this.VolumeVal.Text = "0";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(201, 108);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(117, 107);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // AudioOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 142);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.VolumeVal);
            this.Controls.Add(this.VolumeLabel);
            this.Controls.Add(this.VolumeBar);
            this.Controls.Add(this.EnableSound);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AudioOptionsDialog";
            this.Text = "Audio Options";
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox EnableSound;
        private System.Windows.Forms.TrackBar VolumeBar;
        private System.Windows.Forms.Label VolumeLabel;
        private System.Windows.Forms.Label VolumeVal;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}