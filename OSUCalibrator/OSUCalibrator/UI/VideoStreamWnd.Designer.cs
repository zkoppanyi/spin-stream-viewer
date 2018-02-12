namespace OSUCalibrator
{
    partial class VideoStreamWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoStreamWnd));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadDataStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCurrentDataLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCurrentStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHotFramePropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.btnDigitize1 = new System.Windows.Forms.ToolStripButton();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.frameBox = new Cyotek.Windows.Forms.ImageBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(623, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadDataStreamToolStripMenuItem,
            this.showCurrentDataLineToolStripMenuItem,
            this.showCurrentStreamToolStripMenuItem,
            this.showHotFramePropertyToolStripMenuItem});
            this.dataToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // reloadDataStreamToolStripMenuItem
            // 
            this.reloadDataStreamToolStripMenuItem.Name = "reloadDataStreamToolStripMenuItem";
            this.reloadDataStreamToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.reloadDataStreamToolStripMenuItem.Text = "Reload Data Stream";
            this.reloadDataStreamToolStripMenuItem.Click += new System.EventHandler(this.reloadDataStreamToolStripMenuItem_Click);
            // 
            // showCurrentDataLineToolStripMenuItem
            // 
            this.showCurrentDataLineToolStripMenuItem.Name = "showCurrentDataLineToolStripMenuItem";
            this.showCurrentDataLineToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showCurrentDataLineToolStripMenuItem.Text = "Show DataLine Property...";
            this.showCurrentDataLineToolStripMenuItem.Click += new System.EventHandler(this.showCurrentDataLineToolStripMenuItem_Click);
            // 
            // showCurrentStreamToolStripMenuItem
            // 
            this.showCurrentStreamToolStripMenuItem.Name = "showCurrentStreamToolStripMenuItem";
            this.showCurrentStreamToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showCurrentStreamToolStripMenuItem.Text = "Show DataStream Property...";
            this.showCurrentStreamToolStripMenuItem.Click += new System.EventHandler(this.showCurrentStreamToolStripMenuItem_Click);
            // 
            // showHotFramePropertyToolStripMenuItem
            // 
            this.showHotFramePropertyToolStripMenuItem.Name = "showHotFramePropertyToolStripMenuItem";
            this.showHotFramePropertyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showHotFramePropertyToolStripMenuItem.Text = "Show HotFrame Property...";
            this.showHotFramePropertyToolStripMenuItem.Click += new System.EventHandler(this.showHotFramePropertyToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.fileToolStripMenuItem.Text = "Sync";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.playToolStripMenuItem.Text = "Tie Frame to Global";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.frameTimer_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.btnDigitize1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(623, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoSize = false;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Play";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Stop";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::OSUCalibrator.Properties.Resources.world1;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Tie to the Global Time";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(26, 22);
            this.toolStripButton4.Text = "Up";
            this.toolStripButton4.ToolTipText = "Move file up";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton5.Text = "Dwn";
            this.toolStripButton5.ToolTipText = "Move video file down";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(24, 22);
            this.toolStripButton6.Text = "RF";
            this.toolStripButton6.ToolTipText = "Refresh video file times";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // btnDigitize1
            // 
            this.btnDigitize1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDigitize1.Image = ((System.Drawing.Image)(resources.GetObject("btnDigitize1.Image")));
            this.btnDigitize1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDigitize1.Name = "btnDigitize1";
            this.btnDigitize1.Size = new System.Drawing.Size(42, 22);
            this.btnDigitize1.Text = "D (F1)";
            this.btnDigitize1.ToolTipText = "Digitize points... (F1)";
            this.btnDigitize1.Click += new System.EventHandler(this.btnDigitize1_Click);
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.Location = new System.Drawing.Point(351, 3);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(269, 45);
            this.trackBar.TabIndex = 3;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(623, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatus.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 17);
            this.lblStatus.Text = "...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.frameBox);
            this.splitContainer1.Size = new System.Drawing.Size(623, 371);
            this.splitContainer1.SplitterDistance = 129;
            this.splitContainer1.TabIndex = 5;
            // 
            // listFiles
            // 
            this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listFiles.FormattingEnabled = true;
            this.listFiles.Location = new System.Drawing.Point(0, 0);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(129, 371);
            this.listFiles.TabIndex = 0;
            this.listFiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listFiles_DrawItem);
            this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
            this.listFiles.DoubleClick += new System.EventHandler(this.listFiles_DoubleClick);
            // 
            // frameBox
            // 
            this.frameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameBox.Location = new System.Drawing.Point(0, 0);
            this.frameBox.Name = "frameBox";
            this.frameBox.Size = new System.Drawing.Size(490, 371);
            this.frameBox.TabIndex = 2;
            this.frameBox.Paint += new System.Windows.Forms.PaintEventHandler(this.frameBox_Paint);
            this.frameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frameBox_KeyDown);
            this.frameBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frameBox_MouseClick);
            this.frameBox.MouseEnter += new System.EventHandler(this.frameBox_MouseEnter_1);
            // 
            // VideoStreamWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 442);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VideoStreamWnd";
            this.Text = "Video";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoWnd_FormClosing);
            this.Load += new System.EventHandler(this.VideoWnd_Load);
            this.Shown += new System.EventHandler(this.VideoStreamWnd_Shown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VideoStreamWnd_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Cyotek.Windows.Forms.ImageBox frameBox;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCurrentDataLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCurrentStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHotFramePropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadDataStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDigitize1;
    }
}