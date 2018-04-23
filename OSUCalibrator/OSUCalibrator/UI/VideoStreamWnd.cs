using Accord.Imaging.Filters;
using DotImaging;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class VideoStreamWnd : Form, IStreamWindow
    {
        private VideoDataStream dataStream;
        public DataStream DataStream { get { return dataStream; } }
        public HotFrame CurrentHotFrame { get; private set; }

        private Project project;
        private FileCapture reader;
        private ILogger logger;
        private StreamWindowState windowState;

        private VideoDataLine currentDataLine = null;
        private int currentFrame = -1;
        private int lastId = 0;

        public bool HasToSetFrame { get; private set; }

        public DateTime CurrentTime { get { return dataStream.StartTime.AddSeconds(((double)trackBar.Value / reader.FrameRate)); }  }

        public VideoStreamWnd(VideoDataStream dataStream, ILogger logger)
        {
            InitializeComponent();
            this.dataStream = dataStream;
            this.logger = logger;
            this.HasToSetFrame = true;
        }

        private void VideoStreamWnd_Shown(object sender, EventArgs e)
        {
            try
            {
                project = ((MainForm)this.MdiParent).Project;
                VideoDataLine startDataLine = (VideoDataLine)dataStream.DataLines[0];
                OpenVideo(startDataLine);
                trackBar.Minimum = 0;
                trackBar.Maximum = Convert.ToInt32(dataStream.Length * reader.FrameRate);

                SetFrameByTime(dataStream.StartTime);
                this.Text = "Video Stream: " + dataStream.ShortName;
                Refresh();

            }
            catch (Exception ex)
            {
                reader = null;
                logger.WriteLineError(ex.ToString());
                MessageBox.Show("Error during opening the file: " + ex.Message, "Error during opening video file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult result = MessageBox.Show("Do you want to reload stream for appropriate video format?", "Reloading stream!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.OK)
                {
                    dataStream.WriteMetadata(logger);
                    MessageBox.Show("Stream reloaded! Open stream again!", "Open stream again!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                this.Close();
            }
        }

        private void VideoWnd_Load(object sender, EventArgs e)
        {
           
        }

        private void updateFileList()
        {
            listFiles.Items.Clear();
            foreach (DataLine dataLine in dataStream.DataLines)
            {
                VideoDataLine videoDataLine = dataLine as VideoDataLine;
                listFiles.Items.Add(videoDataLine);
            }

            listFiles.Refresh();
        }

        private void OpenVideo(VideoDataLine dataLine)
        {
            if (this.currentDataLine != null)
            {
                reader.Close();
            }

            try
            {                
                reader = new FileCapture(project.Folder + "\\" + dataStream.SubFolder + "\\" + dataLine.VideoFileName);
                reader.Open();
                timer.Interval = Convert.ToInt32((1 / reader.FrameRate) * 1000);
                currentDataLine = dataLine;
                lblStatus.Text += "Current file: " + dataLine.VideoFileName;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                logger.WriteLineError("Error occured: " + ex.Message);
                throw ex;
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
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
            TimeFeature timeFeautre = new TimeFeature(DataStream, currentDataLine, CurrentTime, hotFrame);

            // remove features with the same datastrean
            hotFrame.Features.RemoveAll(x => (x is TimeFeature) && (x.DataStream == DataStream) );
            hotFrame.Features.Add(timeFeautre);

            double delay = new TimeSpan(gt.Ticks - CurrentTime.Ticks).TotalSeconds;
            dataStream.ApplyDelay(delay, TimeType.ESTIMATED);
            SetFrameByTime(gt);

        }

        private void frameTimer_Tick(object sender, EventArgs e)
        {
            NextFrame();
        }

        public void NextFrame()
        {
            /*if (currentFrame + 1 >= DataStream.DataLines.Count)
            {
                timer.Stop();
                return;
            }*/

            SetFrame(currentFrame + 1);
            lblStatus.Text = "";

        }

        private void SetFrame(int frame)
        {
            CurrentHotFrame = null;

            if (frame >= 0)
            {
                if (frame != currentFrame)
                {
                    if (reader == null)
                    {
                        timer.Stop();
                        return;
                    }

                    // has to seek?
                    if (currentFrame+1 != frame)
                    {
                        reader.Seek(frame, System.IO.SeekOrigin.Begin);
                    }

                    if (frameBox.Image != null) frameBox.Image.Dispose();
                    reader.ReadTimeout = timer.Interval;
                    Image<Bgr<byte>> img = reader.ReadAs<Bgr<byte>>();
                    //ResizeNearestNeighbor filter = new ResizeNearestNeighbor(400, 300);
                    //Bitmap newImage = filter.Apply(img.ToBitmap());

                    if (img != null)
                    {
                        frameBox.Image = img.ToBitmap();
                        frameBox.ZoomToFit();
                    }

                    currentFrame = frame;
                }
            }
            else
            {
                frameBox.Image = null;
                if (frame < 0)
                {
                    currentFrame = 0;
                    trackBar.Value = 0;
                }
                toolStripStatus.Text = "Outside of the stream time limits.";
            }
        }

        public void SetFrameByTime(DateTime time)
        {
            lblStatus.Text = "Req.: " + time.ToString("yyyy-MM-dd HH:mm:ss.fff");

            VideoDataLine bestDataLine = null;            
            foreach (var dataLine in dataStream.DataLines)
            {
                VideoDataLine videoDataLine = dataLine as VideoDataLine;
                if ((time >= dataLine.TimeStamp) && (time < dataLine.TimeStamp.Add(videoDataLine.Length)))
                {
                    bestDataLine = videoDataLine;
                }
            }

            if (bestDataLine != null)
            {
                lblStatus.Text += "File: "  + bestDataLine.VideoFileName;

                if (currentDataLine != bestDataLine)
                {
                    OpenVideo(bestDataLine);
                    currentFrame = -1;
                }

                double dt = new TimeSpan((time.Ticks - bestDataLine.TimeStamp.Ticks)).TotalSeconds;
                int frame = Convert.ToInt32(dt * reader.FrameRate);
                SetFrame(frame);
            }
            else
            {
                frameBox.Image = null;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            trackBar.Value = Convert.ToInt32((new TimeSpan(currentDataLine.TimeStamp.Ticks - dataStream.StartTime.Ticks)).TotalSeconds * reader.FrameRate + currentFrame);
            updateFileList();
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {          
            timer.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void VideoWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            if (frameBox.Image != null) frameBox.Image.Dispose();

            if (reader != null)
            {
                reader.Close();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {            
            SetFrameByTime(this.CurrentTime);
            updateFileList();
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
        }

        public void SetHotFrame(HotFrame frame)
        {
            SetFrameByTime(frame.Timestamp);
            CurrentHotFrame = frame;
            this.Refresh();

            ImageFeature imageFeature = frame.GetImageFeatureByDataStream(dataStream);
            imageFeature.Image = frameBox.Image;
            imageFeature.TimeStamp = frame.Timestamp;
        }

        private void listFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            VideoDataLine videoDataLine = listFiles.Items[e.Index] as VideoDataLine;
            e.DrawBackground();
            Graphics g = e.Graphics;

            if ((currentDataLine != null) && (currentDataLine == videoDataLine))
            {
                g.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                g.DrawString(videoDataLine.ToString(), e.Font, new SolidBrush(Color.Blue), new PointF(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                g.DrawString(videoDataLine.ToString(), e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            dataStream.FixTimestampsByOrder(logger);
            Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            var idx = listFiles.SelectedIndex;
            if (idx < 0) return;

            // swap data lines
            DataLine prevDataLine = dataStream.DataLines[idx - 1];
            dataStream.DataLines[idx - 1] = dataStream.DataLines[idx];
            dataStream.DataLines[idx] = prevDataLine;
            dataStream.FixTimestampsByOrder();
            Refresh();
            listFiles.SelectedIndex = idx - 1;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            var idx = listFiles.SelectedIndex;
            if (idx == listFiles.Items.Count - 1) return;

            // swap data lines
            DataLine prevDataLine = dataStream.DataLines[idx + 1];
            dataStream.DataLines[idx + 1] = dataStream.DataLines[idx];
            dataStream.DataLines[idx] = prevDataLine;
            dataStream.FixTimestampsByOrder();
            Refresh();
            listFiles.SelectedIndex = idx + 1;
        }

        private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listFiles_DoubleClick(object sender, EventArgs e)
        {
            var idx = listFiles.SelectedItem as VideoDataLine;
            if (idx != null)
            {
                SetFrameByTime(idx.TimeStamp);
                Refresh();
            }
        }

        private void showCurrentDataLineToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void showCurrentStreamToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void reloadDataStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataStream.WriteMetadata(logger);
        }

        private void btnDigitize1_Click(object sender, EventArgs e)
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

        private void VideoStreamWnd_MouseMove(object sender, MouseEventArgs e)
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
                List<Feature> features = CurrentHotFrame.Features.FindAll(x => x.DataStream == dataStream);
                if (features != null)
                {
                    foreach (Feature feature in features)
                    {
                        if (feature is ImageFeature)
                        {
                            ImageFeature imageFeature = feature as ImageFeature;
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
                                    e.Graphics.DrawString(pt.ID.ToString(), drawFont, drawBrush, pti.X + markerSize / 2, pti.Y + markerSize / 2, drawFormat);
                                }
                            }
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
                    foreach (ImagePoint pti in imageFeature.Points)
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

        private void frameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnDigitize1_Click(sender, e);
            }
        }

        private void frameBox_MouseEnter_1(object sender, EventArgs e)
        {
            frameBox.Focus();
            if (windowState == StreamWindowState.DigitizePoints)
            {
                frameBox.Cursor = Cursors.Cross;
            }
        }

        private void frameBox_Click(object sender, EventArgs e)
        {

        }

        private void importDigitizedPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream stream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog1.OpenFile()) != null)
                    {

                        using (TextReader reader = new StreamReader(stream))
                        {
                            logger.WriteLineInfo(" ");
                            logger.WriteLineInfo("Reading image points from " + openFileDialog1.FileName);
                            ImageFeature imageFeature = (ImageFeature)CurrentHotFrame.GetImageFeatureByDataStream(dataStream);

                            String line;
                            int pointNum = 0;
                            while ((line = reader.ReadLine()) != null)
                            {
                                String[] lineSplit = line.Split(',');

                                if (lineSplit.Length < 2)
                                {
                                    MessageBox.Show("Wrong text format!", "Wrong Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                int id = Convert.ToInt32(lineSplit[0]);
                                double x = Convert.ToDouble(lineSplit[1]);
                                double y = Convert.ToDouble(lineSplit[2]);

                                ImagePoint imagePoint = new ImagePoint(id, (int)x, (int)y);
                                logger.WriteLineInfo("Point #" + id + " x= " + x + " y= " + y);

                                imageFeature.Points.Add(imagePoint);
                                pointNum++;
                            }

                            frameBox.Refresh();
                            logger.WriteLineInfo("# of imported points: " + pointNum);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
