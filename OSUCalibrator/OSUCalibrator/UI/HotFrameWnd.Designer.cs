namespace OSUCalibrator
{
    partial class FrameWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameWnd));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblInfo = new System.Windows.Forms.Label();
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstFrames = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.velodyneExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonAddFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveFrame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddFrame,
            this.toolStripButtonRemoveFrame,
            this.toolStripButtonExport,
            this.toolStripButtonInfo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(318, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(0, 259);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(318, 77);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Info: ...";
            // 
            // Date
            // 
            this.Date.Text = "Date";
            // 
            // lstFrames
            // 
            this.lstFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFrames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Date});
            this.lstFrames.GridLines = true;
            this.lstFrames.Location = new System.Drawing.Point(0, 52);
            this.lstFrames.Name = "lstFrames";
            this.lstFrames.Size = new System.Drawing.Size(318, 204);
            this.lstFrames.TabIndex = 0;
            this.lstFrames.UseCompatibleStateImageBehavior = false;
            this.lstFrames.View = System.Windows.Forms.View.List;
            this.lstFrames.SelectedIndexChanged += new System.EventHandler(this.lstFrames_SelectedIndexChanged);
            this.lstFrames.Click += new System.EventHandler(this.lstFrames_Click);
            this.lstFrames.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstFrames_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(318, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.velodyneExportToolStripMenuItem});
            this.dataToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.dataToolStripMenuItem.Text = "Hot Frame";
            // 
            // velodyneExportToolStripMenuItem
            // 
            this.velodyneExportToolStripMenuItem.Name = "velodyneExportToolStripMenuItem";
            this.velodyneExportToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.velodyneExportToolStripMenuItem.Text = "Velodyne Export...";
            this.velodyneExportToolStripMenuItem.Click += new System.EventHandler(this.velodyneExportToolStripMenuItem_Click);
            // 
            // toolStripButtonAddFrame
            // 
            this.toolStripButtonAddFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddFrame.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddFrame.Image")));
            this.toolStripButtonAddFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddFrame.Name = "toolStripButtonAddFrame";
            this.toolStripButtonAddFrame.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddFrame.Tag = "";
            this.toolStripButtonAddFrame.Text = "A";
            this.toolStripButtonAddFrame.ToolTipText = "Add frame";
            this.toolStripButtonAddFrame.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonRemoveFrame
            // 
            this.toolStripButtonRemoveFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveFrame.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemoveFrame.Image")));
            this.toolStripButtonRemoveFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveFrame.Name = "toolStripButtonRemoveFrame";
            this.toolStripButtonRemoveFrame.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveFrame.Text = "R";
            this.toolStripButtonRemoveFrame.ToolTipText = "Remove frame";
            this.toolStripButtonRemoveFrame.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExport.Image")));
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExport.Text = "E";
            this.toolStripButtonExport.ToolTipText = "Export frame to Matlab";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButtonInfo
            // 
            this.toolStripButtonInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInfo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonInfo.Image")));
            this.toolStripButtonInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInfo.Name = "toolStripButtonInfo";
            this.toolStripButtonInfo.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInfo.Text = "I";
            this.toolStripButtonInfo.ToolTipText = "Info...";
            this.toolStripButtonInfo.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // FrameWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 336);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lstFrames);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrameWnd";
            this.Text = "Frames";
            this.Load += new System.EventHandler(this.FrameWnd_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveFrame;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ToolStripButton toolStripButtonInfo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ListView lstFrames;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem velodyneExportToolStripMenuItem;
    }
}