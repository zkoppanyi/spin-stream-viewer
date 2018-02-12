using csmatio.io;
using csmatio.types;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using OSUCalibrator.Loggers;
using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSUCalibrator
{
    public enum MatlabExporterOptions
    {
        Overwrite,
        Append
    }

    public static class MatlabExporter
    {
        public async static Task Export(String filename, Project project, ILogger logger=null, CancellationToken cancelletionToken = default(CancellationToken))
        {
            logger?.WriteLineInfo("");
            logger?.WriteLineInfo("Saving project into MAT file!");
            logger?.WriteLineInfo("MAT file location: " + filename);

            List<MLArray> mlList = new List<MLArray>();
            int pi = 0;
            foreach (DataStream dataStream in project.DataStreams)
            {
                pi++;
                cancelletionToken.ThrowIfCancellationRequested();

                logger?.WriteLineInfo("Exporting " + dataStream.ShortName + " datastream...");
                logger?.WriteProgress((double)pi / (double)project.DataStreams.Count() * 100.0);

                MLStructure structDataStream = new MLStructure(dataStream.ShortName, new int[] { 1, 1 });
                structDataStream["Name", 0] = new MLChar(null, dataStream.Name);
                structDataStream["ShortName", 0] = new MLChar(null, dataStream.ShortName);

                if (dataStream.DataLines.Count > 0)
                {
                    // DataLines
                    MLCell arrayDataLines = new MLCell("DataLines", new int[] { dataStream.DataLines.Count, 1 });
                    int k = 0;
                    foreach (DataLine dataLine in dataStream.DataLines)
                    {
                        MLStructure structDataLine = new MLStructure(dataStream.ShortName, new int[] { 1, 1 });
                        structDataLine["Timestamp", 0] = new MLDouble("", new double[] { Utils.ConvertToUnixTimestamp(dataLine.TimeStamp) }, 1);
                        structDataLine["TimestampC", 0] = ConvertDateTimeToMLDouble(dataLine.TimeStamp);

                        if (dataLine is ImageDataLine)
                        {
                            structDataLine["ImageFileName", 0] = new MLChar(null, (dataLine as ImageDataLine).ImageFileName);
                        }

                        if (dataLine is VideoDataLine)
                        {
                            structDataLine["VideoFileName", 0] = new MLChar(null, (dataLine as VideoDataLine).VideoFileName);
                        }

                        arrayDataLines[k] = structDataLine;
                        k++;
                    }
                    structDataStream["DataLines", 0] = arrayDataLines;
                }

                
                if (dataStream is GPSDataStream)
                {
                    GPSDataStream gpsDataStream = dataStream as GPSDataStream;

                    // Event Marker
                    if (gpsDataStream.MarkerEvents.Count > 0)
                    {
                        MLCell arrayMarkerEvents = new MLCell("MarkerEvents", new int[] { gpsDataStream.MarkerEvents.Count, 1 });
                        int k = 0;
                        foreach (EvenMarkerDataLine evnt in gpsDataStream.MarkerEvents)
                        {
                            MLStructure structEventMarkers = new MLStructure(dataStream.ShortName, new int[] { 1, 1 });
                            structEventMarkers["Timestamp", 0] = new MLDouble("", new double[] { (double)evnt.TimeStamp.Ticks }, 1);
                            structEventMarkers["TimestampC", 0] = ConvertDateTimeToMLDouble(evnt.TimeStamp);
                            structEventMarkers["Port", 0] = new MLChar(null, evnt.Port.ToString()); 
                            arrayMarkerEvents[k] = structEventMarkers;
                            k++;
                        }
                        structDataStream["MarkerEvents", 0] = arrayMarkerEvents;
                    }

                    // Positions
                    if (gpsDataStream.Positions.Count > 0)
                    {
                        double[][] points = new double[gpsDataStream.Positions.Count][];
                        int k = 0;
                        foreach (GPSPositionDataLine pt in gpsDataStream.Positions)
                        {

                            points[k] = new double[5];
                            points[k][0] = Utils.ConvertToUnixTimestamp(pt.TimeStamp);
                            points[k][1] = pt.Lat;
                            points[k][2] = pt.Lon;
                            points[k][3] = pt.Height;
                            points[k][4] = pt.Quality;
                            k++;
                        }
                        MLDouble arrayPoints = new MLDouble("Points", points);
                        structDataStream["Points", 0] = arrayPoints;
                    }
                }

                mlList.Add(structDataStream);
            }

            try
            {
                MatFileWriter mfw = new MatFileWriter(filename, mlList, false);
            }
            catch (Exception ex)
            {
                logger?.WriteLineError("Error occured: " + ex.ToString());
                return;
            }
            
            logger?.WriteLineInfo("MAT file location: " + filename);
            logger?.WriteLineInfo("Done.");

        }

        public static void ExportHotFrame(String rootFolder, String annotationName, HotFrame frame, 
            MatlabExporterOptions options = MatlabExporterOptions.Overwrite, ILogger logger = null, CancellationToken cancelletionToken = default(CancellationToken))
        {
            // delete features
            /*List<Feature> newFeatureSet = new List<Feature>();
            foreach (Feature feature in frame.Features)
            {
                if (feature.DataStream != null) newFeatureSet.Add(feature);
            }
            frame.Features = newFeatureSet;*/


            String folderName = rootFolder + "\\frame_" + annotationName;
            String matFilePath = folderName + "\\frame_" + annotationName + ".mat";
            Directory.CreateDirectory(folderName);

            logger?.WriteLineInfo("");
            logger?.WriteLineInfo("Convert frame to Matlab MAT format and export images!");
            logger?.WriteLineInfo("Export folder: " + folderName);
            logger?.WriteLineInfo("MAT file location: " + matFilePath);

            List<MLArray> mlList = new List<MLArray>();
            Dictionary<DataStream, List<VeloFeature>> velodyneData = new Dictionary<DataStream, List<VeloFeature>>();

            if ((options == MatlabExporterOptions.Append) && (File.Exists(matFilePath)))
            {
                MatFileReader matReader = new MatFileReader(matFilePath);
                mlList = matReader.Data;
            }

            int i = 0;
            foreach (Feature feature in frame.Features)
            {
                logger?.WriteLineInfo("Exporting: " + feature.DataStream.ShortName);

                MLStructure structFeatures = new MLStructure(feature.DataStream.ShortName, new int[] { 1, 1 });
                structFeatures["Name", 0] = new MLChar(null, feature.DataStream.Name);
                structFeatures["ShortName", 0] = new MLChar(null, feature.DataStream.ShortName);

                if (cancelletionToken.IsCancellationRequested == true)
                {
                    logger?.WriteLineWarning("Export cancelled!");
                    return;
                }

                if (feature is GPSFeature)
                {
                    var gpsFeature = feature as GPSFeature;
                    structFeatures["Lat", 0] = new MLDouble("", new double[] { gpsFeature.Position.Lat }, 1);
                    structFeatures["Lon", 0] = new MLDouble("", new double[] { gpsFeature.Position.Lon }, 1);
                    structFeatures["Height", 0] = new MLDouble("", new double[] { gpsFeature.Position.Height }, 1);
                    structFeatures["Quality", 0] = new MLDouble("", new double[] { gpsFeature.Position.Quality }, 1);

                    mlList.Add(structFeatures);
                }
                else if (feature is ImageFeature)
                {
                    ImageFeature imageFeature = feature as ImageFeature;
                    structFeatures["Timestamp", 0] = new MLDouble("", new double[] { Utils.ConvertToUnixTimestamp(imageFeature.TimeStamp)  }, 1);
                    structFeatures["TimestampC", 0] = ConvertDateTimeToMLDouble(imageFeature.TimeStamp);

                    double[][] points = new double[imageFeature.Points.Count][];
                    int k = 0;
                    foreach (ImagePoint pt in ((ImageFeature)feature).Points)
                    {
                        points[k] = new double[3];
                        points[k][0] = pt.ID;
                        points[k][1] = pt.X;
                        points[k][2] = pt.Y;
                        k++;
                    }

                    if (k != 0)
                    {
                        MLDouble arrayPoints = new MLDouble("Points", points);
                        structFeatures["Points", 0] = arrayPoints;
                    }



                    // save images
                    if (imageFeature.Image != null)
                    {
                        using (Bitmap img = new Bitmap(imageFeature.Image))
                        {
                            img.Save(folderName + "\\frame_" + annotationName + "_" + feature.DataStream.ShortName + ".bmp");
                        }

                        // you can export the images here, but it's very slow and the code also needs to be tested!
                        /*using (Bitmap img = new Bitmap(imageFeature.Image))
                        {

                            int[] dims = new int[] { img.Width, img.Height, 3 };
                            MLDouble arrImg = new MLDouble("Image", dims);
                            for (int i = 0; i < img.Width; i++)
                            {
                                for (int j = 0; j < img.Height; j++)
                                {
                                    Color col = img.GetPixel(i, j);
                                    arrImg.Set(col.R, i, j + img.Height * 0);
                                    arrImg.Set(col.G, i, j + img.Height * 1);
                                    arrImg.Set(col.B, i, j + img.Height * 2);
                                }
                            }

                            structFeatures["Image", 0] = arrImg;
                        }*/
                    }

                    mlList.Add(structFeatures);
                }
                else if (feature is VeloFeature)
                {
                    VeloFeature veloFeature = feature as VeloFeature;
                    // if key does not exist create one
                    if (!velodyneData.ContainsKey(veloFeature.DataStream))
                    {
                        velodyneData.Add(veloFeature.DataStream, new List<VeloFeature>());
                    }

                    velodyneData[veloFeature.DataStream].Add(veloFeature);
                }

                i++;
                logger?.WriteProgress((double)i / (double)frame.Features.Count() * 100.0);
            }


            // ok, now finalize velodyne data
            foreach (KeyValuePair<DataStream, List<VeloFeature>> entry in velodyneData)
            {
                if (cancelletionToken.IsCancellationRequested == true)
                {
                    logger?.WriteLineWarning("Export cancelled!");
                    return;
                }

                MLStructure structFeatures = new MLStructure(entry.Key.ShortName, new int[] { 1, 1 });
                structFeatures["Name", 0] = new MLChar(null, entry.Key.Name);
                structFeatures["ShortName", 0] = new MLChar(null, entry.Key.ShortName);
                structFeatures["TimeStamp", 0] = new MLDouble("", new double[] { Utils.ConvertToUnixTimestamp(frame.Timestamp) }, 1);

                // get number of points 
                long n_points = 0;
                foreach(VeloFeature feature in entry.Value)
                {
                    n_points += feature.Points.Count();
                }

                if (n_points == 0)
                {
                    logger?.WriteLineWarning(entry.Key.ShortName + ": No features!");
                    continue;
                }

                // populate points
                double[][] points = new double[n_points][];
                int k = 0;
                foreach (VeloFeature feature in entry.Value)
                {
                    foreach (VelodynePoint pt in feature.Points)
                    {
                        points[k] = new double[9];
                        points[k][0] = feature.ID;
                        points[k][1] = pt.X;
                        points[k][2] = pt.Y;
                        points[k][3] = pt.Z;
                        points[k][4] = Convert.ToDouble(pt.Intensity);
                        points[k][5] = pt.Distance;
                        points[k][6] = pt.Hz;
                        points[k][7] = pt.Vz;
                        points[k][8] = pt.InternalTime;
                        k++;
                    }
                }

                MLDouble arrayPoints = new MLDouble("Points", points);
                structFeatures["Points", 0] = arrayPoints;

                mlList.Add(structFeatures);
            }

            try
            {
                logger?.WriteLineInfo("Saving mat file...");
                MatFileWriter mfw = new MatFileWriter(matFilePath, mlList, true);
            }
            catch (Exception ex)
            {
                logger?.WriteLineError("Error occured: " + ex.ToString());
                return;
            }

            logger?.WriteProgress(100.0);
            logger?.WriteLineInfo("Done.");

        }

        private static MLDouble ConvertDateTimeToMLDouble(DateTime TimeStamp)
        {
            MLDouble tsArray = new MLDouble(null, new double[] { TimeStamp.Year, TimeStamp.Month, TimeStamp.Day,
                                TimeStamp.Hour, TimeStamp.Minute, (double)TimeStamp.Second + (double)TimeStamp.Millisecond / 1000.0 }, 1);
            return tsArray;
        }
    }
}
