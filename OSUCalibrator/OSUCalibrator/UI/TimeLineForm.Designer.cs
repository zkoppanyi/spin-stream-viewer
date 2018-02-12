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
            this.timeLineGraph = new ZedGraph.ZedGraphControl();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbRecord = new System.Windows.Forms.ToolStripButton();
            this.tsbExportVideo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmbFreq = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripSeparator1,
            this.tsbRecord,
            this.tsbExportVideo,
            this.cmbFreq});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(565, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // tsbRecord
            // 
            this.tsbRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRecord.Image = ((System.Drawing.Image)(resources.GetObject("tsbRecord.Image")));
            this.tsbRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRecord.Name = "tsbRecord";
            this.tsbRecord.Size = new System.Drawing.Size(23, 22);
            this.tsbRecord.Text = "R";
            this.tsbRecord.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // tsbExportVideo
            // 
            this.tsbExportVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExportVideo.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportVideo.Image")));
            this.tsbExportVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportVideo.Name = "tsbExportVideo";
            this.tsbExportVideo.Size = new System.Drawing.Size(23, 22);
            this.tsbExportVideo.Text = "E";
            this.tsbExportVideo.ToolTipText = "Export videos...";
            this.tsbExportVideo.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "J";
            this.toolStripButton3.ToolTipText = "Jumping to Time";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmbFreq
            // 
            this.cmbFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFreq.Name = "cmbFreq";
            this.cmbFreq.Size = new System.Drawing.Size(121, 25);
            this.cmbFreq.SelectedIndexChanged += new System.EventHandler(this.cmbFreq_SelectedIndexChanged);
            this.cmbFreq.Click += new System.EventHandler(this.cmbFreq_Click);
            // 
            // TimeLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 427);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.timeLineGraph);
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

        private ZedGraph.ZedGraphControl timeLineGraph;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripButton tsbRecord;
        private System.Windows.Forms.ToolStripButton tsbExportVideo;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cmbFreq;
    }
}