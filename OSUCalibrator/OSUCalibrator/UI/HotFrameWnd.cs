using Accord.Math;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using OSUCalibrator.Loggers;
using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class FrameWnd : Form
    {
        private MainForm mainForm;
        private Project project;

        public FrameWnd()
        {
            InitializeComponent();
        }

        private void FrameWnd_Load(object sender, EventArgs e)
        {
            this.mainForm = (MainForm)this.MdiParent;
            this.project = mainForm.Project;
            UpdateUI();
        }

        public void UpdateUI()
        {
            lstFrames.Clear();

            foreach (HotFrame frame in project.HotFrames)
            {
                ListViewItem item = new ListViewItem();
                item.Text = frame.ToString();
                item.Tag = frame;
                lstFrames.Items.Add(item);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DateTime? time = mainForm.GlobalTime;
            if (time != null)
            {
                HotFrame frame = new HotFrame(time.Value);
                project.HotFrames.Add(frame);
                UpdateUI();
            }
            else
            {
                MessageBox.Show("No frame selected! Use the Timeline window to select frame!", "Select frame...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstFrames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {
                HotFrame hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;
                mainForm.ShowTimeLineWnd();
                mainForm.TimeLineWnd.SetHotFrame(hotFrame);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {
                HotFrame hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;
                project.HotFrames.Remove(hotFrame);
                UpdateUI();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {

                List<ILogger> loggers = new List<ILogger>();
                loggers.Add(mainForm.ConsoleLogger);

                ProgressBarWnd wnd = new ProgressBarWnd();
                wnd.Text = "Exporting frame...";
                wnd.ProgressBarStyle = ProgressBarStyle.Blocks;
                loggers.Add(wnd);

                MultipleLogger logger = new MultipleLogger(loggers);

                HotFrame hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;
                var rootFolder = project.Folder + "\\" + Project.MetadataFolder;
                var annot = hotFrame.Timestamp.ToString("HH_mm_ss");

                wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
                {
                    MatlabExporter.ExportHotFrame(rootFolder, annot, hotFrame, MatlabExporterOptions.Overwrite, logger, wnd.CancelTokenSource.Token);
                };

                wnd.ShowDialog();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {
                HotFrame hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;
                if (hotFrame != null)
                {
                    PropertiesWnd wnd = new PropertiesWnd(hotFrame, false);
                    if (wnd.ShowDialog() == DialogResult.OK)
                    {
                        Refresh();
                    }
                }
            }
        }

        private void lstFrames_Click(object sender, EventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {
                HotFrame hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;

                String info = "Time: " + hotFrame.Timestamp.ToString("yyyy.MM.dd HH:mm:ss.fff") + Environment.NewLine;
                info += "Streams: ";
                foreach (Feature feature in hotFrame.Features)
                {
                    info += feature.DataStream.ShortName;
                    if (feature is TimeFeature) info += "(T)";
                    info += ",";
                }

                lblInfo.Text = info;

            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (lstFrames.SelectedItems.Count > 0)
            {
                var hotFrame = lstFrames.SelectedItems[0].Tag as HotFrame;
                var mainForm = this.MdiParent as MainForm;
                mainForm.SampleLiDAR(hotFrame.Timestamp);
            }
        }
    }
}
