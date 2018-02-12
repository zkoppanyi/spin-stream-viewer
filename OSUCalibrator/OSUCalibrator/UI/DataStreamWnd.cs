using OSUCalibrator.DataStreams;
using OSUCalibrator.Loggers;
using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class DataStreamWnd : Form
    {
        public ILogger Logger { get; private set; }

        public DataStreamWnd(ILogger logger)
        {
            InitializeComponent();
            this.Logger = logger;
        }

        private void DataStreamWnd_Load(object sender, EventArgs e)
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            Project project = ((MainForm)this.MdiParent).Project;
            dataStreamList.Items.Clear();
            foreach (DataStream dataStream in project.DataStreams)
            {
                dataStreamList.Items.Add(dataStream);
            }
        }

        private void dataStreamList_DoubleClick(object sender, EventArgs e)
        {
            DataStream dataStream = (DataStream)dataStreamList.SelectedItem;
            if (dataStream.DataLines.Count == 0)
            {
                Logger.WriteLineWarning("No file for " + dataStream.ShortName);
                MessageBox.Show("No file for " + dataStream.ShortName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataStreamList.SelectedItem is VideoDataStream)
            {
                VideoDataStream videoDataStream = dataStreamList.SelectedItem as VideoDataStream;
                VideoStreamWnd wnd = new VideoStreamWnd(videoDataStream, Logger);
                wnd.MdiParent = this.MdiParent;
                wnd.Show();
            }
            else if (dataStreamList.SelectedItem is ImageDataStream)
            {
                ImageDataStream imageDataStream = dataStreamList.SelectedItem as ImageDataStream;
                ImageStreamWnd wnd = new ImageStreamWnd(imageDataStream, Logger);
                wnd.MdiParent = this.MdiParent;
                wnd.Show();
            }
            else if(dataStreamList.SelectedItem is VelodyneDataStream)
            {                
                VelodyneDataStream velodyneDataStream = dataStreamList.SelectedItem as VelodyneDataStream;
                if (velodyneDataStream.HasIndexedFiles() == false)
                {
                    DialogResult result = MessageBox.Show("No index file for " + dataStream.ShortName + "! Do you want to create index file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) return;
                    ConvertVelodyneDataStream(velodyneDataStream);
                }
                else
                {
                    VelodyneStreamWnd wnd = new VelodyneStreamWnd(velodyneDataStream, Logger);
                    wnd.MdiParent = this.MdiParent;
                    wnd.Show();
                }
            }
            else if (dataStreamList.SelectedItem is GPSDataStream)
            {
                GPSDataStream gpsDataStream = dataStreamList.SelectedItem as GPSDataStream;
                GPSDataStreamWnd wnd = new GPSDataStreamWnd(gpsDataStream, Logger);
                wnd.MdiParent = this.MdiParent;
                wnd.Show();
            }
        }

        /// <summary>
        /// Convert Velodyne data stream into internal format
        /// </summary>
        /// <param name="velodyneDataStream"></param>
        private void ConvertVelodyneDataStream(VelodyneDataStream velodyneDataStream)
        {
            MainForm form = (MainForm)this.MdiParent;
            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Conversion: " + velodyneDataStream.ShortName;

            // create multiple logger
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(Logger);
            loggers.Add(wnd);
            MultipleLogger logger = new MultipleLogger(loggers);


            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                foreach (VelodyneDataLine dataLine in velodyneDataStream.DataLines)
                {
                    VelodyneConverter converter = VelodyneConverter.Create(form.Project.Folder + "\\" + velodyneDataStream.SubFolder + "\\" + dataLine.PcapLocation);

                    converter.ProgressReport += delegate (object senderReport, ProgressReportEventArgs argsReport)
                    {
                        logger.WriteLineInfo(argsReport.CurrentDataTime.ToString("yyy-MM-dd hh:mm:ss") + " " + argsReport.ReadBytes / 1000000 + " MB " + argsReport.Precentage.ToString("0.00") + "%" + Environment.NewLine);
                        logger.WriteProgress(argsReport.Precentage);
                    };

                    try
                    {
                        converter.Convert(wnd.CancelTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // TODO: clean up files!
                    }
                    catch (Exception ex)
                    {
                        logger.WriteLineInfo("Error occured: " + ex.Message);
                    }
                }
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                logger.WriteLineInfo("Conversion done.");
                logger.WriteProgress(100);
            };

            wnd.ShowDialog();
        }


        private void dataStreamList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
