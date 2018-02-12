namespace OSUCalibrator
{
    partial class DataStreamWnd
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
            this.dataStreamList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // dataStreamList
            // 
            this.dataStreamList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataStreamList.FormattingEnabled = true;
            this.dataStreamList.Location = new System.Drawing.Point(0, 0);
            this.dataStreamList.Name = "dataStreamList";
            this.dataStreamList.Size = new System.Drawing.Size(322, 402);
            this.dataStreamList.TabIndex = 0;
            this.dataStreamList.SelectedIndexChanged += new System.EventHandler(this.dataStreamList_SelectedIndexChanged);
            this.dataStreamList.DoubleClick += new System.EventHandler(this.dataStreamList_DoubleClick);
            // 
            // DataStreamWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 402);
            this.Controls.Add(this.dataStreamList);
            this.Name = "DataStreamWnd";
            this.Text = "Data Streams";
            this.Load += new System.EventHandler(this.DataStreamWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox dataStreamList;
    }
}