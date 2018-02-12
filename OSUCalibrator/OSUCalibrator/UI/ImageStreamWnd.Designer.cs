namespace OSUCalibrator
{
    partial class ImageStreamWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageStreamWnd));
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusSecond = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDataLinePropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDataStreamPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHotFramePropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToNOVATELToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToSEPTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tieToGlobalFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToNOVATELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToSEPTFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.btnDigitize1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.btnDigitize = new System.Windows.Forms.ToolStripButton();
            this.frameBox = new Cyotek.Windows.Forms.ImageBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFileTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromCurrentTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.Location = new System.Drawing.Point(179, 0);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(280, 45);
            this.trackBar.TabIndex = 1;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 339);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(457, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(51, 17);
            this.lblStatus.Text = "Status:...";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatus.Text = "Status:";
            // 
            // toolStripStatusSecond
            // 
            this.toolStripStatusSecond.Name = "toolStripStatusSecond";
            this.toolStripStatusSecond.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusSecond.Text = "...";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem,
            this.syncToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(457, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDataLinePropertyToolStripMenuItem,
            this.showDataStreamPropertyToolStripMenuItem,
            this.showHotFramePropertyToolStripMenuItem});
            this.dataToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // showDataLinePropertyToolStripMenuItem
            // 
            this.showDataLinePropertyToolStripMenuItem.Name = "showDataLinePropertyToolStripMenuItem";
            this.showDataLinePropertyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showDataLinePropertyToolStripMenuItem.Text = "Show Data Line Property...";
            this.showDataLinePropertyToolStripMenuItem.Click += new System.EventHandler(this.showDataLinePropertyToolStripMenuItem_Click_1);
            // 
            // showDataStreamPropertyToolStripMenuItem
            // 
            this.showDataStreamPropertyToolStripMenuItem.Name = "showDataStreamPropertyToolStripMenuItem";
            this.showDataStreamPropertyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showDataStreamPropertyToolStripMenuItem.Text = "Show DataStream Property...";
            this.showDataStreamPropertyToolStripMenuItem.Click += new System.EventHandler(this.showDataStreamPropertyToolStripMenuItem_Click);
            // 
            // showHotFramePropertyToolStripMenuItem
            // 
            this.showHotFramePropertyToolStripMenuItem.Name = "showHotFramePropertyToolStripMenuItem";
            this.showHotFramePropertyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showHotFramePropertyToolStripMenuItem.Text = "Show HotFrame Property...";
            this.showHotFramePropertyToolStripMenuItem.Click += new System.EventHandler(this.showHotFramePropertyToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem1
            // 
            this.syncToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncToNOVATELToolStripMenuItem1,
            this.syncToSEPTToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tieToGlobalFrameToolStripMenuItem,
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem});
            this.syncToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.syncToolStripMenuItem1.Name = "syncToolStripMenuItem1";
            this.syncToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.syncToolStripMenuItem1.Text = "Sync";
            // 
            // syncToNOVATELToolStripMenuItem1
            // 
            this.syncToNOVATELToolStripMenuItem1.Name = "syncToNOVATELToolStripMenuItem1";
            this.syncToNOVATELToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.syncToNOVATELToolStripMenuItem1.Text = "Sync to NOVATEL";
            this.syncToNOVATELToolStripMenuItem1.Click += new System.EventHandler(this.syncToNOVATELToolStripMenuItem_Click);
            // 
            // syncToSEPTToolStripMenuItem
            // 
            this.syncToSEPTToolStripMenuItem.Name = "syncToSEPTToolStripMenuItem";
            this.syncToSEPTToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.syncToSEPTToolStripMenuItem.Text = "Sync to SEPT";
            this.syncToSEPTToolStripMenuItem.Click += new System.EventHandler(this.syncToSEPTFromFileToolStripMenuItem_Click);
            // 
            // tieToGlobalFrameToolStripMenuItem
            // 
            this.tieToGlobalFrameToolStripMenuItem.Name = "tieToGlobalFrameToolStripMenuItem";
            this.tieToGlobalFrameToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.tieToGlobalFrameToolStripMenuItem.Text = "Tie to Global Frame";
            this.tieToGlobalFrameToolStripMenuItem.Click += new System.EventHandler(this.tieToGlobalFrameToolStripMenuItem_Click);
            // 
            // calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem
            // 
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem.Name = "calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem";
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem.Text = "Calculate FT and GT Diff...";
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem.ToolTipText = "Calcualte File time and Global time difference";
            this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem.Click += new System.EventHandler(this.calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncToNOVATELToolStripMenuItem,
            this.syncToSEPTFromFileToolStripMenuItem});
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.syncToolStripMenuItem.Text = "Sync";
            // 
            // syncToNOVATELToolStripMenuItem
            // 
            this.syncToNOVATELToolStripMenuItem.Name = "syncToNOVATELToolStripMenuItem";
            this.syncToNOVATELToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.syncToNOVATELToolStripMenuItem.Text = "Sync to NOVATEL";
            this.syncToNOVATELToolStripMenuItem.Click += new System.EventHandler(this.syncToNOVATELToolStripMenuItem_Click);
            // 
            // syncToSEPTFromFileToolStripMenuItem
            // 
            this.syncToSEPTFromFileToolStripMenuItem.Name = "syncToSEPTFromFileToolStripMenuItem";
            this.syncToSEPTFromFileToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.syncToSEPTFromFileToolStripMenuItem.Text = "Sync To SEPT From File";
            this.syncToSEPTFromFileToolStripMenuItem.Click += new System.EventHandler(this.syncToSEPTFromFileToolStripMenuItem_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.btnDigitize1,
            this.toolStripButton7,
            this.toolStripButton8});
            this.toolStrip2.Location = new System.Drawing.Point(0, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(457, 25);
            this.toolStrip2.TabIndex = 7;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.AutoSize = false;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Play";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Stop";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::OSUCalibrator.Properties.Resources.world1;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "Tie to the Global Time";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton3_Click);
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
            this.btnDigitize1.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "B";
            this.toolStripButton7.ToolTipText = "Reverse Back";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton8.Text = "F";
            this.toolStripButton8.ToolTipText = "Move Forward";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
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
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
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
            // btnDigitize
            // 
            this.btnDigitize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDigitize.Image = ((System.Drawing.Image)(resources.GetObject("btnDigitize.Image")));
            this.btnDigitize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDigitize.Name = "btnDigitize";
            this.btnDigitize.Size = new System.Drawing.Size(23, 22);
            this.btnDigitize.Text = "D";
            this.btnDigitize.ToolTipText = "Digitize Points";
            this.btnDigitize.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // frameBox
            // 
            this.frameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameBox.Location = new System.Drawing.Point(0, 24);
            this.frameBox.Name = "frameBox";
            this.frameBox.Size = new System.Drawing.Size(457, 337);
            this.frameBox.TabIndex = 2;
            this.frameBox.Click += new System.EventHandler(this.frameBox_Click);
            this.frameBox.Paint += new System.Windows.Forms.PaintEventHandler(this.frameBox_Paint);
            this.frameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frameBox_KeyDown);
            this.frameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frameBox_KeyPress);
            this.frameBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frameBox_MouseClick);
            this.frameBox.MouseEnter += new System.EventHandler(this.frameBox_MouseEnter);
            this.frameBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frameBox_MouseMove);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromFileTimeToolStripMenuItem,
            this.fromCurrentTimeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem1.Text = "Sync Manually";
            // 
            // fromFileTimeToolStripMenuItem
            // 
            this.fromFileTimeToolStripMenuItem.Name = "fromFileTimeToolStripMenuItem";
            this.fromFileTimeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fromFileTimeToolStripMenuItem.Text = "From File Time";
            this.fromFileTimeToolStripMenuItem.Click += new System.EventHandler(this.fromFileTimeToolStripMenuItem_Click);
            // 
            // fromCurrentTimeToolStripMenuItem
            // 
            this.fromCurrentTimeToolStripMenuItem.Name = "fromCurrentTimeToolStripMenuItem";
            this.fromCurrentTimeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.fromCurrentTimeToolStripMenuItem.Text = "From Current Time";
            this.fromCurrentTimeToolStripMenuItem.Click += new System.EventHandler(this.fromCurrentTimeToolStripMenuItem_Click);
            // 
            // ImageStreamWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 361);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.frameBox);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ImageStreamWnd";
            this.Text = "Image Stream";
            this.Load += new System.EventHandler(this.ImageStreamWnd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TrackBar trackBar;
        private Cyotek.Windows.Forms.ImageBox frameBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncToNOVATELToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncToSEPTFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSecond;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton btnDigitize;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton btnDigitize1;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem syncToNOVATELToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem syncToSEPTToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDataStreamPropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHotFramePropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tieToGlobalFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDataLinePropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripMenuItem calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fromFileTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromCurrentTimeToolStripMenuItem;
    }
}