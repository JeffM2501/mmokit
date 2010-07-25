namespace ManaSourceSpriteTool
{
    partial class NewLayerForm
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
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.XMLFileItem = new System.Windows.Forms.TextBox();
            this.BrowseXML = new System.Windows.Forms.Button();
            this.BrowseImage = new System.Windows.Forms.Button();
            this.ImageLocationItem = new System.Windows.Forms.TextBox();
            this.ImageSetFileLabel = new System.Windows.Forms.Label();
            this.NameItem = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DefaultActionItem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ImagePathItem = new System.Windows.Forms.TextBox();
            this.ImagesetPathLabel = new System.Windows.Forms.Label();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.GridXLabel = new System.Windows.Forms.Label();
            this.GridX = new System.Windows.Forms.NumericUpDown();
            this.GridY = new System.Windows.Forms.NumericUpDown();
            this.GridYLabel = new System.Windows.Forms.Label();
            this.ImageSetNameItem = new System.Windows.Forms.TextBox();
            this.ImageSetNameLabel = new System.Windows.Forms.Label();
            this.ImageSetGroup = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridY)).BeginInit();
            this.ImageSetGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(348, 385);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(267, 385);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "XML Location";
            // 
            // XMLFileItem
            // 
            this.XMLFileItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.XMLFileItem.Location = new System.Drawing.Point(15, 64);
            this.XMLFileItem.Name = "XMLFileItem";
            this.XMLFileItem.Size = new System.Drawing.Size(377, 20);
            this.XMLFileItem.TabIndex = 3;
            // 
            // BrowseXML
            // 
            this.BrowseXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseXML.Location = new System.Drawing.Point(398, 62);
            this.BrowseXML.Name = "BrowseXML";
            this.BrowseXML.Size = new System.Drawing.Size(26, 23);
            this.BrowseXML.TabIndex = 4;
            this.BrowseXML.Text = "...";
            this.BrowseXML.UseVisualStyleBackColor = true;
            this.BrowseXML.Click += new System.EventHandler(this.BrowseXML_Click);
            // 
            // BrowseImage
            // 
            this.BrowseImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseImage.Location = new System.Drawing.Point(378, 69);
            this.BrowseImage.Name = "BrowseImage";
            this.BrowseImage.Size = new System.Drawing.Size(26, 23);
            this.BrowseImage.TabIndex = 7;
            this.BrowseImage.Text = "...";
            this.BrowseImage.UseVisualStyleBackColor = true;
            this.BrowseImage.Click += new System.EventHandler(this.BrowseImage_Click);
            // 
            // ImageLocationItem
            // 
            this.ImageLocationItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageLocationItem.Location = new System.Drawing.Point(9, 71);
            this.ImageLocationItem.Name = "ImageLocationItem";
            this.ImageLocationItem.Size = new System.Drawing.Size(363, 20);
            this.ImageLocationItem.TabIndex = 6;
            this.ImageLocationItem.TextChanged += new System.EventHandler(this.ImageLocationItem_TextChanged);
            // 
            // ImageSetFileLabel
            // 
            this.ImageSetFileLabel.AutoSize = true;
            this.ImageSetFileLabel.Location = new System.Drawing.Point(6, 55);
            this.ImageSetFileLabel.Name = "ImageSetFileLabel";
            this.ImageSetFileLabel.Size = new System.Drawing.Size(80, 13);
            this.ImageSetFileLabel.TabIndex = 5;
            this.ImageSetFileLabel.Text = "Image Location";
            // 
            // NameItem
            // 
            this.NameItem.Location = new System.Drawing.Point(15, 25);
            this.NameItem.Name = "NameItem";
            this.NameItem.Size = new System.Drawing.Size(194, 20);
            this.NameItem.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name";
            // 
            // DefaultActionItem
            // 
            this.DefaultActionItem.Location = new System.Drawing.Point(221, 25);
            this.DefaultActionItem.Name = "DefaultActionItem";
            this.DefaultActionItem.Size = new System.Drawing.Size(192, 20);
            this.DefaultActionItem.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(218, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Default Action";
            // 
            // ImagePathItem
            // 
            this.ImagePathItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagePathItem.Location = new System.Drawing.Point(9, 110);
            this.ImagePathItem.Name = "ImagePathItem";
            this.ImagePathItem.Size = new System.Drawing.Size(363, 20);
            this.ImagePathItem.TabIndex = 13;
            // 
            // ImagesetPathLabel
            // 
            this.ImagesetPathLabel.AutoSize = true;
            this.ImagesetPathLabel.Location = new System.Drawing.Point(6, 94);
            this.ImagesetPathLabel.Name = "ImagesetPathLabel";
            this.ImagesetPathLabel.Size = new System.Drawing.Size(54, 13);
            this.ImagesetPathLabel.TabIndex = 12;
            this.ImagesetPathLabel.Text = "XML Path";
            // 
            // ImageBox
            // 
            this.ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageBox.Location = new System.Drawing.Point(15, 273);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(408, 105);
            this.ImageBox.TabIndex = 14;
            this.ImageBox.TabStop = false;
            // 
            // GridXLabel
            // 
            this.GridXLabel.AutoSize = true;
            this.GridXLabel.Location = new System.Drawing.Point(6, 133);
            this.GridXLabel.Name = "GridXLabel";
            this.GridXLabel.Size = new System.Drawing.Size(36, 13);
            this.GridXLabel.TabIndex = 15;
            this.GridXLabel.Text = "Grid X";
            // 
            // GridX
            // 
            this.GridX.Location = new System.Drawing.Point(9, 149);
            this.GridX.Name = "GridX";
            this.GridX.Size = new System.Drawing.Size(46, 20);
            this.GridX.TabIndex = 16;
            // 
            // GridY
            // 
            this.GridY.Location = new System.Drawing.Point(61, 149);
            this.GridY.Name = "GridY";
            this.GridY.Size = new System.Drawing.Size(46, 20);
            this.GridY.TabIndex = 18;
            // 
            // GridYLabel
            // 
            this.GridYLabel.AutoSize = true;
            this.GridYLabel.Location = new System.Drawing.Point(58, 133);
            this.GridYLabel.Name = "GridYLabel";
            this.GridYLabel.Size = new System.Drawing.Size(14, 13);
            this.GridYLabel.TabIndex = 17;
            this.GridYLabel.Text = "Y";
            // 
            // ImageSetNameItem
            // 
            this.ImageSetNameItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageSetNameItem.Location = new System.Drawing.Point(9, 32);
            this.ImageSetNameItem.Name = "ImageSetNameItem";
            this.ImageSetNameItem.Size = new System.Drawing.Size(363, 20);
            this.ImageSetNameItem.TabIndex = 20;
            // 
            // ImageSetNameLabel
            // 
            this.ImageSetNameLabel.AutoSize = true;
            this.ImageSetNameLabel.Location = new System.Drawing.Point(6, 16);
            this.ImageSetNameLabel.Name = "ImageSetNameLabel";
            this.ImageSetNameLabel.Size = new System.Drawing.Size(35, 13);
            this.ImageSetNameLabel.TabIndex = 19;
            this.ImageSetNameLabel.Text = "Name";
            // 
            // ImageSetGroup
            // 
            this.ImageSetGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageSetGroup.Controls.Add(this.ImageSetNameLabel);
            this.ImageSetGroup.Controls.Add(this.GridY);
            this.ImageSetGroup.Controls.Add(this.ImageSetNameItem);
            this.ImageSetGroup.Controls.Add(this.GridYLabel);
            this.ImageSetGroup.Controls.Add(this.ImageSetFileLabel);
            this.ImageSetGroup.Controls.Add(this.GridX);
            this.ImageSetGroup.Controls.Add(this.ImageLocationItem);
            this.ImageSetGroup.Controls.Add(this.GridXLabel);
            this.ImageSetGroup.Controls.Add(this.BrowseImage);
            this.ImageSetGroup.Controls.Add(this.ImagesetPathLabel);
            this.ImageSetGroup.Controls.Add(this.ImagePathItem);
            this.ImageSetGroup.Location = new System.Drawing.Point(15, 90);
            this.ImageSetGroup.Name = "ImageSetGroup";
            this.ImageSetGroup.Size = new System.Drawing.Size(410, 177);
            this.ImageSetGroup.TabIndex = 21;
            this.ImageSetGroup.TabStop = false;
            this.ImageSetGroup.Text = "Inital Image Set";
            // 
            // NewLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 420);
            this.Controls.Add(this.ImageSetGroup);
            this.Controls.Add(this.ImageBox);
            this.Controls.Add(this.DefaultActionItem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NameItem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BrowseXML);
            this.Controls.Add(this.XMLFileItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewLayerForm";
            this.Text = "Layer Info";
            this.Load += new System.EventHandler(this.NewLayerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridY)).EndInit();
            this.ImageSetGroup.ResumeLayout(false);
            this.ImageSetGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox XMLFileItem;
        private System.Windows.Forms.Button BrowseXML;
        private System.Windows.Forms.Button BrowseImage;
        private System.Windows.Forms.TextBox ImageLocationItem;
        private System.Windows.Forms.Label ImageSetFileLabel;
        private System.Windows.Forms.TextBox NameItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DefaultActionItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ImagePathItem;
        private System.Windows.Forms.Label ImagesetPathLabel;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.Label GridXLabel;
        private System.Windows.Forms.NumericUpDown GridX;
        private System.Windows.Forms.NumericUpDown GridY;
        private System.Windows.Forms.Label GridYLabel;
        private System.Windows.Forms.TextBox ImageSetNameItem;
        private System.Windows.Forms.Label ImageSetNameLabel;
        private System.Windows.Forms.GroupBox ImageSetGroup;
    }
}