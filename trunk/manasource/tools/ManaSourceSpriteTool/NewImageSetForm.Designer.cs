namespace ManaSourceSpriteTool
{
    partial class NewImageSetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewImageSetForm));
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ImagePath = new System.Windows.Forms.TextBox();
            this.ImageBrowse = new System.Windows.Forms.Button();
            this.XMLPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.ImageSetName = new System.Windows.Forms.TextBox();
            this.label122 = new System.Windows.Forms.Label();
            this.Recolor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GridX = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.GridY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridY)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(351, 183);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(270, 183);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Image File";
            // 
            // ImagePath
            // 
            this.ImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagePath.Location = new System.Drawing.Point(13, 64);
            this.ImagePath.Name = "ImagePath";
            this.ImagePath.Size = new System.Drawing.Size(377, 20);
            this.ImagePath.TabIndex = 3;
            this.ImagePath.TextChanged += new System.EventHandler(this.ImagePath_TextChanged);
            // 
            // ImageBrowse
            // 
            this.ImageBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageBrowse.Location = new System.Drawing.Point(398, 64);
            this.ImageBrowse.Name = "ImageBrowse";
            this.ImageBrowse.Size = new System.Drawing.Size(28, 23);
            this.ImageBrowse.TabIndex = 4;
            this.ImageBrowse.Text = "...";
            this.ImageBrowse.UseVisualStyleBackColor = true;
            this.ImageBrowse.Click += new System.EventHandler(this.ImageBrowse_Click);
            // 
            // XMLPath
            // 
            this.XMLPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.XMLPath.Location = new System.Drawing.Point(11, 103);
            this.XMLPath.Name = "XMLPath";
            this.XMLPath.Size = new System.Drawing.Size(413, 20);
            this.XMLPath.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "XML Path";
            // 
            // ImageBox
            // 
            this.ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject("ImageBox.ErrorImage")));
            this.ImageBox.Location = new System.Drawing.Point(12, 212);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(416, 265);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImageBox.TabIndex = 7;
            this.ImageBox.TabStop = false;
            // 
            // ImageSetName
            // 
            this.ImageSetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageSetName.Location = new System.Drawing.Point(12, 24);
            this.ImageSetName.Name = "ImageSetName";
            this.ImageSetName.Size = new System.Drawing.Size(377, 20);
            this.ImageSetName.TabIndex = 9;
            // 
            // label122
            // 
            this.label122.AutoSize = true;
            this.label122.Location = new System.Drawing.Point(12, 9);
            this.label122.Name = "label122";
            this.label122.Size = new System.Drawing.Size(83, 13);
            this.label122.TabIndex = 8;
            this.label122.Text = "ImageSet Name";
            // 
            // Recolor
            // 
            this.Recolor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Recolor.Location = new System.Drawing.Point(13, 142);
            this.Recolor.Name = "Recolor";
            this.Recolor.Size = new System.Drawing.Size(413, 20);
            this.Recolor.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Recolor Info";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Grid Width";
            // 
            // GridX
            // 
            this.GridX.Location = new System.Drawing.Point(15, 183);
            this.GridX.Name = "GridX";
            this.GridX.Size = new System.Drawing.Size(62, 20);
            this.GridX.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Height";
            // 
            // GridY
            // 
            this.GridY.Location = new System.Drawing.Point(83, 183);
            this.GridY.Name = "GridY";
            this.GridY.Size = new System.Drawing.Size(62, 20);
            this.GridY.TabIndex = 13;
            // 
            // NewImageSetForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(440, 489);
            this.Controls.Add(this.GridY);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GridX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Recolor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ImageSetName);
            this.Controls.Add(this.label122);
            this.Controls.Add(this.ImageBox);
            this.Controls.Add(this.XMLPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ImageBrowse);
            this.Controls.Add(this.ImagePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewImageSetForm";
            this.Text = "New ImageSet";
            this.Load += new System.EventHandler(this.NewImageSetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ImagePath;
        private System.Windows.Forms.Button ImageBrowse;
        private System.Windows.Forms.TextBox XMLPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.TextBox ImageSetName;
        private System.Windows.Forms.Label label122;
        private System.Windows.Forms.TextBox Recolor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown GridX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown GridY;
    }
}