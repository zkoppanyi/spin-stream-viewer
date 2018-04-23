namespace OSUCalibrator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPrevToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToMATLABToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openYUVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertYUVFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateFileTimesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sampleLiDARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cornerDetectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.velodyneTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripButtonConsole = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDataStreams = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTimeLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHotFrames = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.algorithmsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(853, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.loadPrevToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToMATLABToolStripMenuItem,
            this.openYUVFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveToolStripMenuItem.Text = "Save Project";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.loadToolStripMenuItem.Text = "Load Project";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // loadPrevToolStripMenuItem
            // 
            this.loadPrevToolStripMenuItem.Name = "loadPrevToolStripMenuItem";
            this.loadPrevToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.loadPrevToolStripMenuItem.Text = "Recent Projects";
            this.loadPrevToolStripMenuItem.DropDownOpening += new System.EventHandler(this.loadPrevToolStripMenuItem_DropDownOpening);
            this.loadPrevToolStripMenuItem.Click += new System.EventHandler(this.loadPrevToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // exportToMATLABToolStripMenuItem
            // 
            this.exportToMATLABToolStripMenuItem.Name = "exportToMATLABToolStripMenuItem";
            this.exportToMATLABToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exportToMATLABToolStripMenuItem.Text = "Export to MATLAB";
            this.exportToMATLABToolStripMenuItem.Click += new System.EventHandler(this.exportToMATLABToolStripMenuItem_Click);
            // 
            // openYUVFileToolStripMenuItem
            // 
            this.openYUVFileToolStripMenuItem.Name = "openYUVFileToolStripMenuItem";
            this.openYUVFileToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.openYUVFileToolStripMenuItem.Text = "Open YUV File...";
            this.openYUVFileToolStripMenuItem.Click += new System.EventHandler(this.openYUVFileToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFolderToolStripMenuItem,
            this.convertYUVFilesToolStripMenuItem,
            this.updateFileTimesToolStripMenuItem,
            this.sampleLiDARToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // extractFolderToolStripMenuItem
            // 
            this.extractFolderToolStripMenuItem.Name = "extractFolderToolStripMenuItem";
            this.extractFolderToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.extractFolderToolStripMenuItem.Text = "Create Metadata File...";
            this.extractFolderToolStripMenuItem.Click += new System.EventHandler(this.extractFolderToolStripMenuItem_Click);
            // 
            // convertYUVFilesToolStripMenuItem
            // 
            this.convertYUVFilesToolStripMenuItem.Name = "convertYUVFilesToolStripMenuItem";
            this.convertYUVFilesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.convertYUVFilesToolStripMenuItem.Text = "Convert YUV Files...";
            this.convertYUVFilesToolStripMenuItem.Click += new System.EventHandler(this.convertYUVFilesToolStripMenuItem_Click);
            // 
            // updateFileTimesToolStripMenuItem
            // 
            this.updateFileTimesToolStripMenuItem.Name = "updateFileTimesToolStripMenuItem";
            this.updateFileTimesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.updateFileTimesToolStripMenuItem.Text = "Update File Times";
            this.updateFileTimesToolStripMenuItem.Click += new System.EventHandler(this.updateFileTimesToolStripMenuItem_Click);
            // 
            // sampleLiDARToolStripMenuItem
            // 
            this.sampleLiDARToolStripMenuItem.Name = "sampleLiDARToolStripMenuItem";
            this.sampleLiDARToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.sampleLiDARToolStripMenuItem.Text = "Sample LiDAR...";
            this.sampleLiDARToolStripMenuItem.Click += new System.EventHandler(this.sampleLiDARToolStripMenuItem_Click);
            // 
            // algorithmsToolStripMenuItem
            // 
            this.algorithmsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cornerDetectorToolStripMenuItem,
            this.velodyneTestToolStripMenuItem});
            this.algorithmsToolStripMenuItem.Name = "algorithmsToolStripMenuItem";
            this.algorithmsToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.algorithmsToolStripMenuItem.Text = "Algorithms";
            // 
            // cornerDetectorToolStripMenuItem
            // 
            this.cornerDetectorToolStripMenuItem.Name = "cornerDetectorToolStripMenuItem";
            this.cornerDetectorToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.cornerDetectorToolStripMenuItem.Text = "Corner detector";
            this.cornerDetectorToolStripMenuItem.Click += new System.EventHandler(this.cornerDetectorToolStripMenuItem_Click);
            // 
            // velodyneTestToolStripMenuItem
            // 
            this.velodyneTestToolStripMenuItem.Name = "velodyneTestToolStripMenuItem";
            this.velodyneTestToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.velodyneTestToolStripMenuItem.Text = "Velodyne Test";
            this.velodyneTestToolStripMenuItem.Click += new System.EventHandler(this.velodyneTestToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator2,
            this.toolStripButtonConsole,
            this.toolStripButtonDataStreams,
            this.toolStripButtonTimeLine,
            this.toolStripButtonHotFrames});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(853, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 650);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(853, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripButtonConsole
            // 
            this.toolStripButtonConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConsole.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConsole.Image")));
            this.toolStripButtonConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConsole.Name = "toolStripButtonConsole";
            this.toolStripButtonConsole.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonConsole.Text = "Console...";
            this.toolStripButtonConsole.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonDataStreams
            // 
            this.toolStripButtonDataStreams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDataStreams.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDataStreams.Image")));
            this.toolStripButtonDataStreams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDataStreams.Name = "toolStripButtonDataStreams";
            this.toolStripButtonDataStreams.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDataStreams.Text = "Data Streams...";
            this.toolStripButtonDataStreams.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButtonTimeLine
            // 
            this.toolStripButtonTimeLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTimeLine.Image = global::OSUCalibrator.Properties.Resources.timeline1;
            this.toolStripButtonTimeLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTimeLine.Name = "toolStripButtonTimeLine";
            this.toolStripButtonTimeLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTimeLine.Text = "Timeline...";
            this.toolStripButtonTimeLine.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButtonHotFrames
            // 
            this.toolStripButtonHotFrames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHotFrames.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHotFrames.Image")));
            this.toolStripButtonHotFrames.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHotFrames.Name = "toolStripButtonHotFrames";
            this.toolStripButtonHotFrames.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHotFrames.Text = "Hot Frames...";
            this.toolStripButtonHotFrames.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save...";
            this.toolStripButtonSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 672);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "OSU Calibrator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cornerDetectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertYUVFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonConsole;
        private System.Windows.Forms.ToolStripMenuItem exportToMATLABToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openYUVFileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonDataStreams;
        private System.Windows.Forms.ToolStripButton toolStripButtonTimeLine;
        private System.Windows.Forms.ToolStripMenuItem velodyneTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonHotFrames;
        private System.Windows.Forms.ToolStripMenuItem updateFileTimesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sampleLiDARToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPrevToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

