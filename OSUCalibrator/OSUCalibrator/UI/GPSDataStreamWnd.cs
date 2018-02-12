using GMap.NET.WindowsForms;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using OSUCalibrator.Loggers;
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
    public partial class GPSDataStreamWnd : Form, IStreamWindow
    {
        public MainForm MainForm { get; set; }
        private GPSDataStream dataStream;
        public DataStream DataStream { get { return dataStream;  } }
        public HotFrame CurrentHotFrame { get; private set; }
        public GPSPositionDataLine CurrentPosition { get; private set; }
        public bool HasToSetFrame { get { return isHasToSetFrame; } }

        private ILogger logger;
        private GMapOverlay trajectoryPoints;
        private bool isHasToSetFrame = true;
        private int lastFrame = -1;

        public GPSDataStreamWnd(GPSDataStream dataStream, ILogger logger)
        {
            InitializeComponent();
            this.dataStream = dataStream;
            this.MainForm = (MainForm)this.MdiParent;
            this.logger = logger;
            if ((dataStream.Positions == null) || dataStream.Positions.Count() == 0) dataStream.LoadPositionFile(logger);

            InitMap();

            // seek to first GPS fix
            int i = 0;
            foreach (GPSPositionDataLine dataLine in dataStream.Positions)
            {
                if (dataLine.Quality == 1)
                {
                    SetFrame(i);
                    break;
                }
                i++;
            }
        }

        public void NextFrame()
        {
        }

        public void InitMap()
        {
            //map.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            map.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            //map.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            map.MinZoom = 2;
            map.MaxZoom = 18;
            map.Zoom = 18;
            //map.SetPositionByKeywords("Ohio State University, Columbus, Ohio, USA");
            map.ShowCenter = false;

            // put whole data!
            /*GMapOverlay markers = new GMapOverlay("markers");
            int i = 0;
            foreach (GPSPositionDataLine dataLine in dataStream.Positions)
            {
                i++;
                if (dataStream.ShortName.Contains("SEPT"))
                {
                    if (!(i % 5 == 0)) continue;
                }

                if (dataLine.Quality == 1)
                {
                    double lat = dataLine.Lat;
                    double lon = dataLine.Lon;

                    GMap.NET.WindowsForms.GMapMarker marker =
                                new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                                    new GMap.NET.PointLatLng(lat, lon),
                                    Properties.Resources.point_marker);

                    markers.Markers.Add(marker);
                }
               
            }
            map.Overlays.Add(markers);*/

            trajectoryPoints = new GMapOverlay("selected_marker");
            map.Overlays.Add(trajectoryPoints);

            GMapMarker marker =
            new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                new GMap.NET.PointLatLng(39.999019019444447, -83.032988538888887),
                Properties.Resources.point_marker);

            trajectoryPoints.Markers.Add(marker);

            marker =
                new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                    new GMap.NET.PointLatLng(39.999090419444443, -83.032773480555548),
                    Properties.Resources.point_marker);
            trajectoryPoints.Markers.Add(marker);

        }

        public void SetFrame(int frame)
        {
            if ((frame < 0) || (frame >= dataStream.Positions.Count()))
            {
                toolStripStatus.Text = "Outside of the stream time limits.";
                return;
            }

            if (frame == lastFrame) return;

            if (dataStream.Positions.Count() != 0)
            {

                CurrentPosition = dataStream.Positions[frame];

                //if (frameDataLine.Quality == 1)
                //{
                    GMap.NET.WindowsForms.GMapMarker marker =
                                new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                                    //new GMap.NET.PointLatLng(frameDataLine.Lat + 8/3600,  frameDataLine.Lon + 8 / 3600),
                                    new GMap.NET.PointLatLng(CurrentPosition.Lat, CurrentPosition.Lon),
                                    Properties.Resources.point_marker_selected);

                    trajectoryPoints.Markers.Add(marker);

                    map.Position = new GMap.NET.PointLatLng(CurrentPosition.Lat, CurrentPosition.Lon);
                /*}
                else
                {
                    toolStripStatus.Text = "No GPS fix!";
                }*/
            }
            map.Refresh();
        }

        public void SetFrameByTime(DateTime time)
        {
            GPSPositionDataLine position = dataStream.GetPositionByTime(time);

            // kinda ugly here, but too tired, still working just a day before thanksgiving, it's 6pm, the building is empty
            double min_dt = Double.MaxValue;
            int min_frame = -1;
            for (int i = 0; i < dataStream.Positions.Count; i++)
            {
                if (dataStream.Positions[i] == position)
                {
                    min_dt = Math.Abs((new TimeSpan(dataStream.Positions[i].TimeStamp.Ticks - time.Ticks)).TotalSeconds);
                    min_frame = i;
                    break;
                }
            }

            if (min_dt < 4)
            {
                SetFrame(min_frame);
            }
            else
            {
                SetFrame(-1);
            }

            toolStripStatusSecond.Text = "Req.: " + time.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Diff: " + min_dt;
        }

        public Image GetSnapshot()
        {
            Bitmap bmp = new Bitmap(640, 480);
            map.DrawToBitmap(bmp, new Rectangle(0, 0, 640, 480));
            return bmp;
        }

        private void GPSDataStreamWnd_Load(object sender, EventArgs e)
        {

        }

        private void deletePointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trajectoryPoints.Clear();
        }

        public void SetHotFrame(HotFrame frame)
        {
            SetFrameByTime(frame.Timestamp);
            CurrentHotFrame = frame;

            // update coordinates in the feature
            GPSFeature gpsFeature = null;
            foreach (var feature in frame.Features)
            {
                var gpsFeatureCurr = feature as GPSFeature;
                if (gpsFeatureCurr != null)
                {
                    if (gpsFeatureCurr.DataStream.Name == this.DataStream.Name)
                    {
                        gpsFeature = gpsFeatureCurr;
                    }
                }
            }

            if (gpsFeature == null)
            {
                gpsFeature = new GPSFeature(this.DataStream, this.CurrentPosition);
                frame.Features.Add(gpsFeature);
            }

            gpsFeature.Position = this.CurrentPosition;

        }

        private void reloadPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataStream.LoadPositionFile(logger);
        }
    }
}
