namespace OSUCalibrator
{
    partial class ProgressBarWnd
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 235);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(440, 37);
            this.progressBar.TabIndex = 0;
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConsole.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConsole.Location = new System.Drawing.Point(12, 12);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConsole.Size = new System.Drawing.Size(490, 217);
            this.textBoxConsole.TabIndex = 1;
            // 
            // button
            // 
            this.button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button.Enabled = false;
            this.button.Location = new System.Drawing.Point(450, 236);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(52, 36);
            this.button.TabIndex = 2;
            this.button.Text = "Kill";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProgressBarWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 284);
            this.Controls.Add(this.button);
            this.Controls.Add(this.textBoxConsole);
            this.Controls.Add(this.progressBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressBarWnd";
            this.Text = "Progress...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressBarWnd_FormClosing);
            this.Load += new System.EventHandler(this.ProgressBarWnd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.Button button;
    }
}