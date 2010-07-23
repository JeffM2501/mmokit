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
            this.components = new System.ComponentModel.Container();
            this.HorizontalSplitter = new System.Windows.Forms.SplitContainer();
            this.VerticalSplitter = new System.Windows.Forms.SplitContainer();
            this.MainView = new System.Windows.Forms.PictureBox();
            this.LayerPannel = new System.Windows.Forms.Panel();
            this.LayerInfo = new System.Windows.Forms.Button();
            this.NewLayer = new System.Windows.Forms.Button();
            this.LayerDown = new System.Windows.Forms.Button();
            this.LayerUp = new System.Windows.Forms.Button();
            this.HideLayer = new System.Windows.Forms.Button();
            this.RemoveLayer = new System.Windows.Forms.Button();
            this.AddLayer = new System.Windows.Forms.Button();
            this.LayerList = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LayerListImages = new System.Windows.Forms.ImageList(this.components);
            this.DirectionPanel = new System.Windows.Forms.Panel();
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.RightButton = new System.Windows.Forms.Button();
            this.AnimPanel = new System.Windows.Forms.Panel();
            this.OffsetPanel = new System.Windows.Forms.Panel();
            this.OffsetLabel = new System.Windows.Forms.Label();
            this.XOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.YOffset = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ActionList = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.HorizontalSplitter.Panel1.SuspendLayout();
            this.HorizontalSplitter.Panel2.SuspendLayout();
            this.HorizontalSplitter.SuspendLayout();
            this.VerticalSplitter.Panel1.SuspendLayout();
            this.VerticalSplitter.Panel2.SuspendLayout();
            this.VerticalSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainView)).BeginInit();
            this.LayerPannel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.DirectionPanel.SuspendLayout();
            this.AnimPanel.SuspendLayout();
            this.OffsetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // HorizontalSplitter
            // 
            this.HorizontalSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HorizontalSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.HorizontalSplitter.Location = new System.Drawing.Point(3, 6);
            this.HorizontalSplitter.Name = "HorizontalSplitter";
            this.HorizontalSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // HorizontalSplitter.Panel1
            // 
            this.HorizontalSplitter.Panel1.Controls.Add(this.MainView);
            // 
            // HorizontalSplitter.Panel2
            // 
            this.HorizontalSplitter.Panel2.Controls.Add(this.OffsetPanel);
            this.HorizontalSplitter.Panel2.Controls.Add(this.DirectionPanel);
            this.HorizontalSplitter.Size = new System.Drawing.Size(699, 551);
            this.HorizontalSplitter.SplitterDistance = 424;
            this.HorizontalSplitter.TabIndex = 0;
            // 
            // VerticalSplitter
            // 
            this.VerticalSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.VerticalSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VerticalSplitter.Location = new System.Drawing.Point(12, 27);
            this.VerticalSplitter.Name = "VerticalSplitter";
            // 
            // VerticalSplitter.Panel1
            // 
            this.VerticalSplitter.Panel1.Controls.Add(this.HorizontalSplitter);
            // 
            // VerticalSplitter.Panel2
            // 
            this.VerticalSplitter.Panel2.Controls.Add(this.LayerPannel);
            this.VerticalSplitter.Panel2.Controls.Add(this.AnimPanel);
            this.VerticalSplitter.Size = new System.Drawing.Size(945, 564);
            this.VerticalSplitter.SplitterDistance = 709;
            this.VerticalSplitter.TabIndex = 0;
            // 
            // MainView
            // 
            this.MainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainView.Location = new System.Drawing.Point(-2, 3);
            this.MainView.Name = "MainView";
            this.MainView.Size = new System.Drawing.Size(694, 414);
            this.MainView.TabIndex = 0;
            this.MainView.TabStop = false;
            this.MainView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainView_MouseMove);
            this.MainView.Resize += new System.EventHandler(this.MainView_Resize);
            this.MainView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainView_MouseDown);
            this.MainView.Paint += new System.Windows.Forms.PaintEventHandler(this.MainView_Paint);
            this.MainView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainView_MouseUp);
            // 
            // LayerPannel
            // 
            this.LayerPannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
            this.LayerPannel.Size = new System.Drawing.Size(227, 220);
            this.LayerPannel.TabIndex = 0;
            // 
            // LayerInfo
            // 
            this.LayerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerInfo.Location = new System.Drawing.Point(140, 194);
            this.LayerInfo.Name = "LayerInfo";
            this.LayerInfo.Size = new System.Drawing.Size(22, 23);
            this.LayerInfo.TabIndex = 7;
            this.LayerInfo.Text = "i";
            this.LayerInfo.UseVisualStyleBackColor = true;
            // 
            // NewLayer
            // 
            this.NewLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewLayer.Location = new System.Drawing.Point(59, 194);
            this.NewLayer.Name = "NewLayer";
            this.NewLayer.Size = new System.Drawing.Size(22, 23);
            this.NewLayer.TabIndex = 6;
            this.NewLayer.Text = "N";
            this.NewLayer.UseVisualStyleBackColor = true;
            // 
            // LayerDown
            // 
            this.LayerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerDown.Location = new System.Drawing.Point(196, 165);
            this.LayerDown.Name = "LayerDown";
            this.LayerDown.Size = new System.Drawing.Size(22, 23);
            this.LayerDown.TabIndex = 5;
            this.LayerDown.Text = "ˇ";
            this.LayerDown.UseVisualStyleBackColor = true;
            this.LayerDown.Click += new System.EventHandler(this.LayerDown_Click);
            // 
            // LayerUp
            // 
            this.LayerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LayerUp.Location = new System.Drawing.Point(196, 3);
            this.LayerUp.Name = "LayerUp";
            this.LayerUp.Size = new System.Drawing.Size(22, 23);
            this.LayerUp.TabIndex = 4;
            this.LayerUp.Text = "ˆ";
            this.LayerUp.UseVisualStyleBackColor = true;
            this.LayerUp.Click += new System.EventHandler(this.LayerUp_Click);
            // 
            // HideLayer
            // 
            this.HideLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HideLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideLayer.Location = new System.Drawing.Point(168, 194);
            this.HideLayer.Name = "HideLayer";
            this.HideLayer.Size = new System.Drawing.Size(22, 23);
            this.HideLayer.TabIndex = 3;
            this.HideLayer.Text = "S";
            this.HideLayer.UseVisualStyleBackColor = true;
            // 
            // RemoveLayer
            // 
            this.RemoveLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveLayer.Location = new System.Drawing.Point(31, 194);
            this.RemoveLayer.Name = "RemoveLayer";
            this.RemoveLayer.Size = new System.Drawing.Size(22, 23);
            this.RemoveLayer.TabIndex = 2;
            this.RemoveLayer.Text = "-";
            this.RemoveLayer.UseVisualStyleBackColor = true;
            this.RemoveLayer.Click += new System.EventHandler(this.RemoveLayer_Click);
            // 
            // AddLayer
            // 
            this.AddLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddLayer.Location = new System.Drawing.Point(3, 194);
            this.AddLayer.Name = "AddLayer";
            this.AddLayer.Size = new System.Drawing.Size(22, 23);
            this.AddLayer.TabIndex = 1;
            this.AddLayer.Text = "+";
            this.AddLayer.UseVisualStyleBackColor = true;
            this.AddLayer.Click += new System.EventHandler(this.AddLayer_Click);
            // 
            // LayerList
            // 
            this.LayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerList.FullRowSelect = true;
            this.LayerList.LargeImageList = this.LayerListImages;
            this.LayerList.Location = new System.Drawing.Point(3, 3);
            this.LayerList.MultiSelect = false;
            this.LayerList.Name = "LayerList";
            this.LayerList.Size = new System.Drawing.Size(187, 185);
            this.LayerList.SmallImageList = this.LayerListImages;
            this.LayerList.StateImageList = this.LayerListImages;
            this.LayerList.TabIndex = 0;
            this.LayerList.UseCompatibleStateImageBehavior = false;
            this.LayerList.View = System.Windows.Forms.View.List;
            this.LayerList.SelectedIndexChanged += new System.EventHandler(this.LayerList_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(963, 24);
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
            // LayerListImages
            // 
            this.LayerListImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.LayerListImages.ImageSize = new System.Drawing.Size(32, 32);
            this.LayerListImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DirectionPanel
            // 
            this.DirectionPanel.Controls.Add(this.RightButton);
            this.DirectionPanel.Controls.Add(this.LeftButton);
            this.DirectionPanel.Controls.Add(this.DownButton);
            this.DirectionPanel.Controls.Add(this.UpButton);
            this.DirectionPanel.Location = new System.Drawing.Point(3, 3);
            this.DirectionPanel.Name = "DirectionPanel";
            this.DirectionPanel.Size = new System.Drawing.Size(165, 100);
            this.DirectionPanel.TabIndex = 0;
            // 
            // UpButton
            // 
            this.UpButton.Location = new System.Drawing.Point(54, 9);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(53, 23);
            this.UpButton.TabIndex = 0;
            this.UpButton.Text = "Up";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Location = new System.Drawing.Point(54, 67);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(53, 23);
            this.DownButton.TabIndex = 1;
            this.DownButton.Text = "Down";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // LeftButton
            // 
            this.LeftButton.Location = new System.Drawing.Point(3, 38);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(53, 23);
            this.LeftButton.TabIndex = 2;
            this.LeftButton.Text = "Left";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // RightButton
            // 
            this.RightButton.Location = new System.Drawing.Point(104, 38);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(53, 23);
            this.RightButton.TabIndex = 3;
            this.RightButton.Text = "Right";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // AnimPanel
            // 
            this.AnimPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimPanel.Controls.Add(this.button1);
            this.AnimPanel.Controls.Add(this.button2);
            this.AnimPanel.Controls.Add(this.ActionList);
            this.AnimPanel.Controls.Add(this.label3);
            this.AnimPanel.Location = new System.Drawing.Point(3, 229);
            this.AnimPanel.Name = "AnimPanel";
            this.AnimPanel.Size = new System.Drawing.Size(228, 328);
            this.AnimPanel.TabIndex = 1;
            // 
            // OffsetPanel
            // 
            this.OffsetPanel.Controls.Add(this.label2);
            this.OffsetPanel.Controls.Add(this.YOffset);
            this.OffsetPanel.Controls.Add(this.label1);
            this.OffsetPanel.Controls.Add(this.XOffset);
            this.OffsetPanel.Controls.Add(this.OffsetLabel);
            this.OffsetPanel.Location = new System.Drawing.Point(174, 3);
            this.OffsetPanel.Name = "OffsetPanel";
            this.OffsetPanel.Size = new System.Drawing.Size(518, 114);
            this.OffsetPanel.TabIndex = 2;
            // 
            // OffsetLabel
            // 
            this.OffsetLabel.AutoSize = true;
            this.OffsetLabel.Location = new System.Drawing.Point(3, 9);
            this.OffsetLabel.Name = "OffsetLabel";
            this.OffsetLabel.Size = new System.Drawing.Size(35, 13);
            this.OffsetLabel.TabIndex = 0;
            this.OffsetLabel.Text = "Offset";
            // 
            // XOffset
            // 
            this.XOffset.Location = new System.Drawing.Point(32, 25);
            this.XOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.XOffset.Name = "XOffset";
            this.XOffset.Size = new System.Drawing.Size(51, 20);
            this.XOffset.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Y";
            // 
            // YOffset
            // 
            this.YOffset.Location = new System.Drawing.Point(32, 51);
            this.YOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.YOffset.Name = "YOffset";
            this.YOffset.Size = new System.Drawing.Size(51, 20);
            this.YOffset.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Action";
            // 
            // ActionList
            // 
            this.ActionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActionList.FormattingEnabled = true;
            this.ActionList.Location = new System.Drawing.Point(6, 16);
            this.ActionList.Name = "ActionList";
            this.ActionList.Size = new System.Drawing.Size(156, 21);
            this.ActionList.TabIndex = 1;
            this.ActionList.SelectedIndexChanged += new System.EventHandler(this.ActionList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(196, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(168, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 594);
            this.Controls.Add(this.VerticalSplitter);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "ManaSource Sprite Tool";
            this.HorizontalSplitter.Panel1.ResumeLayout(false);
            this.HorizontalSplitter.Panel2.ResumeLayout(false);
            this.HorizontalSplitter.ResumeLayout(false);
            this.VerticalSplitter.Panel1.ResumeLayout(false);
            this.VerticalSplitter.Panel2.ResumeLayout(false);
            this.VerticalSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainView)).EndInit();
            this.LayerPannel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.DirectionPanel.ResumeLayout(false);
            this.AnimPanel.ResumeLayout(false);
            this.AnimPanel.PerformLayout();
            this.OffsetPanel.ResumeLayout(false);
            this.OffsetPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer HorizontalSplitter;
        private System.Windows.Forms.SplitContainer VerticalSplitter;
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
        private System.Windows.Forms.ImageList LayerListImages;
        private System.Windows.Forms.Panel DirectionPanel;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Panel OffsetPanel;
        private System.Windows.Forms.Panel AnimPanel;
        private System.Windows.Forms.Label OffsetLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown YOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown XOffset;
        private System.Windows.Forms.ComboBox ActionList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

