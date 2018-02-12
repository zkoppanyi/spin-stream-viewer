namespace OSUCalibrator.UI
{
    partial class TimeSelectorWnd
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSecond = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSecond);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtMinute);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtHour);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDay);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 184);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time";
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(84, 19);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(115, 20);
            this.txtYear.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Year";
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(84, 45);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(115, 20);
            this.txtMonth.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Month";
            // 
            // txtDay
            // 
            this.txtDay.Location = new System.Drawing.Point(84, 71);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(115, 20);
            this.txtDay.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Day";
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(84, 97);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(115, 20);
            this.txtHour.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Hour";
            // 
            // txtMinute
            // 
            this.txtMinute.Location = new System.Drawing.Point(84, 123);
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(115, 20);
            this.txtMinute.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Minute";
            // 
            // txtSecond
            // 
            this.txtSecond.Location = new System.Drawing.Point(84, 149);
            this.txtSecond.Name = "txtSecond";
            this.txtSecond.Size = new System.Drawing.Size(115, 20);
            this.txtSecond.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Seconds";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(157, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(76, 202);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TimeSelectorWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 237);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "TimeSelectorWnd";
            this.Text = "Time selector...";
            this.Load += new System.EventHandler(this.TimeSelectorWnd_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSecond;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}