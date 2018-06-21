using Accord;
using Accord.Controls;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Math;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Loggers;
using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class MainForm : Form
    {
        public ConsoleWnd Console { get; private set; }
        public DataStreamWnd DataStreamWnd { get; private set; }
        public TimeLineForm TimeLineWnd { get; private set; }
        public FrameWnd FrameWnd { get; private set; }

        public Project Project { get; private set; }
        public ConsoleWndLogger ConsoleLogger { get; private set; }       

        private BackgroundWorker yuvConverterWorker;
        //private String projectFolder = @"D:\CAR\2017_08_23_1"; // calibration 
        //private String projectFolder = @"D:\CAR\2017_08_21"; // calibration 
        private String projectFolder = @"E:\PN\2018_06_20_PN"; // calibration 

        public DateTime? GlobalTime { get { return TimeLineWnd != null ? (DateTime?)TimeLineWnd.CurrentTime : null;  } }

        public MainForm()
        {
            InitializeComponent();

            ConsoleLogger = ConsoleWndLogger.Create(this);

            Project = new Project(projectFolder);
            Project.StandardInit();

            yuvConverterWorker = new BackgroundWorker();
            yuvConverterWorker.WorkerReportsProgress = true;
            yuvConverterWorker.WorkerSupportsCancellation = true;
            yuvConverterWorker.DoWork += YUVConverterWorker_DoWork;
        }        

        private void extractFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(ConsoleLogger);

            if (!Directory.Exists(Project.Folder + "\\" + Project.MetadataFolder))
            {
                Directory.CreateDirectory(Project.Folder + "\\" + Project.MetadataFolder);
            }
            var fileLogger = FileLogger.Create(Project.Folder + "\\" + Project.MetadataFolder + "\\metadata.txt", Project.Folder + "\\" + Project.MetadataFolder + "\\warnings.txt");
            loggers.Add(fileLogger);

            MainForm form = (MainForm)this.MdiParent;
            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Loading project...";
            wnd.ProgressBarStyle = ProgressBarStyle.Blocks;
            loggers.Add(wnd);

            MultipleLogger logger = new MultipleLogger(loggers);

            wnd.WriteLine("Start loading project: " + projectFolder);
            toolStripProgressBar.Visible = true;

            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                try
                {
                    MetadataBuilder.Create(Project, new MultipleLogger(loggers), wnd.CancelTokenSource.Token);
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception ex)
                {
                    logger.WriteLineError("Error occured: " + ex.Message);
                }
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                toolStripProgressBar.Visible = false;
                fileLogger.Flush();
                fileLogger.Dispose();
            };

            wnd.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
        }        


        private async void exportToMATLABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripProgressBar.Visible = true;
            await MatlabExporter.Export(Project.Folder + "\\Metadata\\matlab_export.mat", this.Project, ConsoleLogger);
            toolStripProgressBar.Visible = false;
        }        

        public void ReportProgress(double pcnt)
        {
            if (InvokeRequired)
            {
                if (pcnt != 100)
                {
                    this.BeginInvoke((Action)(() => toolStripProgressBar.Visible = true));
                }
                else
                {
                    this.BeginInvoke((Action)(() => toolStripProgressBar.Visible = false));
                }

                this.BeginInvoke(new Action<double>(ReportProgress), new object[] { pcnt });
                return;
            }
            toolStripProgressBar.Value = (int)pcnt;

            if (pcnt != 100)
            {
                toolStripProgressBar.Visible = true;
            }
            else
            {
                toolStripProgressBar.Visible = false;
            }

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void velodyneTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region Loading and saving project

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project.Save();
            ConsoleLogger.WriteLineInfo("Project has been saved!");
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog();
            DialogResult result = fdb.ShowDialog();
            if (result != DialogResult.OK) return;

            projectFolder = fdb.SelectedPath;
            OpenProject(projectFolder);
        }

        public void OpenProject(String projectFolder)
        {
            MainForm form = (MainForm)this.MdiParent;
            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Loading project...";
            wnd.ProgressBarStyle = ProgressBarStyle.Marquee;

            // create multiple logger
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(ConsoleLogger);
            loggers.Add(wnd);
            MultipleLogger logger = new MultipleLogger(loggers);

            logger.WriteLineInfo("Start loading project: " + projectFolder);

            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                try
                {
                    Project = Project.Load(projectFolder, logger);
                    Properties.Settings.Default["RecentlyOpenProject"] = projectFolder;
                    Properties.Settings.Default.Save();
                }
                catch (OperationCanceledException)
                {
                    logger.WriteLineInfo("Cancelled!");
                    eWorker.Cancel = true;
                }
                catch (Exception ex)
                {
                    logger.WriteLineInfo("Error occured: " + ex.Message);
                    eWorker.Cancel = true;
                }
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                if (!eWorker.Cancelled)
                {
                    logger.WriteLineInfo("Project has been loaded!");
                    logger.WriteLineInfo("Project folder: " + this.Project.Folder);
                    logger.WriteLineInfo("No. of data streams: " + this.Project.DataStreams.Count());
                    MetadataBuilder.PrintSummary(this.Project, ConsoleLogger);
                    if (DataStreamWnd != null) DataStreamWnd.UpdateUI();
                    form?.ReportProgress(100);
                    wnd.WriteProgress(100);
                    wnd.Close();
                }
            };

            wnd.ShowDialog();
        }

        #endregion

        #region Single windows

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ShowTimeLineWnd();
        }

        public void ShowTimeLineWnd()
        {
            if ((TimeLineWnd == null) || (TimeLineWnd.IsDisposed))
            {
                TimeLineWnd = new TimeLineForm(this.Project);
                TimeLineWnd.MdiParent = this;
                TimeLineWnd.Show();
            }
            else
            {
                TimeLineWnd.Focus();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowConsoleWnd();
        }

        public void ShowConsoleWnd()
        {
            if ((Console == null) || (Console.IsDisposed))
            {
                Console = new ConsoleWnd();
                Console.Write(ConsoleLogger.ConsoleText.ToString());
                Console.MdiParent = this;
                Console.Show();
            }
            else
            {
                Console.Focus();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ShowDataStreamWnd();
        }

        public void ShowDataStreamWnd()
        {
            if ((DataStreamWnd == null) || (DataStreamWnd.IsDisposed))
            {
                DataStreamWnd = new DataStreamWnd(this.ConsoleLogger);
                DataStreamWnd.MdiParent = this;
                DataStreamWnd.Show();
            }
            else
            {
                DataStreamWnd.Focus();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ShowFrameWindow();
        }

        public void ShowFrameWindow()
        {
            if ((FrameWnd == null) || (FrameWnd.IsDisposed))
            {
                FrameWnd = new FrameWnd();
                FrameWnd.MdiParent = this;
                FrameWnd.Show();
            }
            else
            {
                FrameWnd.Focus();
            }
        }

        #endregion

        public enum LiDARTransformType
        {
            None,
            TransformToPlatform,
            TransformToGlobal
        }

        public void SampleLiDAR(DateTime? timeStampIn = null, LiDARTransformType type = LiDARTransformType.None)
        {
            foreach (Form form in this.MdiChildren)
            {
                if ((!form.IsDisposed) && (form is VelodyneStreamWnd))
                {

                    VelodyneStreamWnd veloForm = form as VelodyneStreamWnd;

                    DateTime timeStamp;
                    if (timeStampIn != null)
                    {
                        veloForm.SetFrameByTime(timeStampIn.Value);
                        timeStamp = timeStampIn.Value;
                    }
                    else
                    {
                        veloForm.NextFrame();
                        timeStamp = veloForm.GetTime();
                    }

                    List<VelodynePoint> pts = veloForm.GetPoints();
                    if (pts.Count() == 0) continue;
                    double[,] ptsMat = Matrix.Create<double>(4, pts.Count(), 0);

                    for (int i = 0; i < pts.Count(); i++)
                    {
                        ptsMat[0, i] = pts[i].X;
                        ptsMat[1, i] = pts[i].Y;
                        ptsMat[2, i] = pts[i].Z;
                        ptsMat[3, i] = 1;
                    }

                    var veloStream = veloForm.DataStream as VelodyneDataStream;
                    if ((veloStream.Tp == null) && (type != LiDARTransformType.None))
                    {
                        this.ConsoleLogger.WriteLineWarning("Transfromation matrix for " + veloStream.ShortName + " is not avaialble!");
                        continue;
                    }

                    double[,] convPts = null;

                    if (type == LiDARTransformType.None)
                    {
                        convPts = ptsMat;
                    }
                    else if (type == LiDARTransformType.TransformToPlatform)
                    {
                        convPts = veloStream.Tp.Dot(ptsMat);
                    }
                    else if (type == LiDARTransformType.TransformToGlobal)
                    {
                        throw new NotImplementedException();
                    }
                    
                    var hotFrameSubfix = timeStamp.ToString("HHmmssfff");
                    var saveTo = this.Project.Folder + "\\" + Project.LiDARFrameFolder + "\\LiDAR_" + hotFrameSubfix + "_" + veloStream.ShortName + ".txt";

                    TextWriter tw = new StreamWriter(saveTo);
                    for (int i = 0; i < convPts.Columns(); i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tw.Write(convPts[j, i] + " ");
                        }
                        tw.Write(pts[i].Intensity);
                        tw.WriteLine();
                    }
                    tw.Close();
                }
            }
        }

        #region Old stuffs

        private void convertYUVFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // start working thread
            if (yuvConverterWorker.IsBusy != true)
            {
                // Start the asynchronous operation.
                yuvConverterWorker.RunWorkerAsync();
            }
        }

        private void openYUVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Project.Folder;
            openFileDialog.Filter = "txt files (*.yuv)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String filePath = openFileDialog.FileName;
                String path = Path.GetDirectoryName(filePath);
                String filename = Path.GetFileNameWithoutExtension(filePath);
                String dstFile = path + "\\" + filename + ".bmp";

                toolStripProgressBar.Visible = true;
                BackgroundWorker worker = new BackgroundWorker();
                Bitmap result = null;
                worker.DoWork += delegate (object bgworker, DoWorkEventArgs bge)
                {
                    result = YUVConverter.Convert(filePath, dstFile, ConsoleLogger);
                };

                worker.RunWorkerCompleted += delegate (object bgworker, RunWorkerCompletedEventArgs bge)
                {
                    toolStripProgressBar.Visible = false;
                    if (result != null)
                    {
                        SingleImageWnd form = new SingleImageWnd(result);
                        form.MdiParent = this;
                        form.Show();
                    }
                };

                worker.RunWorkerAsync();
            }
        }

        private void YUVConverterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            String sourceFolder = @"E:\CAR\2017_08_28_1\Cameras\PTGREY";
            String dstFolder = @"E:\CAR\2017_08_28_1\Cameras\PTGREY";

            //string[] files = Directory.GetFiles(folder, "*.yuv", SearchOption.AllDirectories).OrderBy(f => f.LastWriteTime);
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceFolder);
            var files = directoryInfo.GetFiles("*.yuv", SearchOption.AllDirectories).OrderBy(t => t.LastWriteTime).ToList();

            int[] fileIds = new int[100];
            for (int i = 0; i < 100; i++) fileIds[i] = 1;

            int fi = 0;
            foreach (FileInfo file in files)
            {
                ConsoleLogger.WriteLineInfo(" ");
                int camId = System.Convert.ToInt32(file.Name.Substring(1, 1));

                string dstFolderFile = dstFolder + "\\PTGREY" + camId;
                if (!Directory.Exists(dstFolderFile))
                {
                    Directory.CreateDirectory(dstFolderFile);
                }

                String dstFile = dstFolderFile + "\\A" + camId + "-" + fileIds[camId].ToString().PadLeft(5, '0');

                if (File.Exists(dstFile + ".bmp"))
                {
                    ConsoleLogger.WriteLineInfo("" + dstFile + " [(" + fi + "/" + files.Count() + ")" + ((double)fi / (double)files.Count() * 100.0).ToString("0.0") + "%] FILE EXISTS. SKIP.");
                    Thread.Sleep(100);
                }
                else
                {
                    ConsoleLogger.WriteLineInfo("" + dstFile + " [(" + fi + "/" + files.Count() + ")" + ((double)fi / (double)files.Count() * 100.0).ToString("0.0") + "%]");
                    Bitmap bmp = YUVConverter.Convert(file.FullName, dstFile, ConsoleLogger);
                    bmp.Dispose();

                }

                fileIds[camId]++;
                fi++;


            }
        }

        private void cornerDetectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(System.Drawing.Image.FromFile(@"C:\OSU3\UAS\Training Videos\ZoltanTestSets\test\DJI_0247.rd.JPG"));

            float threshold = 0.001f;
            /*int octaves = (int)numOctaves.Value;
            int initial = (int)numInitial.Value;*/

            ResizeNearestNeighbor filter = new ResizeNearestNeighbor(600, 400);
            image = filter.Apply(image);

            // Create a new SURF Features Detector using the given parameters
            SpeededUpRobustFeaturesDetector surf =
                new SpeededUpRobustFeaturesDetector(threshold);

            List<SpeededUpRobustFeaturePoint> points = surf.ProcessImage(image);

            // Create a new AForge's Corner Marker Filter
            FeaturesMarker features = new FeaturesMarker(points, 5);

            // Apply the filter and display it on a picturebox
            Bitmap image_feature = features.Apply(image);


            SingleImageWnd form = new SingleImageWnd(image_feature);
            form.MdiParent = this;
            form.Show();

            /*Bitmap image = new Bitmap(System.Drawing.Image.FromFile(@"C:\OSU3\CAR\Reports\Targets\Target1\DSC00217.JPG"));
            Bitmap image2 = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(image2))
            {
                g.DrawImage(image, 0, 0);

                Grayscale filter = new Grayscale(.33, .33, .33);
                image = filter.Apply(image);

                double sigma = 2;
                float k = 4 / 10;
                float threshold = 500;

                // Create a new Harris Corners Detector using the given parameters
                HarrisCornersDetector harris = new HarrisCornersDetector(k)
                {
                    Measure = HarrisCornerMeasure.Harris, //: HarrisCornerMeasure.Noble,
                    Threshold = threshold,
                    Sigma = sigma
                };

                List<IntPoint> corners = harris.ProcessImage(image);

                foreach (IntPoint corner in corners)
                {
                    //A circle with Red Color and 2 Pixel wide line
                    //gf.DrawEllipse(new Pen(Color.Red, 2), new Rectangle(0, 0, 200, 200));
                    DrawCircle(g, new Pen(Color.Red, 1), corner.X, corner.Y, 1);
                }

            }

            ImageForm form = new ImageForm(image2);
            form.MdiParent = this;
            form.Show();*/
        }

        public static void DrawCircle(Graphics g, Pen pen,
                                 float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        #endregion

        private void updateFileTimesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(ConsoleLogger);

            MainForm form = (MainForm)this.MdiParent;
            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Updating file times...";
            loggers.Add(wnd);

            MultipleLogger logger = new MultipleLogger(loggers);

            wnd.WriteLine("Start updating file times: " + projectFolder);
            toolStripProgressBar.Visible = true;

            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                try
                {
                    int k = 0;
                    foreach(DataStream stream in Project.DataStreams)
                    {
                        wnd.CancelTokenSource.Token.ThrowIfCancellationRequested();

                        logger.WriteProgress((++k / (double)Project.DataStreams.Count()) * 100.0);
                        if (stream is ImageDataStream)
                        {
                            logger.WriteLineInfo("Image data stream: " + stream.ShortName);
                            ImageDataStream imageDataStream = stream as ImageDataStream;
                            imageDataStream.UpdateFileTimes();
                        }

                        if (stream is VideoDataStream)
                        {
                            logger.WriteLineInfo("Image data stream: " + stream.ShortName);
                            VideoDataStream videoDataStream = stream as VideoDataStream;
                            videoDataStream.UpdateFileTimes();
                        }

                    }
                }
                catch (OperationCanceledException)
                {
                    logger.WriteLineWarning("User cancelled!");
                    return;
                }
                catch (Exception ex)
                {
                    logger.WriteLineError("Error occured: " + ex.Message);
                }

                logger.WriteLineInfo("done.");
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                toolStripProgressBar.Visible = false;
            };

            wnd.ShowDialog();
        }

        private void sampleLiDARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TimeLineWnd == null)
            {
                MessageBox.Show("Frame is not selected! Open the TimeLine window and select a frame.", "Frame is not selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(ConsoleLogger);

            MainForm form = (MainForm)this.MdiParent;
            ProgressBarWnd wnd = new ProgressBarWnd();
            wnd.Text = "Export LiDAR frames...";
            loggers.Add(wnd);

            MultipleLogger logger = new MultipleLogger(loggers);

            wnd.WriteLine("Export LiDAR frames..");
            toolStripProgressBar.Visible = true;
            
            DateTime startTime = TimeLineWnd.CurrentTime;
            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);

            wnd.Worker.DoWork += delegate (object senderWorker, DoWorkEventArgs eWorker)
            {
                try
                {
                    for (int k = 0; k < 1000; k++)
                    {
                        wnd.CancelTokenSource.Token.ThrowIfCancellationRequested();
                        //DateTime currentTime = startTime.AddSeconds(k);
                        //this.SampleLiDAR(currentTime, LiDARTransformType.None);
                        this.SampleLiDAR();
                        //wnd.WriteLine("Time: " + currentTime.ToString("yyyy-MM-hh HH:mm:ss.fff"));
                        Thread.Sleep(100);
                    }
                }
                catch (OperationCanceledException)
                {
                    logger.WriteLineWarning("User cancelled!");
                    return;
                }
                catch (Exception ex)
                {
                    logger.WriteLineError("Error occured: " + ex.Message);
                }

                logger.WriteLineInfo("done.");
            };

            wnd.Worker.RunWorkerCompleted += delegate (object senderWorker, RunWorkerCompletedEventArgs eWorker)
            {
                toolStripProgressBar.Visible = false;
            };

            wnd.ShowDialog();
        }

        private void loadPrevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void loadPrevToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (Properties.Settings.Default["RecentlyOpenProject"] != null)
            {
                string lastProject = (string)Properties.Settings.Default["RecentlyOpenProject"];
                ToolStripMenuItem recentProjectMenu = new ToolStripMenuItem(lastProject, null, null, "recentlyOpenedProject");
                recentProjectMenu.Click += RecentProjectMenu_Click;
                loadPrevToolStripMenuItem.DropDownItems.Clear();
                loadPrevToolStripMenuItem.DropDownItems.Add(recentProjectMenu);
            }
        }

        private void RecentProjectMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem recentProjectMenu = (ToolStripMenuItem)sender;
            OpenProject(recentProjectMenu.Text);
        }
    }
}
