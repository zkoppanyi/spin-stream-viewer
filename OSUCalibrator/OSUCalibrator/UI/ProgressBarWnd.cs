using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class ProgressBarWnd : Form, ILogger
    {
        public BackgroundWorker Worker { get; private set; }
        public CancellationTokenSource CancelTokenSource { get; set; }
        public ProgressBarStyle ProgressBarStyle { get { return progressBar.Style;  }  set { progressBar.Style = value;  } }
        public bool IsCancellable { get; set; }

        private const string btnTextKill = "Kill";
        private const string btnTextOK = "OK";

        public ProgressBarWnd()
        {
            InitializeComponent();

            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            CancelTokenSource = new CancellationTokenSource();
            IsCancellable = true;

            button.Text = btnTextKill;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button.Text = btnTextOK;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteProgress(e.ProgressPercentage);
        }

        private void ProgressBarWnd_Load(object sender, EventArgs e)
        {
            Worker.RunWorkerAsync();
            button.Enabled = IsCancellable == true;
        }

        private void ProgressBarWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Worker.IsBusy)
            {
                e.Cancel = true;
                Cancel();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button.Text == btnTextKill)
            {
                Cancel();
                Write("Cancelled...");
            }
            else if (button.Text == btnTextOK)
            {
                this.Close();
            }
        }

        public void Cancel()
        {
            Worker.CancelAsync();
            CancelTokenSource.Cancel();
        }

        public void WriteLine(String str)
        {
            Write(str + Environment.NewLine);
        }

        public void Write(String str)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(Write), new object[] { str });
                return;
            }

            textBoxConsole.AppendText(str);
        }

        public void Flush()
        {

        }

        public void WriteInfo(string str)
        {
            Write(str);
        }

        public void WriteLineInfo(string str)
        {
            WriteLine(str);
        }

        public void WriteWarning(string str)
        {
            Write(str);
        }

        public void WriteLineWarning(string str)
        {
            WriteLine(str);
        }

        public void WriteError(string str)
        {
            Write(str);
        }

        public void WriteLineError(string str)
        {
            WriteLine(str);
        }

        public void WriteProgress(double percnt)
        {
            if (InvokeRequired)
            {

                this.BeginInvoke(new Action<double>(WriteProgress), new object[] { Convert.ToInt32(percnt) });
                return;
            }

            progressBar.Value = Convert.ToInt32(percnt);
        }
    }
}
