using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class ImageStreamWnd : Form, IStreamWindow
    {       

        public HotFrame CurrentHotFrame { get; private set; }
        public bool HasToSetFrame { get; private set; }

        private ImageDataStream dataStream;
        public DataStream DataStream { get { return dataStream; } }

        private ImageDataLine currentDataLine { get { return this.dataStream.DataLines[currentFrame] as ImageDataLine; }  }
        private Project project { get; set; }
        public ILogger logger { get; set; }
        private StreamWindowState windowState;
        private int currentFrame = 0;
        private int lastId = 0;
        private double timeDiff = 0;

        public ImageStreamWnd(ImageDataStream dataStream, ILogger logger)
        {
            InitializeComponent();

            this.dataStream = dataStream;
            this.logger = logger;
            timer.Interval = 500;

            trackBar.Minimum = 0;
            trackBar.Maximum = Convert.ToInt32(dataStream.DataLines.Count());
            this.HasToSetFrame = true;

            this.Text = "Image Stream: " + dataStream.ShortName;
            this.windowState = StreamWindowState.Normal;
        }

        private void ImageStreamWnd_Load(object sender, EventArgs e)
        {
            project = ((MainForm)this.MdiParent).Project;
            SetFrame(currentFrame);
            lastId = project.GetNextId();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextFrame();
        }

        public void NextFrame()
        {
            if (currentFrame + 1 >= DataStream.DataLines.Count)
            {
                timer.Stop();
                return;
            }

            SetFrame(currentFrame + 1);
            toolStripStatusSecond.Text = "";

        }

        public void SetFrame(int frame)
        {
            CurrentHotFrame = null;

            if ((frame >= 0) && (frame < dataStream.DataLines.Count()))
            {
                if (frame != currentFrame)
                {
                    if (frameBox.Image != null) frameBox.Image.Dispose();
                    ImageDataLine dataLine = (ImageDataLine)dataStream.DataLines[frame];
                    String fileName = dataLine.ImageFileName;

                    if (Path.GetExtension(fileName) == ".yuv")
                    {
                        frameBox.Image = YUVConverter.Convert(project.Folder + "\\" + dataStream.SubFolder + "\\" + fileName, logger);
                    }
                    else
                    {
                        frameBox.Image = Image.FromFile(project.Folder + "\\" + dataStream.SubFolder + "\\" + fileName);
                    }

                    frameBox.ZoomToFit();
                    frameBox.Refresh();
                    currentFrame = frame;

                    //some UI stuff
                    lblStatus.Text = dataLine.TimeType.ToString() + " " + dataLine.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff") + " #: " + currentFrame;
                    trackBar.Value = currentFrame;
                }


            }
            else
            {
                frameBox.Image = null;
                if (frame < 0)
                {
                    currentFrame = 0;
                    trackBar.Value = 0;
                } else if (frame > dataStream.DataLines.Count())
                {
                    currentFrame = dataStream.DataLines.Count()-1;
                    trackBar.Value = trackBar.Maximum;
                }

                //some UI stuff
                toolStripStatus.Text = "Outside of the stream time limits.";
            }
        }

        public void SetFrameByTime(DateTime time)
        {
            timeDiff = Double.MaxValue;
            int min_frame = -1;
            for (int i = 0; i < dataStream.DataLines.Count; i++)
            {
                // return with the floor value
                if (dataStream.DataLines[i].TimeStamp.Ticks > time.Ticks )
                {
                    min_frame = i-1;
                    int min_frame2 = min_frame < 0 ? 0 : min_frame;
                    timeDiff = Math.Abs((new TimeSpan(dataStream.DataLines[min_frame2].TimeStamp.Ticks - time.Ticks)).TotalSeconds);
                    break;
                }
            }

            SetFrame(min_frame);

            if ((timeDiff == Double.MaxValue) && (dataStream.DataLines.Count() != 0))
            {
                timeDiff = Math.Abs((new TimeSpan(dataStream.DataLines[dataStream.DataLines.Count()-1].TimeStamp.Ticks - time.Ticks)).TotalSeconds);
            }

            lblStatus.Text = "Req.: " + time.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Diff: " + timeDiff;
            if (timeDiff > 3)
            {
                frameBox.Image = null;
                lblStatus.Text += "Outside of the boundaries!";
            }

          
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            SetFrame(trackBar.Value);
        }

        private void syncToNOVATELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.WriteLineInfo(" ");
            logger.WriteLineInfo("Matching NIKON images to NOVATEL EVNT timestamps");

            GPSDataStream novatel = project.DataStreams.Find(x => x.ShortName == "NOVATEL") as GPSDataStream;

            if (dataStream == null)
            {
                logger.WriteLineError("Cannot find NIKON datastream in the project!");
                return;
            }

            DateTime startTime = dataStream.StartTime - TimeSpan.FromMinutes(2);
            DateTime endTime = dataStream.EndTime + TimeSpan.FromMinutes(2);
            int numMarkers = 0;

            // check events and image numbers
            foreach (EvenMarkerDataLine evnt in novatel.MarkerEvents)
            {
                if ((startTime <= evnt.TimeStamp) && (evnt.TimeStamp <= endTime))
                {
                    numMarkers++;
                }
            }

            if (dataStream.ShortName != "NIKON")
            {
                logger.WriteLineWarning("This is not NIKON stream! This method is developed for NIKON!");
            }

            if (numMarkers != novatel.MarkerEvents.Count)
            {
                logger.WriteLineError("No of marker events does not match with NIKON images!");
                logger.WriteLineError("# of NIKON images: " + dataStream.DataLines.Count);
                logger.WriteLineError("# of NOVATEL EVNT: " + novatel.MarkerEvents.Count);
                return;
            }

            logger.WriteLineInfo("# of NIKON images: " + dataStream.DataLines.Count);
            logger.WriteLineInfo("# of NOVATEL EVNT: " + novatel.MarkerEvents.Count);

            logger.WriteLineInfo("Start dT [s]=" + (new TimeSpan(dataStream.DataLines[0].TimeStamp.Ticks - novatel.MarkerEvents[0].TimeStamp.Ticks)).TotalSeconds);
            //logger.WriteLineInfo("End dT [s]=" + (new TimeSpan(dataStream.DataLines[dataStream.DataLines.Count-1].TimeStamp.Ticks - novatel.MarkerEvents[dataStream.DataLines.Count-1].TimeStamp.Ticks)).TotalSeconds);

            double[] dts = new double[novatel.MarkerEvents.Count];
            for (int i = 0; i < novatel.MarkerEvents.Count; i++)
            {
                if ((startTime <= novatel.MarkerEvents[i].TimeStamp) && (novatel.MarkerEvents[i].TimeStamp <= endTime))
                {
                    try
                    {
                        dts[i] = Math.Abs((new TimeSpan(dataStream.DataLines[0].TimeStamp.Ticks - novatel.MarkerEvents[0].TimeStamp.Ticks)).TotalSeconds);
                        dataStream.DataLines[i].TimeStamp = novatel.MarkerEvents[i].TimeStamp;
                        dataStream.DataLines[i].TimeType = TimeType.GPS;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            this.dataStream.OrderDataLines();

            logger.WriteLineInfo("Average dT [s]= "+ dts.Average());
            logger.WriteLineInfo("Maximum dT [s]= " + dts.Max());
            logger.WriteLineInfo("Minimum dT [s]= " + dts.Min());

            logger.WriteLineInfo("Done");
        }

        private async void syncToSEPTFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataStream.ShortName.Contains("SON"))
            {
                logger.WriteLineWarning("This is not SON stream! This method is developed for SON!");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await SyncSept(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    logger.WriteLineError("Error: Could not read file from disk. Original error: " + ex.Message);
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private async Task SyncSept(String fileName)
        {

            using (StreamReader reader = File.OpenText(fileName))
            {
                reader.ReadLine();
                int num_dates = 0;
                int num_lines = 0;
                List<double> dts = new List<double>();

                while (!reader.EndOfStream)
                {
                    num_lines++;
                    string[] elems = reader.ReadLine().Split(',');
                    String imageName = elems[0];
                    ImageDataLine dataLine = (ImageDataLine)dataStream.DataLines.Find(x => ((ImageDataLine)x).ImageFileName == imageName);
                    if (dataLine == null) continue;

                    DateTime dateTime = new DateTime(Convert.ToInt32(elems[1]), Convert.ToInt32(elems[2]), Convert.ToInt32(elems[3]),
                        Convert.ToInt32(elems[4]), Convert.ToInt32(elems[5]), 0);
                    dateTime = dateTime.AddSeconds(Convert.ToDouble(elems[6]));

                    dts.Add(Math.Abs((new TimeSpan(dataLine.TimeStamp.Ticks - dateTime.Ticks)).TotalSeconds));

                    dataLine.TimeStamp = dateTime;
                    dataLine.TimeType = TimeType.GPS;
                    num_dates++;

                    logger.WriteProgress((double)reader.BaseStream.Position / (double)reader.BaseStream.Length * 100);

                }

                this.dataStream.OrderDataLines();
                logger.WriteLineInfo("Number of updated items: " + num_dates + " out of " + dataStream.DataLines.Count());
                logger.WriteLineInfo("Number of items in the file: " + num_lines);
                logger.WriteLineInfo("Average dT [s]= " + dts.Average());
                logger.WriteLineInfo("Maximum dT [s]= " + dts.Max());
                logger.WriteLineInfo("Minimum dT [s]= " + dts.Min());
            }
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

        public Image GetSnapshot()
        {
            return frameBox.Image;
        }

        private void frameBox_MouseEnter(object sender, EventArgs e)
        {
            frameBox.Focus();
            if (windowState == StreamWindowState.DigitizePoints)
            {
                frameBox.Cursor = Cursors.Cross;
            }
        }

        public void SetHotFrame(HotFrame frame)
        {
            SetFrameByTime(frame.Timestamp);
            CurrentHotFrame = frame;
            frameBox.Refresh();

            ImageFeature imageFeature = frame.GetImageFeatureByDataStream(dataStream);
            imageFeature.Image = frameBox.Image;
            imageFeature.TimeStamp = currentDataLine.TimeStamp;

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (this.windowState == StreamWindowState.Normal)
            {
                if (CurrentHotFrame == null)
                {
                    MessageBox.Show("No Hot Frame selected! Use frame window to create a new hot frame, and select it!", "No Hot Frame selected!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.windowState = StreamWindowState.DigitizePoints;
                btnDigitize1.Text = "ND (F1)";
                frameBox.Cursor = Cursors.Cross;

            }
            else if (this.windowState == StreamWindowState.DigitizePoints)
            {
                this.windowState = StreamWindowState.Normal;
                frameBox.Cursor = Cursors.Default;
                btnDigitize1.Text = "D (F1)";
                frameBox.Cursor = Cursors.Default;
            }
        }

        private void frameBox_Click(object sender, EventArgs e)
        {
            
        }

        private void frameBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.windowState == StreamWindowState.DigitizePoints)
            {
                Point pt = frameBox.PointToImage(e.Location);
                lblStatus.Text = "(X=" + pt.X + ", Y=" + pt.Y + ")";
            }
        }

        private void frameBox_Paint(object sender, PaintEventArgs e)
        {
            int markerSize = 5;
            // put digitized points on the images
            if (CurrentHotFrame != null)
            {
                Feature features = CurrentHotFrame.Features.Find(x => x.DataStream == dataStream);
                if (features != null)
                {
                    ImageFeature imageFeature = features as ImageFeature;
                    using (Pen pen = new Pen(new SolidBrush(Color.Red), 1))
                    {
                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
                        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();

                        foreach (ImagePoint pt in imageFeature.Points)
                        {
                            Point pti = frameBox.GetOffsetPoint(pt.GetPoint());
                            //e.Graphics.DrawLine(pen, new Point(pti.X, pti.Y), new Point(pti.X+10, pti.Y));
                            e.Graphics.DrawLine(pen, pti.X - markerSize, pti.Y, pti.X + markerSize, pti.Y);
                            e.Graphics.DrawLine(pen, pti.X, pti.Y - markerSize, pti.X, pti.Y + markerSize);
                            e.Graphics.DrawString(pt.ID.ToString(), drawFont, drawBrush, pti.X + markerSize/2, pti.Y + markerSize/2, drawFormat);
                        }
                    }
                }
            }
        }

        private void frameBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.windowState == StreamWindowState.DigitizePoints)
            {

                if (e.Button == MouseButtons.Left) // add point
                {
                    if (CurrentHotFrame == null) return;

                    ImageFeature imageFeature = (ImageFeature)CurrentHotFrame.GetImageFeatureByDataStream(dataStream);
                    Point pt = frameBox.PointToImage(e.X, e.Y);
                    ImagePoint imagePoint = new ImagePoint(++lastId, pt.X, pt.Y);
                    PropertiesWnd wnd = new PropertiesWnd(imagePoint);
                    if (wnd.ShowDialog() == DialogResult.OK)
                    {
                        ImagePoint pti = wnd.Object as ImagePoint;
                        lastId = pti.ID;
                        imageFeature.Points.Add(pti);
                    }
                    frameBox.Refresh();
                }

                if (e.Button == MouseButtons.Right) // remove point
                {
                    ImageFeature imageFeature = (ImageFeature)CurrentHotFrame.GetImageFeatureByDataStream(dataStream);
                    Point pt = frameBox.PointToImage(e.X, e.Y);
                    /*double minVal = imageFeature.Points.Min(x => Math.Pow(x.X - pt.X, 2) + Math.Pow(x.Y - pt.Y, 2));
                    int minIndex = Array.IndexOf(imageFeature.Points, minVal);*/

                    ImagePoint minPt = null;
                    double minR = Double.MaxValue;
                    foreach(ImagePoint pti in imageFeature.Points)
                    {
                        double r = Math.Pow(pti.X - pt.X, 2) + Math.Pow(pti.Y - pt.Y, 2);
                        if (r < minR)
                        {
                            minPt = pti;
                            minR = r;
                        }
                    }

                    if (minPt != null)
                    {
                        PropertiesWnd wnd = new PropertiesWnd(minPt);
                        if ((wnd.ShowDialog() == DialogResult.OK) && (wnd.IsDeleteObject))
                        {
                            imageFeature.Points.Remove(minPt);
                        }
                    }
                    frameBox.Refresh();
                }
            }
        }

        private void frameBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void frameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                toolStripButton4_Click(sender, e);
            }
        }

        private void showDataLinePropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void showDataStreamPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataStream != null)
            {
                PropertiesWnd wnd = new PropertiesWnd(dataStream, false);
                if (wnd.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
        }

        private void showHotFramePropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentHotFrame != null)
            {
                PropertiesWnd wnd = new PropertiesWnd(CurrentHotFrame, false);
                if (wnd.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
        }

        private void tieToGlobalFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm form = ((MainForm)this.MdiParent);
            if (form.TimeLineWnd.CurrentHotFrame == null)
            {
                MessageBox.Show("Frame is not selected. Frame has to be selected to tie the current video time to the global time. Use frame window to select a frame!", "Cannot tie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HotFrame hotFrame = form.TimeLineWnd.CurrentHotFrame;

            // depricated
            /*DateTime? gtn = form.GlobalTime;
            if (gtn == null)
            {
                logger.WriteLineError("No global time is selected! Open the timeline window and select a time.");
                MessageBox.Show("No global time is selected! Open the timeline window nad select a time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }*/

            DateTime gt = hotFrame.Timestamp;
            TimeFeature timeFeautre = new TimeFeature(DataStream, currentDataLine, hotFrame);

            // remove features with the same datastrean
            hotFrame.Features.RemoveAll(x => (x is TimeFeature) && (x.DataStream == DataStream));
            hotFrame.Features.Add(timeFeautre);

            double delay = new TimeSpan(gt.Ticks - currentDataLine.TimeStamp.Ticks).TotalSeconds;
            dataStream.ApplyDelay(delay, TimeType.ESTIMATED);
                       

            SetFrameByTime(gt);
        }

        private void showDataLinePropertyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (currentDataLine != null)
            {
                PropertiesWnd wnd = new PropertiesWnd(currentDataLine, false);
                if (wnd.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            SetFrameByTime(currentDataLine.TimeStamp.AddSeconds(1));
            Refresh();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            SetFrameByTime(currentDataLine.TimeStamp.AddSeconds(-1));
            Refresh();

        }

        private void calculateFiletimeAndGlobalTimeDifferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double sum = 0;
            var vals = new List<double>();
            foreach (ImageDataLine dataLine in DataStream.DataLines)
            {
                var val = new TimeSpan(dataLine.TimeStamp.Ticks - dataLine.FileTimeStamp.Ticks).TotalSeconds;
                vals.Add(val);
                sum += val;
            }

            int n = DataStream.DataLines.Count();
            double avg = sum / n;

            sum = 0;
            foreach(var val in vals)
            {
                sum += Math.Pow(val - avg, 2);
            }
            double std = Math.Sqrt(sum / (n - 1));

            logger?.WriteLineInfo("Timestamp and file time difference for " + DataStream.ShortName);
            logger?.WriteLineInfo("Average: " + avg + " s");
            logger?.WriteLineInfo("Average: " + TimeSpan.FromSeconds(avg).ToString(@"hh\:mm\:ss\:fff") + " s");
            logger?.WriteLineInfo("Standard deviation: " + std+ " s");


        }

        private void syncManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void fromFileTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDataLine != null)
            {
                SyncToManuallyParams param = new SyncToManuallyParams();
                PropertiesWnd wnd = new PropertiesWnd(param, false);
                wnd.Text = "Parameters for manual sync";
                if (wnd.ShowDialog() == DialogResult.OK)
                {
                    double delay = param.Delay;
                    dataStream.ApplyDelayFromFileTime(delay, TimeType.ESTIMATED);
                }
            }
        }

        private void fromCurrentTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDataLine != null)
            {
                SyncToManuallyParams param = new SyncToManuallyParams();
                PropertiesWnd wnd = new PropertiesWnd(param, false);
                wnd.Text = "Parameters for manual sync";
                if (wnd.ShowDialog() == DialogResult.OK)
                {
                    double delay = param.Delay;
                    dataStream.ApplyDelay(delay, TimeType.ESTIMATED);
                }
            }
        }

        private void timeTagImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            String tFile = project.Folder + "\\" + Project.MetadataFolder + "\\" + this.DataStream.ShortName + "_T.txt";

            using (System.IO.StreamWriter file = new StreamWriter(tFile))
            {
                foreach (DataLine dataLine in this.DataStream.DataLines)
                {
                    ImageDataLine img = dataLine as ImageDataLine;                    
                    String line = Utils.ConvertToUnixTimestamp(img.TimeStamp).ToString("F4") + " " + img.ImageFileName + " " + img.TimeStamp.ToString("yyyy MM dd HH mm ss.fff");
                    file.WriteLine(line);                   
                }
            }

            MessageBox.Show("Timetags are exported, see: " + tFile, "Timetag exported", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }

    public class SyncToManuallyParams
    {
        public double Delay { get; set; }

        public SyncToManuallyParams()
        {
            Delay = 0;
        }

    }
}
