using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.DataStreams
{
    public interface VData
    {

    }

    [Serializable]
    public class VEof : VData
    {

    }

    [Serializable]
    public class VPos : VData
    {
        public DateTime GPSTime;
        public int LatDeg = 0;
        public double LatMin = 0;
        public int LonDeg = 0;
        public double LonMin = 0;
        public long PointIdx = -1;

        public VPos(DateTime gpsTime)
        {
            this.GPSTime = gpsTime;
        }

        public void Write(BinaryWriter stream)
        {
            stream.Write(this.GPSTime.Hour); // int
            stream.Write(this.GPSTime.Minute); // int
            stream.Write(this.GPSTime.Second); // int
            stream.Write(this.LatDeg); // int
            stream.Write(this.LatMin); // double
            stream.Write(this.LonDeg); // int
            stream.Write(this.LonMin); // double
            stream.Write(this.PointIdx); // long
        }
    }

    [Serializable]
    public class VPoint3d : VData
    {
        public DateTime PacketTimestamp;
        public DateTime LastNMEATime;
        public double InternalTime;
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public double r = 0;
        public double Hz = 0;
        public double Vz = 0;
        public byte intensity = 0;

        public VPoint3d()
        {
            LastNMEATime = DateTime.MaxValue;
            PacketTimestamp = DateTime.MaxValue;
            InternalTime = -1;
        }

        public VPoint3d(double x, double y, double z, double r, double Hz, double Vz, byte intensity) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
            this.Hz = Hz;
            this.Vz = Vz;
            this.intensity = intensity;
        }

        public void Write(BinaryWriter stream)
        {
            //stream.Write(this.x); // double
            //stream.Write(this.y); // double
            //stream.Write(this.z); // double

            stream.Write(this.r); // double
            stream.Write(this.Hz); // double
            stream.Write(this.Vz); // double            
            stream.Write(this.intensity); // byte
            stream.Write(this.InternalTime); // double

            /*stream.Write(this.LastNMEATime.Hour); // int
            stream.Write(this.LastNMEATime.Minute); // int
            stream.Write(this.LastNMEATime.Second); // int

            stream.Write(this.PacketTimestamp.Hour); // int
            stream.Write(this.PacketTimestamp.Minute); // int
            stream.Write(this.PacketTimestamp.Second); // int
            stream.Write(this.PacketTimestamp.Millisecond); // int
            */
        }
    }
}
