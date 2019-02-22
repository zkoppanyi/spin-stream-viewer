using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public class VelodyneReader : IDisposable
    {
        public enum SearchType
        {
            CEIL,
            FLOOR,
            CLOSEST
        }

        public delegate void ProgressReportEventHandler(object sender, ProgressReportEventArgs args);
        public static event ProgressReportEventHandler ProgressReport;

        protected static void OnReportProgress(ProgressReportEventArgs e)
        {
            ProgressReport?.Invoke(null, e);
        }

        public static void ClearProgressReport()
        {
            ProgressReport = null;
        }

        public VelodyneSensorType Sensor { get; private set; }
        public String IndexFile { get; private set; }
        public String PointFile { get; private set; }
        public List<IndexData> Indeces { get; private set; }
        public PacketInterpreter PacketInterpreter { get; private set; }

        private BinaryReader pointReader = null;
        //private byte[] nextPacket = null;

        /// <summary>
        /// Create reader object
        /// </summary>
        /// <param name="type">Type of the sensor</param>
        /// <param name="returnMode">Define return type for VLP-16, this variable is neglected for HDL-32E</param>
        /// <param name="indexFile">Index file path</param>
        /// <param name="pointFile">Point file path</param>
        private VelodyneReader(VelodyneSensorType type, ReturnMode returnMode, String indexFile, String pointFile)
        {
            this.IndexFile = indexFile;
            this.PointFile = pointFile;
            this.Sensor = type;
            Indeces = new List<IndexData>();

            if (!File.Exists(indexFile))
            {
                throw new FileNotFoundException("Index file does not exist!");
            }

            if (!File.Exists(pointFile))
            {
                throw new FileNotFoundException("Point file does not exist!");
            }

            if (type == VelodyneSensorType.HDL32E)
            {
                PacketInterpreter = new PacketInterpreterHDL32E();
            } else if (type == VelodyneSensorType.VLP16)
            {
                PacketInterpreter = new PacketInterpreterVLP16(returnMode);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create reader object
        /// </summary>
        /// <param name="type">Type of the sensor</param>
        /// <param name="indexFile">Index file path</param>
        /// <param name="pointFile">Point file path</param>
        private VelodyneReader(VelodyneSensorType type, String indexFile, String pointFile) : this(type, ReturnMode.AllReturns, indexFile, pointFile)
        {

        }

        /// <summary>
        /// Open Velodyne reader object
        /// </summary>
        /// <param name="type">Type of the sensor</param>
        /// <param name="returnMode">Define return type for VLP-16, this variable is neglected for HDL-32E</param>
        /// <param name="indexFile">Index file path</param>
        /// <param name="pointFile">Point file path</param>
        /// <returns></returns>
        public static VelodyneReader Open(VelodyneSensorType type, ReturnMode returnMode, String indexFile, String pointFile)
        {
            VelodyneReader obj = new VelodyneReader(type, returnMode, indexFile, pointFile);
            FileInfo fi = new FileInfo(indexFile);
            long fileSize = fi.Length;
            obj.Indeces.Clear();

            byte[] idxData = File.ReadAllBytes(obj.IndexFile);
            MemoryStream mem = new MemoryStream(idxData);
            long idx = 0;
            using (BinaryReader reader = new BinaryReader(mem))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    long internalTimeTicks = reader.ReadInt64();
                    DateTime internalTime = new DateTime(internalTimeTicks);
                    long packetTimeStampTicks = reader.ReadInt64();
                    DateTime packetTimeStamp = new DateTime(packetTimeStampTicks);

                    long position = reader.ReadInt64();

                    byte[] nmeBytes = reader.ReadBytes(IndexData.NMEA_LENGTH);
                    String nmea = Encoding.ASCII.GetString(nmeBytes);

                    obj.Indeces.Add(new IndexData(packetTimeStamp, internalTime, position, nmea));
                    idx++;

                    if (idx % 500 == 0)
                    {
                        ProgressReportEventArgs args = new ProgressReportEventArgs((((double)reader.BaseStream.Position / (double)fileSize) * 100.0), reader.BaseStream.Position, packetTimeStamp.ToUniversalTime());
                        OnReportProgress(args);
                    }
                }
            }

            obj.pointReader = new BinaryReader(File.Open(obj.PointFile, FileMode.Open));
            
            return obj;
        }

        /// <summary>
        /// Open Velodyne reader object
        /// </summary>
        /// <param name="type">Type of the sensor</param>
        /// <param name="returnMode">Define return type for VLP-16, this variable is neglected for HDL-32E</param>
        /// <param name="indexFile">Index file path</param>
        /// <param name="pointFile">Point file path</param>
        /// <returns></returns>
        public static VelodyneReader Open(VelodyneSensorType type, String indexFile, String pointFile)
        {
            return Open(type, ReturnMode.AllReturns, indexFile, pointFile);
        }

        /// <summary>
        /// Seek data record by index object
        /// </summary>
        /// <param name="idx">Index object</param>
        public void Seek(IndexData idx)
        {
            if (pointReader == null) throw new Exception("Reader has not been opened!");
            pointReader.BaseStream.Seek(idx.Position, SeekOrigin.Begin);
            //nextPacket = null;
        }


        public void AnalysOffset()
        {
            DateTime prevGps = DateTime.MaxValue;
            int k = 0;
            foreach(IndexData idx in Indeces)
            {
                if (idx.Nmea.GPSTime != prevGps)
                {
                    Seek(idx);
                    VelodynePacket packet = ReadNext();
                    VelodynePointPacket pointPacket = packet as VelodynePointPacket;
                    DateTime? ptTs = pointPacket.Points[0].Timestamp;
                    if (ptTs != null)
                    {
                        TimeSpan dt = new TimeSpan(ptTs.Value.TimeOfDay.Ticks - idx.Nmea.GPSTime.TimeOfDay.Ticks);

                        //if (dt.Minutes < 1)
                        //{
                            //if (k++ > 5000) return;
                            if (dt.TotalSeconds > 0.1)
                            {
                                Console.WriteLine(dt.TotalSeconds.ToString("0.000") + " " + idx.InternalTimeStamp.ToString("HH:mm:ss.fff") + " " + idx.Nmea.GPSTime.ToString("HH:mm:ss.fff") + " " + Math.Floor(pointPacket.Points[0].InternalTime / 60.0).ToString("0")
                                    + " " + (pointPacket.Points[0].InternalTime - Math.Floor(pointPacket.Points[0].InternalTime / 60.0) * 60).ToString("0.000"));
                            }
                        //}
                    }
                }
                prevGps = idx.Nmea.GPSTime;
                //prevGps = DateTime.MaxValue;

                //if (k++ > 5000) return;
            }
        }

        /// <summary>
        /// Seek to the indicated type
        /// </summary>
        /// <param name="timeStamp">Timestamp to seek</param>
        /// <param name="searchType"></param>
        /// <returns>Time difference between the found and requested times</returns>
        public double SeekByTime(DateTime timeStamp, SearchType searchType = SearchType.FLOOR)
        {
            IndexData idx = FindIndexByTime(timeStamp, SearchType.FLOOR);
            Seek(idx);

            // now read records until find the best point record
            VelodynePacket packet = ReadNext();
            double it = 0;
            VelodynePointPacket pointPacket = null;
            while ((!(packet is VelodyneEndOfFile)))
            {
                pointPacket = packet as VelodynePointPacket;
                if (pointPacket.Points[0].Timestamp.Value.Ticks > timeStamp.Ticks) break;
                packet = ReadNext();
                it = pointPacket.Points[0].InternalTime;
            }

            return new TimeSpan(pointPacket.Points[0].Timestamp.Value.Ticks - timeStamp.Ticks).TotalSeconds;
        }

        /// <summary>
        /// Get internal time from the correspnding point
        /// </summary>
        /// <param name="idx">Index object</param>
        /// <returns>Internal time as seconds from top of the hour</returns>
        /*private double GetInternalTimestampForIndex(IndexData idx)
        {
            long pos = pointReader.BaseStream.Position;
            pointReader.BaseStream.Seek(idx.Position, SeekOrigin.Begin);

            //VelodynePointPacket pointPacket = (VelodynePointPacket)this.ReadNext();
            int indexId = pointReader.ReadInt32();
            long packetTimestampTicks = pointReader.ReadInt64();
            byte[] packet = pointReader.ReadBytes(1248);
            int l = packet.Length - 6; // this is the end of the pack!
            double ts = BitConverter.ToUInt32(new byte[] { packet[l], packet[l + 1], packet[l + 2], packet[l + 3] }, 0) / 1000000.00;

            pointReader.BaseStream.Seek(pos, SeekOrigin.Begin);
            return ts;
        }*/

        public IndexData FindIndexByTime(DateTime timeStamp, SearchType searchType = SearchType.FLOOR)
        {
            // TODO: do some binary search here...
            //Indeces.BinarySearch(nmeaTimeStamp, new NmeaTimestampComperer());

            for(int i = 0; i < Indeces.Count(); i++ )
            {
                if (Indeces[i].InternalTimeStamp > timeStamp)
                {
                    if (searchType == SearchType.FLOOR)
                    {
                        int k = i - 1 < 0 ? 0 : i - 1;
                        return Indeces[k];
                    }
                    else if (searchType == SearchType.CEIL)
                    {
                        return Indeces[i];
                    }
                    else if (searchType == SearchType.CLOSEST)
                    {
                        int k = i - 1 < 0 ? 0 : i - 1;

                        if (Math.Abs((Indeces[k].InternalTimeStamp - timeStamp).Ticks) > Math.Abs((Indeces[i].InternalTimeStamp - timeStamp).Ticks))
                        {
                            return Indeces[i];
                        }

                        if (Math.Abs((Indeces[k].InternalTimeStamp - timeStamp).Ticks) < Math.Abs((Indeces[i].InternalTimeStamp - timeStamp).Ticks))
                        {
                            return Indeces[k];
                        }

                    }
                }
            }

            return Indeces.Last();
        }      

        public VelodynePacket ReadNext()
        {
            if (pointReader.BaseStream.Position >= pointReader.BaseStream.Length)
            {
                return new VelodyneEndOfFile();
            }

            long packetTimestampTicks = pointReader.ReadInt64();
            DateTime baseTime = new DateTime(packetTimestampTicks);
            baseTime = baseTime.ToUniversalTime();
            DateTime dateTopOfTheHour = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, 0, 0);

            /*if (nextPacket == null)
            {
                nextPacket = pointReader.ReadBytes(1248);
            }*/

            byte[] packet = pointReader.ReadBytes(1248);
            VelodynePointPacket pointPacket = (VelodynePointPacket)PacketInterpreter.ReadRecord(packet, dateTopOfTheHour);
            pointPacket.PacketTimestamp = new DateTime(packetTimestampTicks);
            //nextPacket = pointReader.ReadBytes(1248);

            return pointPacket;
        }

        VelodynePoint lastPoint = null;
        public List<VelodynePoint> ReadNextFrame()
        {
           List<VelodynePoint> points = new List<VelodynePoint>();

            bool isGo = true;
            while (isGo)
            {
                VelodynePacket packet = ReadNext();

                if (packet is VelodynePacket)
                {
                    VelodynePointPacket pointPacket = packet as VelodynePointPacket;
                    foreach (VelodynePoint pt in pointPacket.Points)
                    {
                        points.Add(pt);
                        if ((lastPoint != null) && ((pt.Hz - lastPoint.Hz) < 0))
                        {                            
                            isGo = false;
                            break;
                        }
                    }

                    lastPoint = pointPacket.Points.Last();
                }
            }

            return points;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (pointReader != null) pointReader.Close();
                    pointReader = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~VelodyneReader() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
