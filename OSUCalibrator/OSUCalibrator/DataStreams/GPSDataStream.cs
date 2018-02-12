using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSUCalibrator.DataStreams
{
    [Serializable]
    public class GPSDataStream : DataStream
    {
        public List<EvenMarkerDataLine> MarkerEvents { get; set; }
        public List<GPSPositionDataLine> Positions{ get; set; }

        public override DateTime EndTime { get { return DataLines.Count > 0 ? DataLines.Max(x => x.TimeStamp + ((LongFileDataLine)x).Length) : DateTime.MaxValue; } }


        public GPSDataStream(Project project, string name, string shortName, string subFolder) : base(project, name, shortName, subFolder)
        {
            MarkerEvents = new List<EvenMarkerDataLine>();
            Positions = new List<GPSPositionDataLine>();
        }

        public override void WriteMetadata(ILogger writer, CancellationToken cancelletionToken = default(CancellationToken))
        {
            CreateMetadataHeader(writer);

            String subFolder = this.project.Folder + "\\" + this.SubFolder;
            writer.WriteLineInfo("Data location:           " + subFolder);
            writer.WriteLineInfo(" ");

            if (Directory.Exists(subFolder) == false)
            {
                writer.WriteLineInfo("!!!   Folder does not exist! Data stream is missing!");
                return;
            }

            // RINEX logs
            string[] allFiles = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.*O");
            string[] files = allFiles;
            if (files.Count() > 0)
            {
                foreach (string fpath in files)
                {
                    DateTime startTimef = DateTime.UtcNow;
                    DateTime endTimef = DateTime.UtcNow;
                    using (StreamReader reader = File.OpenText(fpath))
                    {
                        string line = "";
                        int linenum = 0;                       
                        while ((line = reader.ReadLine()) != null)
                        {
                            cancelletionToken.ThrowIfCancellationRequested();

                            string[] linesp = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            if (linesp.Count() > 9)
                            {
                                if ((linesp[9] == "FIRST") || (linesp[9] == "LAST"))
                                {
                                    DateTime dt = new DateTime(Convert.ToInt16(linesp[0]), Convert.ToInt16(linesp[1]), Convert.ToInt16(linesp[2]),
                                                                Convert.ToInt16(linesp[3]), Convert.ToInt16(linesp[4]), Convert.ToInt16(Convert.ToDouble(linesp[5])));

                                    if ((linesp[9] == "FIRST")) startTimef = dt;
                                    if ((linesp[9] == "LAST"))
                                    {
                                        endTimef = dt;
                                        break;
                                    }
                                }
                            }

                            if (linenum > 200) break;
                            linenum++;
                        }

                        if (linenum > 200)
                        {
                            writer.WriteLineInfo("\tObservations could not be detected! File: " + fpath);
                            writer.WriteLineWarning("tObservations could not be detected! File: " + fpath);
                        }
                    }

                    GPSObsFileDataLine dataLine = new GPSObsFileDataLine(fpath, startTimef, TimeType.GPS);
                    dataLine.Length = new TimeSpan(endTimef.Ticks - startTimef.Ticks);
                    this.DataLines.Add(dataLine);
                }

                if (this.Length == 0)
                {
                    writer.WriteLineInfo("\tGPS observation is empty of too short GPS!");
                    writer.WriteLineWarning("GPS observation is empty of too short GPS!");
                }
                writer.WriteLineInfo("\t             First observation : " + StartTime);
                writer.WriteLineInfo("\t              Last observation : " + EndTime);
                writer.WriteLineInfo("\t             Length of observ. : " + (new TimeSpan(EndTime.Ticks - StartTime.Ticks)).ToString(@"hh\:mm\:ss"));

                this.OrderDataLines();
            }
            else
            {
                writer.WriteLineInfo("\tNo observation file!");
                writer.WriteLineWarning(this.Name + ": No observation file!");
            }            
        }


        public void LoadPositionFile(ILogger writer)
        {
            if (Positions == null)
            {
                Positions = new List<GPSPositionDataLine>();
            }
            else
            {
                Positions.Clear();
            }

            string[] files = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.pos");
            if (files.Count() > 0)
            {
                foreach (string fpath in files)
                {
                    using (StreamReader reader = File.OpenText(fpath))
                    {
                        // skip header
                        for (int k = 0; k < 24; k++) reader.ReadLine();

                        String header = reader.ReadLine();
                        double offset = 0;
                        if (header.Contains("GPST"))
                        {
                            //offset = project.GPSTLeapSeconds;
                        }

                        String line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] linesplt = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            // get time
                            string[] datesplt = linesplt[0].Split('/');
                            string[] timesplt = linesplt[1].Split(':');
                            DateTime dt = new DateTime(Convert.ToInt32(datesplt[0]), Convert.ToInt32(datesplt[1]), Convert.ToInt32(datesplt[2]),
                                                                Convert.ToInt32(timesplt[0]), Convert.ToInt32(timesplt[1]), 0);
                            dt = dt.AddSeconds(Convert.ToDouble(timesplt[2]) + offset); 
                            GPSPositionDataLine dataLine = new GPSPositionDataLine(dt, TimeType.GPS);

                            //get coors
                            int latd = Convert.ToInt32(linesplt[2]);
                            int lath = Convert.ToInt32(linesplt[3]);
                            double latss = Convert.ToDouble(linesplt[4]);

                            double lat = (double)latd + (double)lath / 60.0 + (double)latss / 3600.0;
                            if (latd < 0)
                            {
                                lat = (double)latd - (double)lath / 60.0 - (double)latss / 3600.0;
                            }

                            dataLine.Lat = lat;

                            int lond = Convert.ToInt32(linesplt[5]);
                            int lonh = Convert.ToInt32(linesplt[6]);
                            double lonss = Convert.ToDouble(linesplt[7]);
                            double lon = (double)lond + (double)lonh / 60.0 + (double)lonss / 3600.0;
                            if (lond < 0)
                            {
                                lon = (double)lond - (double)lonh / 60.0 - (double)lonss / 3600.0;
                            }
                            dataLine.Lon = lon;


                            double height = Convert.ToDouble(linesplt[8]);
                            dataLine.Height = height;

                            double q = Convert.ToInt32(linesplt[9]);
                            dataLine.Quality = q;

                            this.Positions.Add(dataLine);
                        }
                    }
                }
            }

            Positions.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));
            if ((Positions.Count) == 0)
            {
                writer.WriteLineWarning("No postion file is found or no coordiantes!");
            }
            {
                writer.WriteLineInfo("\t            # of position entries : " + Positions.Count());
                writer.WriteLineInfo("\t                   First position : " + Positions.Min(x => x.TimeStamp));
                writer.WriteLineInfo("\t                    Last position : " + Positions.Max(x => x.TimeStamp));
            }
        }

        public GPSPositionDataLine GetPositionByTime(DateTime time)
        {
            double min_dt = Double.MaxValue;
            int min_frame = -1;
            for (int i = 0; i < Positions.Count; i++)
            {
                double dt = Math.Abs((new TimeSpan(Positions[i].TimeStamp.Ticks - time.Ticks)).TotalSeconds);
                if (dt < min_dt)
                {
                    min_dt = dt;
                    min_frame = i;
                }
            }

            return Positions[min_frame];

        }


    }
}
