using OpenTK;

namespace modeler
{
    partial class ModelerDialog
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
            this.glControl1 = new OpenTK.GLControl();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swapYZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Sidebar = new System.Windows.Forms.Panel();
            this.RemoveSkin = new System.Windows.Forms.Button();
            this.NewSkin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SkinView = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MaterialGroup = new System.Windows.Forms.GroupBox();
            this.MainMenu.SuspendLayout();
            this.Sidebar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 27);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(614, 463);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseWheel);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            this.glControl1.MouseHover += new System.EventHandler(this.glControl1_MouseHover);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(798, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "MainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showNormalsToolStripMenuItem,
            this.headlightToolStripMenuItem,
            this.backgroundColorToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showNormalsToolStripMenuItem
            // 
            this.showNormalsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.normalsToolStripMenuItem,
            this.wireframeToolStripMenuItem});
            this.showNormalsToolStripMenuItem.Name = "showNormalsToolStripMenuItem";
            this.showNormalsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showNormalsToolStripMenuItem.Text = "Show";
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.gridToolStripMenuItem.Text = "Grid";
            this.gridToolStripMenuItem.Click += new System.EventHandler(this.gridToolStripMenuItem_Click);
            // 
            // normalsToolStripMenuItem
            // 
            this.normalsToolStripMenuItem.Name = "normalsToolStripMenuItem";
            this.normalsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.normalsToolStripMenuItem.Text = "Normals";
            this.normalsToolStripMenuItem.Click += new System.EventHandler(this.normalsToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.wireframeToolStripMenuItem.Text = "Wireframe Overlay";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // headlightToolStripMenuItem
            // 
            this.headlightToolStripMenuItem.Name = "headlightToolStripMenuItem";
            this.headlightToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.headlightToolStripMenuItem.Text = "Headlight";
            this.headlightToolStripMenuItem.Click += new System.EventHandler(this.headlightToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.backgroundColorToolStripMenuItem.Text = "Background Color";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.swapYZToolStripMenuItem,
            this.scaleToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // swapYZToolStripMenuItem
            // 
            this.swapYZToolStripMenuItem.Name = "swapYZToolStripMenuItem";
            this.swapYZToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.swapYZToolStripMenuItem.Text = "Swap YZ";
            this.swapYZToolStripMenuItem.Click += new System.EventHandler(this.swapYZToolStripMenuItem_Click);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.scaleToolStripMenuItem.Text = "Scale";
            this.scaleToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // Sidebar
            // 
            this.Sidebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Sidebar.Controls.Add(this.MaterialGroup);
            this.Sidebar.Controls.Add(this.groupBox1);
            this.Sidebar.Controls.Add(this.label1);
            this.Sidebar.Controls.Add(this.SkinView);
            this.Sidebar.Location = new System.Drawing.Point(632, 27);
            this.Sidebar.Name = "Sidebar";
            this.Sidebar.Size = new System.Drawing.Size(166, 475);
            this.Sidebar.TabIndex = 2;
            // 
            // RemoveSkin
            // 
            this.RemoveSkin.Location = new System.Drawing.Point(71, 19);
            this.RemoveSkin.Name = "RemoveSkin";
            this.RemoveSkin.Size = new System.Drawing.Size(59, 23);
            this.RemoveSkin.TabIndex = 3;
            this.RemoveSkin.Text = "Remove";
            this.RemoveSkin.UseVisualStyleBackColor = true;
            this.RemoveSkin.Click += new System.EventHandler(this.RemoveSkin_Click);
            // 
            // NewSkin
            // 
            this.NewSkin.Location = new System.Drawing.Point(6, 19);
            this.NewSkin.Name = "NewSkin";
            this.NewSkin.Size = new System.Drawing.Size(59, 23);
            this.NewSkin.TabIndex = 2;
            this.NewSkin.Text = "New";
            this.NewSkin.UseVisualStyleBackColor = true;
            this.NewSkin.Click += new System.EventHandler(this.NewSkin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Skins";
            // 
            // SkinView
            // 
            this.SkinView.Location = new System.Drawing.Point(7, 23);
            this.SkinView.Name = "SkinView";
            this.SkinView.Size = new System.Drawing.Size(154, 209);
            this.SkinView.TabIndex = 0;
            this.SkinView.DoubleClick += new System.EventHandler(this.SkinView_DoubleClick);
            this.SkinView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SkinView_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RemoveSkin);
            this.groupBox1.Controls.Add(this.NewSkin);
            this.groupBox1.Location = new System.Drawing.Point(7, 238);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 53);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Skins";
            // 
            // MaterialGroup
            // 
            this.MaterialGroup.Location = new System.Drawing.Point(7, 297);
            this.MaterialGroup.Name = "MaterialGroup";
            this.MaterialGroup.Size = new System.Drawing.Size(154, 157);
            this.MaterialGroup.TabIndex = 5;
            this.MaterialGroup.TabStop = false;
            this.MaterialGroup.Text = "Material";
            // 
            // ModelerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 502);
            this.Controls.Add(this.Sidebar);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "ModelerDialog";
            this.Text = "Modeler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModelerDialog_FormClosed);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.Sidebar.ResumeLayout(false);
            this.Sidebar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GLControl glControl1;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel Sidebar;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swapYZToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView SkinView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem headlightToolStripMenuItem;
        private System.Windows.Forms.Button RemoveSkin;
        private System.Windows.Forms.Button NewSkin;
        private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox MaterialGroup;

    }
}

