using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SharpVelodyne
{
    public enum SimpleVelodyneViewerRenderingMode
    {
        Responsive, 
        Manual
    }

    public enum SimpleVelodyneViewerColorMap
    {
        Default,
        Selection
    }

    public partial class SimpleVelodyneViewer : UserControl
    {
        #region ViewPoint class
        // proxy class for VelodynePoints
        private class ViewPoint
        {
            public double ViewX { get; set; }
            public double ViewY { get; set; }
            public bool IsSelected { get; set; }
            public PointLikeObject PointObject { get; private set; }
            public SimpleVelodyneViewerColorMap ColorMap { get; set; }

            public ViewPoint(PointLikeObject point) : this(point, 0, 0)
            {
              
            }

            public ViewPoint(PointLikeObject point, SimpleVelodyneViewerColorMap colorMap) : this(point, 0, 0, colorMap)
            {

            }

            public ViewPoint(PointLikeObject point, double x, double y) : this(point, x, y, SimpleVelodyneViewerColorMap.Default)
            {
 
            }

            public ViewPoint(PointLikeObject point, double x, double y, SimpleVelodyneViewerColorMap colorMap)
            {
                this.ViewX = x;
                this.ViewY = y;
                this.PointObject = point;
                this.IsSelected = false;
                this.ColorMap = colorMap;
            }

        }
        #endregion

        #region Camera class
        private class Camera
        {
            public double f;
            public double cx;
            public double cy;

            public double x;
            public double y;
            public double z;
            public double omega;
            public double phi;
            public double kappa;

            public Camera()
            {
                this.f = 1000;
                this.cx = 500;
                this.cy = 500;

                this.x = 0;
                this.y = 0;
                this.z = 100;
                this.omega = 0;
                this.phi = 0;
                this.kappa = 0;
            }
        }
        #endregion

        #region Objec transformation class
        private class ObjectTransfromation
        {
            public double alpha;
            public double beta;
            public double gamma;
            public double kappa;
            public double dx;
            public double dy;
            public double dz;

            public ObjectTransfromation()
            {
                Reset();
            }

            public void Reset()
            {
                alpha = 0;
                beta = 0;
                gamma = 0;
                kappa = Math.PI / 2.0;
                dx = 0;
                dy = 0;
                dz = 0;
            }
        }
        #endregion

        // Events
        public delegate void AnnotationClickedEventHandler(object sender, EventArgs e);
        public event AnnotationClickedEventHandler AnnotationClicked;

        // public properties and variables to control the control from outside

        public SimpleVelodyneViewerRenderingMode RenderingMode { get; set; }
        /// <summary>
        /// Maximum no. of points to be rendered for the preview 
        /// </summary>
        public long PointNumForPreview { get; set; }

        /// <summary>
        /// Default size of the points
        /// </summary>
        public int PointSize { get; set; }

        /// <summary>
        /// Image of the viewport
        /// </summary>
        public Image Image { get { return viewPort.Image; } }

        /// <summary>
        /// Maximum distance for snapping 
        /// </summary>
        public double MaximumSnapRadius { get; set; }

        // Private variables for visualization
        private List<ViewPoint> viewPoints { get; set; }
        private ObjectTransfromation cloudTransform;
        private Camera cam = new Camera();
        
        // Obejcts for multi-thread visualization
        private BackgroundWorker worker = null;
        private AutoResetEvent workerDone = new AutoResetEvent(false);

        // Some variables for maintaing window states
        private Point mPosition;
        private bool mDown = false;
        private Point selectStart = default(Point);
        private Point selectEnd = default(Point);
        private DateTime lastRenderCall = DateTime.MinValue;
        private bool initDone = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SimpleVelodyneViewer()
        {
            InitializeComponent();

            viewPoints = new List<ViewPoint>();
            cloudTransform = new ObjectTransfromation();
            viewPort.MouseWheel += PictureBox_MouseWheel;
            RenderingMode = SimpleVelodyneViewerRenderingMode.Responsive;

            PointNumForPreview = 10000;
            MaximumSnapRadius = 20;

            ResetView(SimpleVelodyneViewerRenderingMode.Manual);

            // populate help text
            lblHelpText.Text = "Help" + Environment.NewLine;
            lblHelpText.Text += "Rotate     : Left Mouse button" + Environment.NewLine;
            lblHelpText.Text += "Z rotation : SHIFT + Mouse wheel" + Environment.NewLine;
            lblHelpText.Text += "Zoom       : Mouse wheel" + Environment.NewLine;
            lblHelpText.Text += "Move       : CONTROL + Left mouse button" + Environment.NewLine;
            lblHelpText.Text += "Point size : CONTROL + Mouse wheel" + Environment.NewLine;
            lblHelpText.Text += "Reset view : Double left click" + Environment.NewLine;
            lblHelpText.Text += "Select pts : SHIFT + Left Mouse button + Select" + Environment.NewLine;
            lblHelpText.Text += "Reset sele : SHIFT + Double left click" + Environment.NewLine;

        }

        /// <summary>
        /// Constructor with velodyne point initializer
        /// </summary>
        /// <param name="points">Collection of VelodynePoint</param>
        public SimpleVelodyneViewer(IEnumerable<VelodynePoint> points) : this()
        {
            ClearAndAddNewPoints(points, SimpleVelodyneViewerRenderingMode.Manual);
        }

        // Control loading...
        private void SimpleVelodyneViewer_Load(object sender, EventArgs e)
        {
            initDone = true;
        }

        #region Public functions

        /// <summary>
        /// Add points to the view with removing the previous points
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        public void ClearAndAddNewPoints(IEnumerable<VelodynePoint> points, SimpleVelodyneViewerColorMap colorMap = SimpleVelodyneViewerColorMap.Default)
        {
            ClearAndAddNewPoints(points, this.RenderingMode, colorMap);
        }

        /// <summary>
        /// Add points to the view with removing the previous points, viewport will be rendered
        /// </summary>
        /// <param name="points">Collection of VelodynePoint</param>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void ClearAndAddNewPoints(IEnumerable<VelodynePoint> points, SimpleVelodyneViewerRenderingMode renderingMode, SimpleVelodyneViewerColorMap colorMap = SimpleVelodyneViewerColorMap.Default)
        {
            this.Clear();
            AddNewPoints(points, renderingMode, colorMap);
        }

        /// <summary>
        /// Clear points from viewer
        /// </summary>
        public void Clear()
        {
            this.ClearSelection();
            this.viewPoints.Clear();
        }

        /// <summary>
        /// Reset the view port and render
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        public void ResetView()
        {
            ResetView(this.RenderingMode);
        }

        /// <summary>
        /// Reset the view port and render
        /// </summary>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void ResetView(SimpleVelodyneViewerRenderingMode renderingMode)
        {
            cam = new Camera();
            cloudTransform.Reset();
            PointSize = 3;

            if (renderingMode == SimpleVelodyneViewerRenderingMode.Responsive) Render();
        }

        /// <summary>
        /// Clear the selection
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void ClearSelection()
        {
            ClearSelection(this.RenderingMode);
        }

        /// <summary>
        /// Clear the selection
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        public void ClearSelection(SimpleVelodyneViewerRenderingMode renderingMode)
        {
            foreach (ViewPoint pt in this.viewPoints)
            {
                pt.IsSelected = false;
            }

            if (renderingMode == SimpleVelodyneViewerRenderingMode.Responsive) Render();
        }

        /// <summary>
        /// Add new points to the view
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        /// <param name="points">Collection of VelodynePoint</param>
        public void AddNewPoints(IEnumerable<VelodynePoint> points, SimpleVelodyneViewerColorMap colorMap = SimpleVelodyneViewerColorMap.Default)
        {
            AddNewPoints(points, this.RenderingMode, colorMap);
        }

        /// <summary>
        /// Add new points to the view
        /// </summary>
        /// <param name="points">Collection of VelodynePoint</param>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void AddNewPoints(IEnumerable<VelodynePoint> points, SimpleVelodyneViewerRenderingMode renderingMode, SimpleVelodyneViewerColorMap colorMap = SimpleVelodyneViewerColorMap.Default)
        {
            foreach (VelodynePoint pt in points)
            {
                this.viewPoints.Add(new ViewPoint(pt, colorMap));
            }

            if (renderingMode == SimpleVelodyneViewerRenderingMode.Responsive) Render();
        }

        /// <summary>
        /// Add annotation
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        /// <param name="annot">Annotation to be added</param>
        public void AddAnnotaion(VelodyneAnnotation annot)
        {
            AddAnnotaion(annot, this.RenderingMode);
        }

        /// <summary>
        /// Add annotation
        /// </summary>
        /// <param name="annot">Annotation to be added</param>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void AddAnnotaion(VelodyneAnnotation annot, SimpleVelodyneViewerRenderingMode renderingMode)
        {
            this.viewPoints.Add(new ViewPoint(annot));

            if (renderingMode == SimpleVelodyneViewerRenderingMode.Responsive) Render();
        }

        /// <summary>
        /// Remove annotation; annotation is identified by ref
        /// Viewport will be rendered if it is specified by RenderingMode
        /// </summary>
        /// <param name="annot">Annotation object</param>
        public void RemoveAnnotaion(VelodyneAnnotation annot)
        {
            RemoveAnnotaion(annot, this.RenderingMode);
        }

        /// <summary>
        /// Remove annotation; annotation is identified by ref
        /// </summary>
        /// <param name="annot">Annotation object</param>
        /// <param name="renderingMode">Rendering mode, if responsive, viewport will be rendered</param>
        public void RemoveAnnotaion(VelodyneAnnotation annot, SimpleVelodyneViewerRenderingMode renderingMode)
        {
            ViewPoint vpToRemove = null;
            foreach (ViewPoint pt in viewPoints)
            {
                if (pt.PointObject == annot)
                {
                    vpToRemove = pt;
                    break;
                }
            }

            if (vpToRemove != null)
            {
                this.viewPoints.Remove(vpToRemove);
                if (renderingMode == SimpleVelodyneViewerRenderingMode.Responsive) Render();
            }
        }

        /// <summary>
        /// Get the selected velodyne points
        /// </summary>
        /// <returns>List of the selected velodyne points</returns>
        public List<VelodynePoint> GetSelectedPoints()
        {
            List<VelodynePoint> ret = new List<VelodynePoint>();
            foreach (ViewPoint pt in this.viewPoints)
            {
                if (pt.PointObject is VelodynePoint)
                {
                    if (pt.IsSelected == true)
                    {
                        ret.Add(pt.PointObject as VelodynePoint);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Get all velodyne points from the view. Annotations and other points are skipped.
        /// </summary>
        /// <returns>List of all velodyne points</returns>
        public List<VelodynePoint> GetPoints()
        {
            List<VelodynePoint> ret = new List<VelodynePoint>();
            foreach (ViewPoint pt in this.viewPoints)
            {
                if (pt.PointObject is VelodynePoint)
                {
                    ret.Add(pt.PointObject as VelodynePoint);
                }
            }

            return ret;
        }

        #endregion

        #region Rendering 

        /// <summary>
        /// Render preview
        /// </summary>
        public void RenderPreview()
        {
            if ((viewPoints == null) || (viewPoints.Count == 0)) return;

            if (worker != null)
            {
                if (worker.IsBusy)
                {
                    worker.CancelAsync();
                }
            }

            int step = Convert.ToInt32(Math.Floor((double)viewPoints.Count() / (double)PointNumForPreview));
            step = step == 0 ? 1 : step;
            Render(step);
        }

        /// <summary>
        /// Render view
        /// </summary>
        public void Render()
        {
            if ((DateTime.Now - lastRenderCall).TotalMilliseconds < 100) return; // to avoid overflow
            lastRenderCall = DateTime.Now;

            RenderPreview();

            if (worker != null)
            {
                if (worker.IsBusy)
                {
                    worker.CancelAsync();
                    workerDone.WaitOne();
                }
            }

            if (mDown) return;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Render view with a given sampling
        /// </summary>
        /// <param name="sampling">Number of skipped objects</param>
        private void Render(int sampling)
        {
            if ((viewPoints == null) || (viewPoints.Count == 0)) return;
            DateTime startTime = DateTime.Now;

            int width = viewPort.Width;
            int height = viewPort.Height;

            cam.cx = width / 2.0;
            cam.cy = height / 2.0;

            double R11 = Math.Cos(cam.phi) * Math.Cos(cam.kappa);
            double R12 = -Math.Cos(cam.phi) * Math.Sin(cam.kappa);
            double R13 = Math.Sin(cam.phi);
            double R21 = Math.Cos(cam.omega) * Math.Sin(cam.kappa) + Math.Sin(cam.omega) * Math.Sin(cam.phi) * Math.Cos(cam.kappa);
            double R22 = Math.Cos(cam.omega) * Math.Cos(cam.kappa) - Math.Sin(cam.omega) * Math.Sin(cam.phi) * Math.Sin(cam.kappa);
            double R23 = -Math.Sin(cam.omega) * Math.Cos(cam.phi);
            double R31 = Math.Sin(cam.omega) * Math.Sin(cam.kappa) - Math.Cos(cam.omega) * Math.Sin(cam.phi) * Math.Cos(cam.kappa);
            double R32 = Math.Sin(cam.omega) * Math.Cos(cam.kappa) + Math.Cos(cam.omega) * Math.Sin(cam.phi) * Math.Sin(cam.kappa);
            double R33 = Math.Cos(cam.omega) * Math.Cos(cam.phi);

            Bitmap bmp = new Bitmap(width, height);
            long ptk = 0;

            VelodynePoint veloPoint = null;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                for (int i = 0; i < viewPoints.Count(); i += sampling)
                {
                    ptk++;

                    if ((worker != null) && (worker.CancellationPending))
                    {
                        bmp.Dispose();
                        workerDone.Set();
                        g.Dispose();
                        return;
                    }

                    ViewPoint pti = viewPoints[i];
                    if (pti == null) continue;
                    PointLikeObject pt = Transfrom(pti.PointObject, cloudTransform);                   

                    double pti_x = cam.f * (R11 * (pt.X - cam.x) + R12 * (pt.Y - cam.y) + R13 * (pt.Z - cam.z)) / (R31 * (pt.X - cam.x) + R32 * (pt.Y - cam.y) + R33 * (pt.Z - cam.z)) + cam.cx;
                    double pti_y = -cam.f * (R21 * (pt.X - cam.x) + R22 * (pt.Y - cam.y) + R23 * (pt.Z - cam.z)) / (R31 * (pt.X - cam.x) + R32 * (pt.Y - cam.y) + R33 * (pt.Z - cam.z)) + cam.cy;

                    if ((pti_x > 0) && (pti_x < width) && (pti_y > 0) && (pti_y < height))
                    {

                        if (pti.PointObject is VelodynePoint)
                        {
                            veloPoint = pti.PointObject as VelodynePoint;
                            SimpleVelodyneViewerColorMap colorMap = pti.ColorMap;

                            if (pti.IsSelected)
                            {
                                colorMap = SimpleVelodyneViewerColorMap.Selection;
                            }

                            Color clr = default(Color);
                            switch (colorMap)
                            {
                                case SimpleVelodyneViewerColorMap.Default:
                                    clr = MapRainbowColor(255 - veloPoint.Intensity, 0, 255);
                                    break;
                                case SimpleVelodyneViewerColorMap.Selection:
                                    clr = MapRainbowColor(255 - veloPoint.Intensity, 0, 255);
                                    clr = Color.FromArgb(255, clr.G, clr.B);
                                    break;
                                default:
                                    break;
                            }

                            Brush brush = new System.Drawing.SolidBrush(clr);
                            g.FillEllipse(brush, (int)pti_x, (int)pti_y, PointSize, PointSize);
                            brush.Dispose();
                        }
                        else if (pti.PointObject is VelodyneAnnotation)
                        {
                            VelodyneAnnotation annot = pti.PointObject as VelodyneAnnotation;
                            Brush brush = new System.Drawing.SolidBrush(Color.Red);
                            g.FillEllipse(brush, (int)pti_x, (int)pti_y, PointSize*2, PointSize*2);
                            var str = annot.ID.ToString();
                            if (annot.Text != "") str += ": " + annot.Text;
                            g.DrawString(str, new Font("Arial", 12), brush, new PointF((float)pti_x+3, (float)pti_y+3));
                            brush.Dispose();
                        }
                        else
                        {
                            Brush brush = new System.Drawing.SolidBrush(Color.Red);
                            g.FillEllipse(brush, (int)pti_x, (int)pti_y, PointSize * 2, PointSize * 2);
                            brush.Dispose();
                        }

                        // update view coordinates
                        pti.ViewX = pti_x;
                        pti.ViewY = pti_y;
                    }

                    int refreshRate = Convert.ToInt32((double)viewPoints.Count() / 20.0);
                    if ((sampling == 1) && (ptk % 10 == 0))
                    {
                        //lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = ((double)ptk / (double)viewPoints.Count() * 100.0).ToString("0.0") + "%"));
                        lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text +=  ""));
                    }

                }
            }

            if (viewPort.Image != null) viewPort.Image.Dispose();
            viewPort.Invoke((MethodInvoker)(() => viewPort.Image = bmp));


            if (veloPoint != null)
            {
                DateTime endTime = DateTime.Now;
                TimeSpan tsPan = new TimeSpan(Convert.ToInt64(veloPoint.InternalTime * TimeSpan.TicksPerSecond));

                String timeStamp = "???";
                if (veloPoint.Timestamp != null)
                {
                    timeStamp = veloPoint.Timestamp.Value.ToString("yyy-MM-dd HH:mm:ss.fff");
                }

                String its = tsPan.ToString(@"mm\:ss\.fff");
                lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = "" + (endTime - startTime).TotalSeconds + "s (" + ptk / 1000 + "k)" +
                       " x: " + (cloudTransform.alpha / Math.PI * 180.0).ToString("0.0") + " y: " + (cloudTransform.beta / Math.PI * 180.0).ToString("0.0") + " z: " + (cloudTransform.kappa / Math.PI * 180.0).ToString("0.0") + " (" + its + "s) " + timeStamp));

            }
            else
            {
                lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = "No point!"));
            }

            workerDone.Set();

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // TODO: worker completed, what can we do here?
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Preview is done. Let's do the full resoltuion;
            try
            {
                Render(1);
            }catch
            {

            }
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transform a point based on the transformation object. Includes rotation and translation
        /// </summary>
        /// <param name="pt">Points object</param>
        /// <param name="trans">Transformation object</param>
        /// <returns></returns>
        private PointLikeObject Transfrom(PointLikeObject pt, ObjectTransfromation trans)
        {
            PointLikeObject ptt = Rotate(pt, trans);
            ptt.X += trans.dx;
            ptt.Y += trans.dy;
            ptt.Z += trans.dz;
            return ptt;
        }

        /// <summary>
        /// Rotate a point based on the transformation object. Only rotation, translation is skipped
        /// </summary>
        /// <param name="pt">Points object</param>
        /// <param name="trans">Transformation object</param>
        /// <returns>Transformed object; the input object won't be copied</returns>
        private PointLikeObject Rotate(PointLikeObject pt, ObjectTransfromation trans)
        {
            PointLikeObject ptt = Rotate(pt, 0, 0, trans.kappa);
            return Rotate(ptt, trans.alpha, trans.beta, trans.gamma);
        }

        /// <summary>
        /// otate a point based on X, Y, Z (omega, phi, kappa) rotations
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="omega"></param>
        /// <param name="phi"></param>
        /// <param name="kappa"></param>
        /// <returns></returns>
        private PointLikeObject Rotate(PointLikeObject pt, double omega, double phi, double kappa)
        {
            double R11 = Math.Cos(phi) * Math.Cos(kappa);
            double R12 = -Math.Cos(phi) * Math.Sin(kappa);
            double R13 = Math.Sin(phi);
            double R21 = Math.Cos(omega) * Math.Sin(kappa) + Math.Sin(omega) * Math.Sin(phi) * Math.Cos(kappa);
            double R22 = Math.Cos(omega) * Math.Cos(kappa) - Math.Sin(omega) * Math.Sin(phi) * Math.Sin(kappa);
            double R23 = -Math.Sin(omega) * Math.Cos(phi);
            double R31 = Math.Sin(omega) * Math.Sin(kappa) - Math.Cos(omega) * Math.Sin(phi) * Math.Cos(kappa);
            double R32 = Math.Sin(omega) * Math.Cos(kappa) + Math.Cos(omega) * Math.Sin(phi) * Math.Sin(kappa);
            double R33 = Math.Cos(omega) * Math.Cos(phi);

            PointLikeObject ret = pt.Clone() as PointLikeObject;
            ret.X = pt.X * R11 + pt.Y * R12 + pt.Z * R13;
            ret.Y = pt.X * R21 + pt.Y * R22 + pt.Z * R23;
            ret.Z = pt.X * R31 + pt.Y * R32 + pt.Z * R33;

            return ret;
        }

        #endregion

        #region User interactions (UI events)

        private void button6_Click(object sender, EventArgs e)
        {
            cloudTransform.gamma += 5.0 / 180.0 * Math.PI;
            Render();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cloudTransform.gamma -= 5.0 / 180.0 * Math.PI;
            Render();
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0) PointSize++;
                if (e.Delta < 0) PointSize--;
                if (PointSize < 2) PointSize = 2;

            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (e.Delta > 0) cloudTransform.kappa += 5.0 / 180.0 * Math.PI;
                if (e.Delta < 0) cloudTransform.kappa -= 5.0 / 180.0 * Math.PI;
            }
            else
            {
                cam.z = cam.z - e.Delta / 25D;
            }
            Render();

        }


        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            viewPort.Focus();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mDown = true;
            mPosition = e.Location;

            if (Control.ModifierKeys == Keys.Shift)
            {
                ClearSelection();
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // point selection
            if (Control.ModifierKeys == Keys.Shift)
            {
                foreach(ViewPoint pt in this.viewPoints)
                {
                    if ((selectStart.X < pt.ViewX ) && (pt.ViewX < selectEnd.X) && (selectStart.Y < pt.ViewY) && (pt.ViewY < selectEnd.Y))
                    {
                        pt.IsSelected = true;
                    }
                }
            }

            mDown = false;
            selectStart = default(Point);
            selectEnd = default(Point);
            viewPort.Invalidate();
            Render();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
            {
                int ldx = mPosition.X - e.X;
                int ldy = mPosition.Y - e.Y;

                if (Control.ModifierKeys == Keys.Control)
                {
                    cloudTransform.dx += ldx * 0.001 * cam.z;
                    cloudTransform.dy -= ldy * 0.001 * cam.z;
                    Render();
                }
                else if (Control.ModifierKeys == Keys.Shift)
                {
                    if (selectStart == default(Point))
                    {
                        selectStart = new Point(mPosition.X, mPosition.Y);                        
                    }
                    else
                    {
                        selectEnd = new Point(mPosition.X, mPosition.Y);                       
                    }
                    viewPort.Invalidate();
                }
                else
                {
                    cloudTransform.beta += ldx * 0.01;
                    cloudTransform.alpha += ldy * 0.01;
                    Render();
                }
            }

            mPosition = e.Location;

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            // Is an annotation clicked?
            if (Control.ModifierKeys == Keys.None)
            {
                double minR = Double.MaxValue;
                VelodyneAnnotation minAnnot = null;
                foreach (ViewPoint pt in this.viewPoints)
                {
                    if (!(pt.PointObject is VelodyneAnnotation)) continue;
                    double r = Math.Sqrt(Math.Pow(pt.ViewX - mPosition.X, 2) + Math.Pow(pt.ViewX - mPosition.X, 2));
                    if (r < minR)
                    {
                        minR = r;
                        minAnnot = pt.PointObject as VelodyneAnnotation;
                    }
                }

                if ((minAnnot != null) && (minR < MaximumSnapRadius))
                {
                    OnAnnotationClicked(minAnnot, e);
                }
            }

        }

        protected virtual void OnAnnotationClicked(VelodyneAnnotation annot, EventArgs e)
        {
            AnnotationClicked?.Invoke(annot, e);
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {

        }

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            // when the control is loading, this event is fired and a concurrent 
            // rendering thread starts. initDone switch helps avoiding this situation.
            if(initDone) Render();
        }

        private void lblHelp_MouseEnter(object sender, EventArgs e)
        {
            lblHelpText.Visible = true;
        }

        private void lblHelp_MouseLeave(object sender, EventArgs e)
        {
            lblHelpText.Visible = false;
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                ClearSelection();
            }
            else
            {
                ResetView();
            }
            Render();
        }


        private void viewPort_Paint(object sender, PaintEventArgs e)
        {
            if ((selectStart != default(Point)) && (selectEnd != default(Point)))
            {
                Brush brush = new System.Drawing.SolidBrush(Color.Red);
                Pen pen = new Pen(brush);
                e.Graphics.DrawRectangle(pen, selectStart.X, selectStart.Y, mPosition.X - selectStart.X, mPosition.Y - selectStart.Y);
            }

        }

        #endregion

        #region Helpers

        // Map a value to a rainbow color.
        private Color MapRainbowColor(float value, float red_value, float blue_value)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int)(1023 * (value - red_value) /
                (blue_value - red_value));

            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                return Color.FromArgb(255, int_value, 0);
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                return Color.FromArgb(255 - int_value, 255, 0);
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                return Color.FromArgb(0, 255, int_value);
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                return Color.FromArgb(0, 255 - int_value, 255);
            }
        }


        #endregion


    }
}
