using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
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
    public partial class VelodyneStreamWnd : Form, IStreamWindow
    {
        public DataStream DataStream { get; private set; }

        /// <summary>
        /// Stroing multiple Velodyde data streams
        /// </summary>
        public List<VelodyneDataStream> VeodyneDataStreams { get; private set; }

        private IndexData idxStart;
        public bool HasToSetFrame { get; private set;  }
        public HotFrame CurrentHotFrame { get; private set; }
        private VelodyneReader veloReader;
        private bool isFireScrollEvent = true;

        private ILogger logger;

        public VelodyneStreamWnd(VelodyneDataStream dataStream, ILogger logger) : this( ToDataStreamList(dataStream), logger )
        {

        }

        public VelodyneStreamWnd(List<VelodyneDataStream> dataStreams, ILogger logger)
        {
            InitializeComponent();
            if (dataStreams.Count > 0)
            {
                this.DataStream = dataStreams[0];
            }
            VeodyneDataStreams = dataStreams;
            this.logger = logger;
            this.viewer.PointNumForPreview = 10000 * 2;
            this.HasToSetFrame = true;
            this.viewer.AnnotationClicked += Viewer_AnnotationClicked;
        }

        public List<VelodynePoint> GetPoints()
        {
            return viewer.GetPoints();
        }

        /// <summary>
        /// Providing conversrion for the single stream constructor
        /// </summary>
        /// <param name="dataStream"></param>
        /// <returns></returns>
        private static List<VelodyneDataStream> ToDataStreamList(VelodyneDataStream dataStream)
        {
            List<VelodyneDataStream> velodyneDataStreams = new List<VelodyneDataStream>();
            velodyneDataStreams.Add(dataStream);
            return velodyneDataStreams;
        }

        private void Viewer_AnnotationClicked(object sender, EventArgs e)
        {
            if (sender is VelodyneAnnotation)
            {
                VelodyneAnnotation annot = sender as VelodyneAnnotation;
                if (annot.Object is VeloFeature)
                {
                    VeloFeature feature = annot.Object as VeloFeature;
                    PropertiesWnd wnd = new PropertiesWnd(feature);
                    DialogResult dlg = wnd.ShowDialog();
                    if ((dlg == DialogResult.OK) && (!wnd.IsDeleteObject))
                    {
                        SetHotFrame(CurrentHotFrame);
                    }
                    else if ((dlg == DialogResult.OK) && (wnd.IsDeleteObject))
                    {
                        CurrentHotFrame.Features.Remove(feature);
                        SetHotFrame(CurrentHotFrame);
                    }
                }

            }
        }

        private void VelodyneStreamWnd_Load(object sender, EventArgs e)
        {
            MainForm form = (MainForm)this.MdiParent;
            VelodyneDataLine dataLine = ((VelodyneDataLine)DataStream.DataLines[0]);
            String pcapFile = form.Project.Folder + "\\" + DataStream.SubFolder + "\\" + dataLine.PcapLocation;
            String indexFile = VelodyneConverter.GetDefaultIndexFile(pcapFile);
            String pointFile = VelodyneConverter.GetDefaultPointFile(pcapFile);

            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Load data stream: " + ((VelodyneDataStream)DataStream).ShortName;
            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                VelodyneReader.ClearProgressReport();
                VelodyneReader.ProgressReport += delegate (object senderReader, ProgressReportEventArgs argsReport)
                {
                    form.ReportProgress(argsReport.Precentage);
                    logger.WriteInfo(argsReport.CurrentDataTime.ToString("yyy-MM-dd hh:mm:ss") + " " + argsReport.ReadBytes / 1000000 + " MB " + argsReport.Precentage.ToString("0.00") + "%" + Environment.NewLine);
                    wnd.WriteLine(argsReport.CurrentDataTime.ToString("yyy-MM-dd hh:mm:ss") + " " + argsReport.ReadBytes / 1000000 + " MB " + argsReport.Precentage.ToString("0.00") + "%" + Environment.NewLine);

                    ((BackgroundWorker)senderWorker).ReportProgress(Convert.ToInt32(argsReport.Precentage));
                };

                veloReader = VelodyneReader.Open(((VelodyneDataStream)DataStream).SensorType, ReturnMode.StrongestReturnOnly, indexFile, pointFile);
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                wnd.Close();
                form.ReportProgress(100);

                // set scroll bar
                idxStart = veloReader.Indeces.First();
                IndexData idxLast = veloReader.Indeces.Last();
                trackBar.Minimum = 0;
                trackBar.Maximum = Convert.ToInt32((idxLast.InternalTimeStamp.Ticks - idxStart.InternalTimeStamp.Ticks) / TimeSpan.TicksPerSecond);

                this.Text = "Streams: ";
                foreach ( VelodyneDataStream ds in this.VeodyneDataStreams)
                {
                    this.Text += ds.ShortName + " ";
                }

                // set reader to the first index
                veloReader.Seek(idxStart);
                viewer.Render();

                // Update stream times
                if (DataStream.DataLines.Count > 0)
                {
                    DateTime ts = (DataStream.DataLines[0] as VelodyneDataLine).TimeStamp;
                    DateTime gpsTime = idxStart.Nmea.GPSTime;
                    (DataStream.DataLines[0] as VelodyneDataLine).TimeStamp = (new DateTime(ts.Year, ts.Month, ts.Day, gpsTime.Hour, gpsTime.Minute, gpsTime.Second, gpsTime.Millisecond));
                    (DataStream.DataLines[0] as VelodyneDataLine).Length = new TimeSpan(idxLast.InternalTimeStamp.Ticks - idxStart.InternalTimeStamp.Ticks);
                    (DataStream.DataLines[0] as VelodyneDataLine).TimeType = TimeType.GPS;
                }


            };

            wnd.ShowDialog();            
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            NextFrame();
            viewer.RenderPreview();
        }

        public void NextFrame()
        {
            List<VelodynePoint> pts = veloReader.ReadNextFrame();
            viewer.ClearAndAddNewPoints(pts, SimpleVelodyneViewerRenderingMode.Manual);

            // handle hot frame
            if (CurrentHotFrame != null)
            {
                foreach (Feature feature in CurrentHotFrame.Features)
                {
                    if ((feature.DataStream == this.DataStream) && (feature is VeloFeature))
                    {
                        VeloFeature veloFeature = feature as VeloFeature;
                        VelodyneAnnotation annot = veloFeature.GetAnnotiation();
                        viewer.AddAnnotaion(annot, SimpleVelodyneViewerRenderingMode.Manual);
                        viewer.AddNewPoints(veloFeature.Points, SimpleVelodyneViewerColorMap.Selection);
                    }
                }
            }            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            viewer.Render();
        }

        private void VelodyneStreamWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            veloReader.Dispose();
        }

        private void viewer_Load(object sender, EventArgs e)
        {

        }

        public void SetFrameByTime(DateTime time)
        {
            SetFrameByTimeWithoutRender(time, null);
            viewer.RenderPreview();
        }

        public void SetFrameByTime(DateTime time, HotFrame hotFrame)
        {
            SetFrameByTimeWithoutRender(time, hotFrame);
            viewer.Render();
        }

        public DateTime GetTime()
        {
            List<VelodynePoint> veloPts = viewer.GetPoints();
            int idx = veloPts.Count / 2;
            return veloPts[idx].Timestamp.Value; // this might cause exception, fix then
        }

        public void SetFrameByTimeWithoutRender(DateTime time, HotFrame hotFrame) 
        {
            CurrentHotFrame = hotFrame;
            time = time.AddSeconds(-18);

            try
            {
                double dt = veloReader.SeekByTime(time, VelodyneReader.SearchType.FLOOR);
                if (dt > 1)
                {
                    viewer.Clear();
                    lblStatus.Text = "Out of bounds!";
                    return;
                }

                lblStatus.Text = "Time difference: " + dt.ToString("0.000") + "s";
                NextFrame(); // skip partial frame
                NextFrame(); // skip partial frame
            }
            catch (Exception ex)
            {
                viewer.Clear();
                lblStatus.Text = "Error occured while loading data!";
                logger?.WriteLineError(ex.Message);
            }

        }


        public Image GetSnapshot()
        {
            return viewer.Image;
        }

        private void trackBar_Scroll_1(object sender, EventArgs e)
        {
            if (isFireScrollEvent)
            {
                DateTime time = GetTimeFromTrackBar();
                SetFrameByTimeWithoutRender(time, null);
                viewer.Render();
            }
        }

        private void SetTrackBarToTime(DateTime time)
        {
            isFireScrollEvent = false;
            int val = Convert.ToInt32((time.Ticks - idxStart.InternalTimeStamp.Ticks) / TimeSpan.TicksPerSecond);
            if (val < 0)
            {
                logger.WriteLineWarning("Out of bound!");
                return;
            }
            trackBar.Value = val;
            isFireScrollEvent = true;
        }

        private DateTime GetTimeFromTrackBar()
        {
            return new DateTime(trackBar.Value * TimeSpan.TicksPerSecond + idxStart.InternalTimeStamp.Ticks); ;
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.HasToSetFrame)
            {
                this.HasToSetFrame = false;
                toolStripButton3.Image = Properties.Resources.not_world;
            }
            else
            {
                this.HasToSetFrame = true;
                toolStripButton3.Image = Properties.Resources.world;
            }
        }

        public void SetHotFrame(HotFrame frame)
        {
            CurrentHotFrame = frame;
            SetFrameByTimeWithoutRender(frame.Timestamp, frame);
            SetTrackBarToTime(frame.Timestamp);
            viewer.Render();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (CurrentHotFrame == null)
            {
                MessageBox.Show("Frame was not selected! Use frame selection dialog to specify hotframe!", "Frame not selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<VelodynePoint> points = viewer.GetSelectedPoints();
            if (points.Count() == 0)
            {
                MessageBox.Show("No point is selected! Feature won't be created!", "No point", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            VeloFeature feature = new VeloFeature(this.DataStream as VelodyneDataStream);
            feature.Points.AddRange(points);

            PropertiesWnd wnd = new PropertiesWnd(feature, false);
            DialogResult dlg = wnd.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                CurrentHotFrame.Features.Add(feature);
            }

            SetHotFrame(CurrentHotFrame);
        }

        private void viewer_Load_1(object sender, EventArgs e)
        {

        }
    }
}
