using Accord.Imaging.Filters;
using Accord.Video.FFMPEG;
using OSUCalibrator.DataStreams;
using OSUCalibrator.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace OSUCalibrator
{
    public partial class TimeLineForm : Form
    {
        private MainForm mainForm;
        private Project project;

        public DateTime CurrentTime { get; private set; }
        public HotFrame CurrentHotFrame { get; private set; }

        private DateTime minTime = DateTime.MaxValue;
        private DateTime maxTime = DateTime.MinValue;
        private double timeResolution = 0.1; 
        private VideoFileWriter videoWriter;

        private const String recordButtonTextStart = "R";
        private const String recordButtonTextStop = "R Stop";

        private const String saveButtonTextStart = "E";
        private const String saveButtonTextStop = "E Stop";
        private string exportFolder = "";
        private int timeLineGraphHeightDefault = 0;

        public TimeLineForm(Project project)
        {
            InitializeComponent();
            this.project = project;
        }

        private void TimeLineForm_Load(object sender, EventArgs e)
        {
            this.mainForm = (MainForm)(this.MdiParent);
            UpdateUI();
            tsbRecord.Text = recordButtonTextStart;
            tsbExportVideo.Text = saveButtonTextStart;

            cmbFreq.Items.Clear();
            cmbFreq.Items.Add(0.1);
            cmbFreq.Items.Add(0.2);
            cmbFreq.Items.Add(0.5);
            cmbFreq.Items.Add(1);
            cmbFreq.Items.Add(2);
            cmbFreq.SelectedItem = timeResolution;
            timeLineGraphHeightDefault = timeLineGraph.Height;

        }

        public void UpdateUI()
        {
            UpdateGraph();
            SetLimitsToTrackBar();
        }

        public void UpdateGraph()
        {
            GraphPane myPane = timeLineGraph.GraphPane;
            int dataStreamCount = 1;
            foreach (DataStream dataStream in project.DataStreams)
            {
                Color clr = Color.Black;
                if (dataStream is GPSDataStream) clr = Color.Green;
                if (dataStream is MicroStrainDataStream) clr = Color.GreenYellow;
                if (dataStream is ImageDataStream) clr = Color.Blue;
                if (dataStream is PointGreyDataStream) clr = Color.DarkBlue;
                if (dataStream is VideoDataStream) clr = Color.Red;
                if (dataStream is VelodyneDataStream) clr = Color.DarkGray;

                if (dataStream is GPSDataStream)
                {
                    PointPairList ptList = new PointPairList();
                    GPSDataStream gpsDataStream = (GPSDataStream)dataStream;
                    foreach (EvenMarkerDataLine evnt in gpsDataStream.MarkerEvents)
                    {
                        //if (evnt.Port == EvenMarkerDataLine.MarkerEventPort.EventB)
                        //{
                        //ptList.Add(new PointPair((new TimeSpan(evnt.TimeStamp.Ticks)).TotalSeconds, dataStreamCount));
                        ptList.Add(new XDate(evnt.TimeStamp.ToOADate()), dataStreamCount);
                        //}
                    }

                    if (ptList.Count() > 0)
                    {
                        TextObj text = new TextObj(dataStream.ShortName + " EVNT", ptList[0].X, ptList[0].Y, CoordType.AxisXYScale, AlignH.Right, AlignV.Center);
                        text.FontSpec.Size = 8f;
                        text.ZOrder = ZOrder.A_InFront;
                        text.FontSpec.Border.IsVisible = false;
                        text.FontSpec.Fill.IsVisible = false;
                        text.FontSpec.FontColor = clr;
                        myPane.GraphObjList.Add(text);

                        LineItem myCurve = myPane.AddCurve(dataStream.ShortName + " (" + dataStreamCount + ")", ptList, clr, SymbolType.Circle);
                        myCurve.Line.IsVisible = false;
                        myCurve.Symbol.Fill = new Fill(clr);
                        myCurve.Symbol.Size = 2;
                        dataStreamCount++;

                    }
                }

                if (dataStream.DataLines.Count() == 0) continue;

                if (dataStream.DataLines[0] is LongFileDataLine)
                {
                    bool isFirst = true;
                    foreach (LongFileDataLine evnt in dataStream.DataLines)
                    {
                        PointPairList ptList = new PointPairList();
                        //ptList.Add(new PointPair((new TimeSpan(evnt.TimeStamp.Ticks)).TotalSeconds, dataStreamCount));
                        //ptList.Add(new PointPair((new TimeSpan(evnt.TimeStamp.Ticks)).TotalSeconds + evnt.Length.TotalSeconds, dataStreamCount));
                        ptList.Add(new XDate(evnt.TimeStamp.ToOADate()), dataStreamCount);
                        ptList.Add(new XDate(evnt.TimeStamp.AddSeconds(evnt.Length.TotalSeconds).ToOADate()), dataStreamCount);

                        LineItem lineItem;
                        if (isFirst)
                        {
                            lineItem = myPane.AddCurve(dataStream.ShortName + " (" + dataStreamCount + ")", ptList, clr, SymbolType.Circle);
                            isFirst = false;

                            TextObj text = new TextObj(dataStream.ShortName, ptList[0].X, ptList[0].Y, CoordType.AxisXYScale, AlignH.Right, AlignV.Center);
                            text.FontSpec.Size = 8f;
                            text.ZOrder = ZOrder.A_InFront;
                            text.FontSpec.Border.IsVisible = false;
                            text.FontSpec.Fill.IsVisible = false;
                            text.FontSpec.FontColor = clr;
                            myPane.GraphObjList.Add(text);
                        }
                        else
                        {
                            lineItem = myPane.AddCurve(null, ptList, clr, SymbolType.Circle);
                        }
                        lineItem.Line.Style = DashStyle.Solid;
                        lineItem.Line.Width = 2;
                        lineItem.Color = clr;
                        lineItem.Symbol.Fill = new Fill(clr);
                        lineItem.Symbol.Size = 3;
                    }

                    dataStreamCount++;
                }
                else
                {
                    PointPairList ptList = new PointPairList();
                    foreach (DataLine evnt in dataStream.DataLines)
                    {
                        //ptList.Add(new PointPair((new TimeSpan(evnt.TimeStamp.Ticks)).TotalSeconds, dataStreamCount));
                        ptList.Add(new XDate(evnt.TimeStamp.ToOADate()), dataStreamCount);
                    }

                    LineItem myCurve = myPane.AddCurve(dataStream.ShortName + " (" + dataStreamCount + ")", ptList, clr, SymbolType.Circle);
                    myCurve.Line.IsVisible = false;
                    myCurve.Symbol.Fill = new Fill(clr);
                    myCurve.Symbol.Size = 2;

                    if (ptList.Count() > 0)
                    {
                        TextObj text = new TextObj(dataStream.ShortName, ptList[0].X, ptList[0].Y, CoordType.AxisXYScale, AlignH.Right, AlignV.Center);
                        text.FontSpec.Size = 8f;
                        text.ZOrder = ZOrder.A_InFront;
                        text.FontSpec.Border.IsVisible = false;
                        text.FontSpec.Fill.IsVisible = false;
                        text.FontSpec.FontColor = clr;
                        myPane.GraphObjList.Add(text);
                        dataStreamCount++;
                    }
                }
            }

            // Set the XAxis to date type
            myPane.XAxis.Type = AxisType.Date;
            /*myPane.XAxis.Scale.MajorStep = 10;
            myPane.XAxis.Scale.MajorUnit = DateUnit.Minute;
            // tilt the x axis labels to an angle of 65 degrees
            myPane.XAxis.Scale.FontSpec.Angle = 65;
            myPane.XAxis.Scale.FontSpec.IsBold = false;
            //myPane.XAxis.Scale.FontSpec.Size = 12;
            myPane.XAxis.Scale.Format = "hh:mm:ss";*/
            myPane.XAxis.Title.Text = "Time [UTC]";
            myPane.YAxis.Title.Text = "Streams";

            myPane.Legend.IsVisible = false;
            timeLineGraph.AxisChange();
            timeLineGraph.Invalidate();
        }

        public void SetLimitsToTrackBar()
        {
            foreach (DataStream dataStream in project.DataStreams)
            {
                if (dataStream is ImageDataStream)
                {
                    ImageDataStream imageDataStream = dataStream as ImageDataStream;
                    if ((imageDataStream.StartTime != DateTime.MaxValue) && (imageDataStream.EndTime != DateTime.MaxValue))
                    {
                        if (imageDataStream.StartTime < minTime) minTime = imageDataStream.StartTime;
                        if (imageDataStream.EndTime > maxTime) maxTime = imageDataStream.EndTime;
                    }
                }
            }

            double totalSecs = (new TimeSpan(maxTime.Ticks - minTime.Ticks)).TotalSeconds;
            trackBar.Minimum = 0;
            trackBar.Maximum = Convert.ToInt32(totalSecs / timeResolution);
            timer.Interval = Convert.ToInt32(timeResolution*1000.0);
            CurrentTime = minTime;
        }

        private void SetTrackBarTime(DateTime time)
        {
            trackBar.Value = Convert.ToInt32((new TimeSpan(time.Ticks - minTime.Ticks)).TotalSeconds / timeResolution);
            this.CurrentTime = time;
        }

        private DateTime GetTrackBarTime()
        {
            return minTime.AddSeconds((double)trackBar.Value * timeResolution);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            DateTime time = GetTrackBarTime();
            this.SetFrameByTime(time);
        }

        public void NextFrame()
        {
            DateTime newTime = CurrentTime.AddSeconds(this.timeResolution);
            SetFrameByTime(newTime);
        }

        /// <summary>
        /// Set frame by time for all IStreamWindow
        /// </summary>
        /// <param name="time">Time</param>
        public void SetFrameByTime(DateTime time)
        {            
            if ((time < minTime) || (time>maxTime))
            {
                timer.Stop();
                return;
            }

            foreach (Form form in mainForm.MdiChildren)
            {
                if ((!form.IsDisposed) && (form is IStreamWindow))
                {
                    IStreamWindow streamForm = form as IStreamWindow;
                    if (streamForm.HasToSetFrame)
                    {
                        streamForm.SetFrameByTime(time);
                        Form streamFormAsForm = streamForm as Form;
                        streamFormAsForm.Refresh();
                    }
                }
            }
            CurrentTime = time;
        }

        /// <summary>
        /// Set hotframe for all IStreamWindow
        /// </summary>
        /// <param name="frame">Hotframe object</param>
        public void SetHotFrame(HotFrame frame)
        {
            foreach (Form form in mainForm.MdiChildren)
            {
                if ((!form.IsDisposed) && (form is IStreamWindow))
                {
                    IStreamWindow streamForm = form as IStreamWindow;
                    if (streamForm.HasToSetFrame)
                    {
                        streamForm.SetHotFrame(frame);
                        Form streamFormAsForm = streamForm as Form;
                        streamFormAsForm.Refresh();
                    }
                }
            }

            SetTrackBarTime(frame.Timestamp);
            CurrentHotFrame = frame;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            NextFrame();

            // Recording mode
            if (tsbRecord.Text == recordButtonTextStop)
            {
                Bitmap snapshot = TakeSnapshot();
                try
                {
                    videoWriter.WriteVideoFrame(snapshot);
                    snapshot.Dispose();
                }
                catch
                {
                    //...
                }
            }

            // Exporting video
            if (tsbExportVideo.Text == saveButtonTextStop)
            {
                HotFrame frame = new HotFrame(this.CurrentTime);
                foreach (Form form in mainForm.MdiChildren)
                {
                    if ((!form.IsDisposed) && (form is IStreamWindow))
                    {
                        IStreamWindow streamWnd = form as IStreamWindow;
                        streamWnd.SetHotFrame(frame);                        
                    }
                }

                var annot = frame.Timestamp.ToString("HH_mm_ss_fff");
                MatlabExporter.ExportHotFrame(exportFolder, annot, frame, MatlabExporterOptions.Overwrite, mainForm.ConsoleLogger);
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (tsbRecord.Text == recordButtonTextStart)
            {
                try
                {
                    videoWriter = new VideoFileWriter();
                    videoWriter.Open("test.avi", 1920, 1440, Convert.ToInt32(1 / timeResolution), VideoCodec.MPEG4);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                tsbRecord.Text = recordButtonTextStop;
            }
            else if (tsbRecord.Text == recordButtonTextStop)
            {
                if (videoWriter != null)
                {
                    videoWriter.Close();
                }
                tsbRecord.Text = recordButtonTextStart;
            }
            else
            {
                throw new ArgumentException("Cannot be this! Very useful error message!");
            }

           
        }

        public Bitmap TakeSnapshot()
        {
            string[] order = { "GPR1", "NIKON", "SON2", "CAN1", "VHDL", "S5", "SEPT", "PTGREY1", "CAS2" };
            string[] title = { "GoPro/F", "NIKON/F", "Sony/F", "Canon/S", "LiDAR", "Samsung/F", "GPS", "PTGREY/R", "Casio/R" };

            int sub_img_width = 640;
            int sub_img_height = 480;

            Bitmap img = new Bitmap(sub_img_width*3, sub_img_height*3);
            using (Graphics g = Graphics.FromImage(img))
            {
                int x = 0;
                int y = 0;

                // reorder datastreams
                List<IStreamWindow> winds = new List<IStreamWindow>();
                int i = 0;
                foreach (string strStream in order)
                {
                    foreach (Form form in mainForm.MdiChildren)
                    {
                        if ((!form.IsDisposed) && (form is IStreamWindow))
                        {
                            IStreamWindow streamWnd = form as IStreamWindow;
                            if (streamWnd.DataStream.ShortName == strStream)
                            {
                                try
                                {
                                    Image snapshot = streamWnd.GetSnapshot();
                                    if (snapshot != null)
                                    {
                                        try
                                        {
                                            ResizeBilinear filter = new ResizeBilinear(sub_img_width, sub_img_height);
                                            Bitmap newImage = filter.Apply((Bitmap)snapshot);
                                            g.DrawImage(newImage, x, y, sub_img_width, sub_img_height);
                                            //g.FillRectangle(new SolidBrush(Color.White), x, y, 150, 30);
                                            g.DrawString(title[i], new Font("Arial", 30), Brushes.Blue, new PointF(x, y));
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }

                                        //if (strStream == "SEPT") snapshot.Dispose();
                                    }
                                    break;
                                } catch (Exception ex)
                                {
                                    if (mainForm.Console != null)
                                    {
                                        mainForm.Console.Write("Error: " + ex.Message);
                                    }
                                    timer.Stop();
                                }
                            }
                        }
                    }


                    i++;
                    x += sub_img_width;
                    if (x >= sub_img_width*3)
                    {
                        x = 0;
                        y += sub_img_height;
                    }
                }
            }

            return img;
        }

        private void TimeLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((tsbRecord.Text == recordButtonTextStop) && (videoWriter != null))
            {
                videoWriter.Close();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tsbExportVideo.Text == saveButtonTextStart)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = @"C:\OSU3\CAR\TestDb";
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        exportFolder = fbd.SelectedPath;
                        tsbExportVideo.Text = saveButtonTextStop;
                        mainForm.Console?.Write("Exporting open streams!");
                        mainForm.Console?.Write("Export folder: " + exportFolder);
                    }
                }

            }
            else if (tsbExportVideo.Text == saveButtonTextStop)
            {
                tsbExportVideo.Text = saveButtonTextStart;
            }
            else
            {
                throw new ArgumentException("Cannot be this! Very useful error message!");
            }
        }

        private void jumpToTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            TimeSelectorWnd timeLineForm = new TimeSelectorWnd(this.CurrentTime);
            if (timeLineForm.ShowDialog() == DialogResult.OK)
            {
                this.SetFrameByTime(timeLineForm.SelectedTime);
            }
        }

        private void cmbFreq_Click(object sender, EventArgs e)
        {
        }

        private void cmbFreq_SelectedIndexChanged(object sender, EventArgs e)
        {
            timeResolution = Convert.ToDouble(cmbFreq.SelectedItem);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (timeLineGraph.Visible == true)
            {
                timeLineGraph.Visible = false;
                this.Height = this.Height - timeLineGraph.Height;
            }
            else
            {
                timeLineGraph.Visible = true;
                this.Height = this.Height + timeLineGraphHeightDefault;
            }
        }
    }
}
