namespace ManaSourceSpriteTool
{
    partial class NewActionForm
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
            this.ActionNameItem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ImageSetList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CardinalRadio = new System.Windows.Forms.RadioButton();
            this.AnyRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(233, 117);
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
            this.Cancel.Location = new System.Drawing.Point(152, 117);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // ActionNameItem
            // 
            this.ActionNameItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionNameItem.Location = new System.Drawing.Point(15, 25);
            this.ActionNameItem.Name = "ActionNameItem";
            this.ActionNameItem.Size = new System.Drawing.Size(293, 20);
            this.ActionNameItem.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ImageSet";
            // 
            // ImageSetList
            // 
            this.ImageSetList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageSetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ImageSetList.FormattingEnabled = true;
            this.ImageSetList.Location = new System.Drawing.Point(15, 64);
            this.ImageSetList.Name = "ImageSetList";
            this.ImageSetList.Size = new System.Drawing.Size(293, 21);
            this.ImageSetList.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AnyRadio);
            this.groupBox1.Controls.Add(this.CardinalRadio);
            this.groupBox1.Location = new System.Drawing.Point(15, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(122, 50);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Directions";
            // 
            // CardinalRadio
            // 
            this.CardinalRadio.AutoSize = true;
            this.CardinalRadio.Location = new System.Drawing.Point(6, 19);
            this.CardinalRadio.Name = "CardinalRadio";
            this.CardinalRadio.Size = new System.Drawing.Size(63, 17);
            this.CardinalRadio.TabIndex = 0;
            this.CardinalRadio.TabStop = true;
            this.CardinalRadio.Text = "Cardinal";
            this.CardinalRadio.UseVisualStyleBackColor = true;
            // 
            // AnyRadio
            // 
            this.AnyRadio.AutoSize = true;
            this.AnyRadio.Location = new System.Drawing.Point(75, 19);
            this.AnyRadio.Name = "AnyRadio";
            this.AnyRadio.Size = new System.Drawing.Size(43, 17);
            this.AnyRadio.TabIndex = 1;
            this.AnyRadio.TabStop = true;
            this.AnyRadio.Text = "Any";
            this.AnyRadio.UseVisualStyleBackColor = true;
            // 
            // NewActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 152);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ImageSetList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ActionNameItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewActionForm";
            this.Text = "NewActionForm";
            this.Load += new System.EventHandler(this.NewActionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ActionNameItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ImageSetList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton AnyRadio;
        private System.Windows.Forms.RadioButton CardinalRadio;
    }
}