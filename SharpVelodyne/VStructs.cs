using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public interface VelodynePacket
    {

    }

    [Serializable]
    public class VelodyneEndOfFile : VelodynePacket
    {

    }

    [Serializable]
    public class VelodyneNmeaPacket : VelodynePacket
    {
        public DateTime GPSTime { get; private set; }
        public int LatDeg { get; private set; }
        public double LatMin { get; private set; }
        public int LonDeg { get; private set; }
        public double LonMin { get; private set; }
        public long? PointIdx { get; private set; }
        public string NmeaString { get; private set; }

        private VelodyneNmeaPacket(DateTime gpsTime)
        {
            this.GPSTime = gpsTime;
            PointIdx = null;
            LatDeg = 0;
            LatMin = 0;
            LonDeg = 0;
            LonMin = 0;
            NmeaString = "";
        }

        public static VelodyneNmeaPacket Parse(String nmeaStr)
        {
            try
            {
                String[] tsnmeas = nmeaStr.Split(','); // hhmmss.ss
                if (tsnmeas.Length > 0)
                {
                    String dateStr = default(DateTime).ToString("yyyy-MM-dd ") + tsnmeas[1].Substring(0, 2) + ":" + tsnmeas[1].Substring(2, 2) + ":" + tsnmeas[1].Substring(4, 2);
                    DateTime time_nmea = DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    VelodyneNmeaPacket packet = new VelodyneNmeaPacket(time_nmea);
                    packet.LatDeg = Convert.ToInt32(tsnmeas[3].Substring(0, 2));
                    packet.LatMin = Convert.ToDouble(tsnmeas[3].Substring(2, 6));
                    packet.LonDeg = Convert.ToInt32(tsnmeas[5].Substring(0, 3));
                    packet.LonMin = Convert.ToDouble(tsnmeas[5].Substring(3, 6));
                    packet.NmeaString = nmeaStr;
                    return packet;
                }
                else
                {
                    //throw new Exception("NMEA seems empty: " + nmeaStr);
                    return null;
                }
            }
            catch (Exception)
            {
                //throw new Exception("Couldn't parse NMEA data! NMEA message: " + nmeaStr + " Excaption: " + ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class PointLikeObject : ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PointLikeObject() : this(0,0,0)
        {

        }

        /// <summary>
        /// Constructor with coordinate initializtion
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        public PointLikeObject(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        protected PointLikeObject(PointLikeObject obj)
        {
            this.X = obj.X;
            this.Y = obj.Y;
            this.Z = obj.Z;
        }

        public virtual object Clone()
        {
            return new PointLikeObject(this);
        }
    }

    [Serializable]
    public class VelodyneAnnotation : PointLikeObject
    {
        /// <summary>
        /// Some ID for the annotation; this is used for visualization
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Some text to be displayed; empty string for skipping
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Color of the text
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Some attached object, if needed
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public VelodyneAnnotation() : this(0,0,0)
        {
           
        }

        /// <summary>
        /// Constructor with coordinate initializtion
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        public VelodyneAnnotation(double X, double Y, double Z) : base(X, Y, Z)
        {
            this.ID = 1;
            this.Color = Color.Red;
            this.Text = "";
            this.Object = null;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="obj">Copy object</param>
        protected VelodyneAnnotation(VelodyneAnnotation obj) : base(obj)
        {
            this.ID = obj.ID;
            this.Text = obj.Text;
            this.Color = obj.Color;
            this.Object = obj.Object;
        }

        /// <summary>
        /// Deep cloning
        /// </summary>
        /// <returns>Cloned object</returns>
        public override object Clone()
        {
            return new VelodyneAnnotation(this);
        }
    }

    [Serializable]
    public enum ReturnType
    {
        StrongestReturn,
        LastReturn
    }

    [Serializable]
    public class VelodynePoint : PointLikeObject
    {
        /// <summary>
        /// Internal timestamp as top of the hour
        /// </summary>
        public double InternalTime { get; set; }

        /// <summary>
        /// Global timestamp
        /// </summary>
        public DateTime? Timestamp { get; set; }
                
        /// <summary>
        /// Distance of firing
        /// </summary>
        public double Distance = 0;

        /// <summary>
        /// Horizontal angle of firing
        /// </summary>
        public double Hz = 0;

        /// <summary>
        /// Vertical angle of firing
        /// </summary>
        public double Vz = 0;

        /// <summary>
        /// Intensity value
        /// </summary>
        public byte Intensity = 0;

        /// <summary>
        /// Diod Id
        /// </summary>
        public int DiodId = -1;

        /// <summary>
        /// Return type: strongest or last (valid for VLP-16, always strongest for HDL-32E)
        /// </summary>
        public ReturnType ReturnType { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public VelodynePoint() : this(0, 0, 0, 0, 0, 0, 0, -1, ReturnType.StrongestReturn)
        {
           
        }

        /// <summary>
        /// Constructor with some parameters
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        /// <param name="r">Distance of firing</param>
        /// <param name="Hz">Horizontal angle of firing</param>
        /// <param name="Vz">Vertical angle of firing</param>
        /// <param name="intensity">Intensity value</param>
        /// <param name="diodId">Diod ID</param>
        public VelodynePoint(double X, double Y, double Z, double r, double Hz, double Vz, byte intensity, int diodId, ReturnType returnType) : this(-1, null, X, Y, Z, r, Hz, Vz, intensity, diodId, returnType)
        {

            this.Distance = r;
            this.Hz = Hz;
            this.Vz = Vz;
            this.Intensity = intensity;
        }

        /// <summary>
        ///  Constructor with some parameters
        /// </summary>
        /// <param name="internalTimestamp">Internal timestamp as top of the hour</param>
        /// <param name="timeStamp">Global timestamp</param>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        /// <param name="r">Distance of firing</param>
        /// <param name="Hz">Horizontal angle of firing</param>
        /// <param name="Vz">Vertical angle of firing</param>
        /// <param name="intensity">Intensity value</param>
        /// <param name="diodId">Diod ID</param>
        public VelodynePoint(double internalTimestamp, DateTime? timeStamp, double X, double Y, double Z, double r, double Hz, double Vz, byte intensity, int diodId, ReturnType returnType) : base(X, Y, Z)
        {
            this.InternalTime = internalTimestamp;
            this.Timestamp = timeStamp;
            this.Distance = r;
            this.Hz = Hz;
            this.Vz = Vz;
            this.Intensity = intensity;
            this.ReturnType = returnType;
            this.DiodId = diodId;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="obj">Copy object</param>
        protected VelodynePoint(VelodynePoint obj) : base(obj)
        {
            this.InternalTime = obj.InternalTime;
            this.Timestamp = obj.Timestamp;
            this.Distance = obj.Distance;
            this.Hz = obj.Hz;
            this.Vz = obj.Vz;
            this.Intensity = obj.Intensity;
            this.ReturnType = obj.ReturnType;
        }

        /// <summary>
        /// Deep cloning
        /// </summary>
        /// <returns>Cloned object</returns>
        public override object Clone()
        {
            return new VelodynePoint(this);
        }

    }

    [Serializable]
    public class VelodynePointPacket : VelodynePacket
    {
        public DateTime? PacketTimestamp { get; set; }
        public List<VelodynePoint> Points { get; private set; }

        public VelodynePointPacket(DateTime? packetTimestamp = null, int? nmeaId = null)
        {
            Points = new List<VelodynePoint>();
            this.PacketTimestamp = packetTimestamp;
        }
    }


}
