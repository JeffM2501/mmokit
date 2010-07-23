namespace ManaSourceSpriteTool
{
    partial class MainForm
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
            this.MainSplitter = new System.Windows.Forms.SplitContainer();
            this.ViewSplitter = new System.Windows.Forms.SplitContainer();
            this.MainView = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LayerPannel = new System.Windows.Forms.Panel();
            this.LayerList = new System.Windows.Forms.ListView();
            this.AddLayer = new System.Windows.Forms.Button();
            this.RemoveLayer = new System.Windows.Forms.Button();
            this.HideLayer = new System.Windows.Forms.Button();
            this.LayerUp = new System.Windows.Forms.Button();
            this.LayerDown = new System.Windows.Forms.Button();
            this.NewLayer = new System.Windows.Forms.Button();
            this.LayerInfo = new System.Windows.Forms.Button();
            this.MainSplitter.Panel1.SuspendLayout();
            this.MainSplitter.SuspendLayout();
            this.ViewSplitter.Panel1.SuspendLayout();
            this.ViewSplitter.Panel2.SuspendLayout();
            this.ViewSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.LayerPannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainSplitter
            // 
            this.MainSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSplitter.Location = new System.Drawing.Point(0, 27);
            this.MainSplitter.Name = "MainSplitter";
            this.MainSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitter.Panel1
            // 
            this.MainSplitter.Panel1.Controls.Add(this.ViewSplitter);
            this.MainSplitter.Size = new System.Drawing.Size(946, 519);
            this.MainSplitter.SplitterDistance = 342;
            this.MainSplitter.TabIndex = 0;
            // 
            // ViewSplitter
            // 
            this.ViewSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ViewSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewSplitter.Location = new System.Drawing.Point(0, 0);
            this.ViewSplitter.Name = "ViewSplitter";
            // 
            // ViewSplitter.Panel1
            // 
            this.ViewSplitter.Panel1.Controls.Add(this.MainView);
            // 
            // ViewSplitter.Panel2
            // 
            this.ViewSplitter.Panel2.Controls.Add(this.LayerPannel);
            this.ViewSplitter.Size = new System.Drawing.Size(946, 342);
            this.ViewSplitter.SplitterDistance = 710;
            this.ViewSplitter.TabIndex = 0;
            // 
            // MainView
            // 
            this.MainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainView.Location = new System.Drawing.Point(3, 3);
            this.MainView.Name = "MainView";
            this.MainView.Size = new System.Drawing.Size(700, 332);
            this.MainView.TabIndex = 0;
            this.MainView.TabStop = false;
            this.MainView.Resize += new System.EventHandler(this.MainView_Resize);
            this.MainView.Paint += new System.Windows.Forms.PaintEventHandler(this.MainView_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(946, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // LayerPannel
            // 
            this.LayerPannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerPannel.Controls.Add(this.LayerInfo);
            this.LayerPannel.Controls.Add(this.NewLayer);
            this.LayerPannel.Controls.Add(this.LayerDown);
            this.LayerPannel.Controls.Add(this.LayerUp);
            this.LayerPannel.Controls.Add(this.HideLayer);
            this.LayerPannel.Controls.Add(this.RemoveLayer);
            this.LayerPannel.Controls.Add(this.AddLayer);
            this.LayerPannel.Controls.Add(this.LayerList);
            this.LayerPannel.Location = new System.Drawing.Point(3, 3);
            this.LayerPannel.Name = "LayerPannel";
            this.LayerPannel.Size = new System.Drawing.Size(215, 303);
            this.LayerPannel.TabIndex = 0;
            // 
            // LayerList
            // 
            this.LayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerList.Location = new System.Drawing.Point(3, 3);
            this.LayerList.Name = "LayerList";
            this.LayerList.Size = new System.Drawing.Size(175, 268);
            this.LayerList.TabIndex = 0;
            this.LayerList.UseCompatibleStateImageBehavior = false;
            this.LayerList.View = System.Windows.Forms.View.List;
            // 
            // AddLayer
            // 
            this.AddLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddLayer.Location = new System.Drawing.Point(3, 277);
            this.AddLayer.Name = "AddLayer";
            this.AddLayer.Size = new System.Drawing.Size(22, 23);
            this.AddLayer.TabIndex = 1;
            this.AddLayer.Text = "+";
            this.AddLayer.UseVisualStyleBackColor = true;
            this.AddLayer.Click += new System.EventHandler(this.AddLayer_Click);
            // 
            // RemoveLayer
            // 
            this.RemoveLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveLayer.Location = new System.Drawing.Point(31, 277);
            this.RemoveLayer.Name = "RemoveLayer";
            this.RemoveLayer.Size = new System.Drawing.Size(22, 23);
            this.RemoveLayer.TabIndex = 2;
            this.RemoveLayer.Text = "-";
            this.RemoveLayer.UseVisualStyleBackColor = true;
            // 
            // HideLayer
            // 
            this.HideLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HideLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideLayer.Location = new System.Drawing.Point(156, 277);
            this.HideLayer.Name = "HideLayer";
            this.HideLayer.Size = new System.Drawing.Size(22, 23);
            this.HideLayer.TabIndex = 3;
            this.HideLayer.Text = "S";
            this.HideLayer.UseVisualStyleBackColor = true;
            // 
            // LayerUp
            // 
            this.LayerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerUp.Location = new System.Drawing.Point(184, 3);
            this.LayerUp.Name = "LayerUp";
            this.LayerUp.Size = new System.Drawing.Size(22, 23);
            this.LayerUp.TabIndex = 4;
            this.LayerUp.Text = "ˆ";
            this.LayerUp.UseVisualStyleBackColor = true;
            // 
            // LayerDown
            // 
            this.LayerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerDown.Location = new System.Drawing.Point(184, 248);
            this.LayerDown.Name = "LayerDown";
            this.LayerDown.Size = new System.Drawing.Size(22, 23);
            this.LayerDown.TabIndex = 5;
            this.LayerDown.Text = "ˇ";
            this.LayerDown.UseVisualStyleBackColor = true;
            // 
            // NewLayer
            // 
            this.NewLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewLayer.Location = new System.Drawing.Point(59, 277);
            this.NewLayer.Name = "NewLayer";
            this.NewLayer.Size = new System.Drawing.Size(22, 23);
            this.NewLayer.TabIndex = 6;
            this.NewLayer.Text = "N";
            this.NewLayer.UseVisualStyleBackColor = true;
            // 
            // LayerInfo
            // 
            this.LayerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerInfo.Location = new System.Drawing.Point(128, 277);
            this.LayerInfo.Name = "LayerInfo";
            this.LayerInfo.Size = new System.Drawing.Size(22, 23);
            this.LayerInfo.TabIndex = 7;
            this.LayerInfo.Text = "i";
            this.LayerInfo.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 547);
            this.Controls.Add(this.MainSplitter);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "ManaSource Sprite Tool";
            this.MainSplitter.Panel1.ResumeLayout(false);
            this.MainSplitter.ResumeLayout(false);
            this.ViewSplitter.Panel1.ResumeLayout(false);
            this.ViewSplitter.Panel2.ResumeLayout(false);
            this.ViewSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.LayerPannel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer MainSplitter;
        private System.Windows.Forms.SplitContainer ViewSplitter;
        private System.Windows.Forms.PictureBox MainView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Panel LayerPannel;
        private System.Windows.Forms.ListView LayerList;
        private System.Windows.Forms.Button HideLayer;
        private System.Windows.Forms.Button RemoveLayer;
        private System.Windows.Forms.Button AddLayer;
        private System.Windows.Forms.Button LayerDown;
        private System.Windows.Forms.Button LayerUp;
        private System.Windows.Forms.Button NewLayer;
        private System.Windows.Forms.Button LayerInfo;
    }
}

