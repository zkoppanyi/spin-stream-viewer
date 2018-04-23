namespace OSUCalibrator
{
    partial class TimeLineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeLineForm));
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonJump = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRecord = new System.Windows.Forms.ToolStripButton();
            this.tsbExportVideo = new System.Windows.Forms.ToolStripButton();
            this.cmbFreq = new System.Windows.Forms.ToolStripComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolStripButtonShowGraph = new System.Windows.Forms.ToolStripButton();
            this.timeLineGraph = new ZedGraph.ZedGraphControl();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.Location = new System.Drawing.Point(-1, 380);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(566, 45);
            this.trackBar.TabIndex = 1;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPlay,
            this.toolStripButtonStop,
            this.toolStripButtonJump,
            this.toolStripSeparator2,
            this.tsbRecord,
            this.tsbExportVideo,
            this.toolStripSeparator1,
            this.toolStripButtonShowGraph,
            this.cmbFreq});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(565, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonPlay
            // 
            this.toolStripButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPlay.Image")));
            this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPlay.Name = "toolStripButtonPlay";
            this.toolStripButtonPlay.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPlay.Text = "Play";
            this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // toolStripButtonJump
            // 
            this.toolStripButtonJump.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonJump.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonJump.Image")));
            this.toolStripButtonJump.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonJump.Name = "toolStripButtonJump";
            this.toolStripButtonJump.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonJump.Text = "Jump...";
            this.toolStripButtonJump.ToolTipText = "Jump to time...";
            this.toolStripButtonJump.Click += new System.EventHandler(this.toolStripButton3_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRecord
            // 
            this.tsbRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRecord.Image = ((System.Drawing.Image)(resources.GetObject("tsbRecord.Image")));
            this.tsbRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRecord.Name = "tsbRecord";
            this.tsbRecord.Size = new System.Drawing.Size(23, 22);
            this.tsbRecord.ToolTipText = "Record";
            this.tsbRecord.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // tsbExportVideo
            // 
            this.tsbExportVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportVideo.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportVideo.Image")));
            this.tsbExportVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportVideo.Name = "tsbExportVideo";
            this.tsbExportVideo.Size = new System.Drawing.Size(23, 22);
            this.tsbExportVideo.Text = "Export";
            this.tsbExportVideo.ToolTipText = "Export videos...";
            this.tsbExportVideo.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // cmbFreq
            // 
            this.cmbFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFreq.Name = "cmbFreq";
            this.cmbFreq.Size = new System.Drawing.Size(121, 25);
            this.cmbFreq.SelectedIndexChanged += new System.EventHandler(this.cmbFreq_SelectedIndexChanged);
            this.cmbFreq.Click += new System.EventHandler(this.cmbFreq_Click);
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolStripButtonShowGraph
            // 
            this.toolStripButtonShowGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowGraph.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowGraph.Image")));
            this.toolStripButtonShowGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowGraph.Name = "toolStripButtonShowGraph";
            this.toolStripButtonShowGraph.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowGraph.Text = "Show graph";
            this.toolStripButtonShowGraph.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // timeLineGraph
            // 
            this.timeLineGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLineGraph.Location = new System.Drawing.Point(-1, 28);
            this.timeLineGraph.Name = "timeLineGraph";
            this.timeLineGraph.ScrollGrace = 0D;
            this.timeLineGraph.ScrollMaxX = 0D;
            this.timeLineGraph.ScrollMaxY = 0D;
            this.timeLineGraph.ScrollMaxY2 = 0D;
            this.timeLineGraph.ScrollMinX = 0D;
            this.timeLineGraph.ScrollMinY = 0D;
            this.timeLineGraph.ScrollMinY2 = 0D;
            this.timeLineGraph.Size = new System.Drawing.Size(566, 346);
            this.timeLineGraph.TabIndex = 0;
            this.timeLineGraph.UseExtendedPrintDialog = true;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TimeLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 427);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.timeLineGraph);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TimeLineForm";
            this.Text = "Timeline";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TimeLineForm_FormClosing);
            this.Load += new System.EventHandler(this.TimeLineForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripButton tsbRecord;
        private System.Windows.Forms.ToolStripButton tsbExportVideo;
        private System.Windows.Forms.ToolStripButton toolStripButtonJump;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cmbFreq;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowGraph;
        private ZedGraph.ZedGraphControl timeLineGraph;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}