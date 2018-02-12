namespace SharpVelodyne
{
    partial class SimpleVelodyneViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.viewPort = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.viewPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPort.Location = new System.Drawing.Point(0, 0);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(428, 345);
            this.viewPort.TabIndex = 7;
            this.viewPort.TabStop = false;
            this.viewPort.SizeChanged += new System.EventHandler(this.pictureBox_SizeChanged);
            this.viewPort.Click += new System.EventHandler(this.pictureBox_Click);
            this.viewPort.Paint += new System.Windows.Forms.PaintEventHandler(this.viewPort_Paint);
            this.viewPort.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
            this.viewPort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.viewPort.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.viewPort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.viewPort.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            this.viewPort.Resize += new System.EventHandler(this.pictureBox_Resize);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Black;
            this.lblStatus.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 16);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "Status:";
            // 
            // lblHelp
            // 
            this.lblHelp.AutoSize = true;
            this.lblHelp.BackColor = System.Drawing.Color.Black;
            this.lblHelp.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelp.ForeColor = System.Drawing.Color.White;
            this.lblHelp.Location = new System.Drawing.Point(3, 19);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(16, 16);
            this.lblHelp.TabIndex = 16;
            this.lblHelp.Text = "?";
            this.lblHelp.MouseEnter += new System.EventHandler(this.lblHelp_MouseEnter);
            this.lblHelp.MouseLeave += new System.EventHandler(this.lblHelp_MouseLeave);
            // 
            // lblHelpText
            // 
            this.lblHelpText.AutoSize = true;
            this.lblHelpText.BackColor = System.Drawing.Color.Black;
            this.lblHelpText.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelpText.ForeColor = System.Drawing.Color.White;
            this.lblHelpText.Location = new System.Drawing.Point(51, 19);
            this.lblHelpText.Name = "lblHelpText";
            this.lblHelpText.Size = new System.Drawing.Size(80, 16);
            this.lblHelpText.TabIndex = 17;
            this.lblHelpText.Text = "Help text";
            this.lblHelpText.Visible = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SimpleVelodyneViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHelpText);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.viewPort);
            this.Name = "SimpleVelodyneViewer";
            this.Size = new System.Drawing.Size(428, 345);
            this.Load += new System.EventHandler(this.SimpleVelodyneViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox viewPort;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblHelpText;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
