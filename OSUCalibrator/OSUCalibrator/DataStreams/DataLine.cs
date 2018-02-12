using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.DataStreams
{
    [Serializable]
    public enum TimeType
    {
        UNKNOWN,
        ESTIMATED,
        CPU,
        FILE,
        GPS
    }

    [Serializable]
    public class DataLine
    {
        public DateTime TimeStamp { get; set; }
        public TimeType TimeType { get; set; }

        public DataLine(DateTime timeStamp, TimeType timeType)
        {
            this.TimeStamp = timeStamp;
            this.TimeType = timeType;
        }

        public DataLine(DateTime timeStamp) : this(timeStamp, TimeType.UNKNOWN)
        {

        }
    }

    [Serializable]
    public class LongFileDataLine: DataLine
    {
        public TimeSpan Length { get; set; }
        
        public LongFileDataLine(DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            Length = new TimeSpan(0);
        }

        public LongFileDataLine(DateTime timeStamp) : this(timeStamp, TimeType.UNKNOWN)
        {

        }
    }


    [Serializable]
    public class ImageDataLine : DataLine
    {
        public String ImageFileName { get; set; }
        public DateTime FileTimeStamp { get; set; }

        public ImageDataLine(String imageFileName, DateTime timeStamp, DateTime fileTime, TimeType timeType) : base(timeStamp, timeType)
        {
            this.ImageFileName = imageFileName;
            this.FileTimeStamp = fileTime;
        }

        public ImageDataLine(String imageFileName, DateTime timeStamp, TimeType timeType) : this(imageFileName, timeStamp, timeStamp, timeType)
        {
        }

        public ImageDataLine(String imageLocation, DateTime timeStamp) : this(imageLocation, timeStamp, TimeType.UNKNOWN)
        {

        }

    }

    [Serializable]
    public class VideoDataLine : LongFileDataLine
    {
        public String VideoFileName { get; set; }
        public DateTime FileTimeStamp { get; set; }
        public double FrameRate { get; set; }
        public long FrameCount { get; set; }


        public VideoDataLine(String videoFileName, DateTime timeStamp, DateTime fileTimeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            this.VideoFileName = videoFileName;
            this.FileTimeStamp= fileTimeStamp;
        }

        public VideoDataLine(String videoFileName, DateTime timeStamp, TimeType timeType) : this(videoFileName, timeStamp, timeStamp, timeType)
        {
        }

        public VideoDataLine(String videoLocation, DateTime timeStamp) : this(videoLocation, timeStamp, TimeType.UNKNOWN)
        {

        }



        public override string ToString()
        {
            return this.VideoFileName + " (" + this.TimeStamp.ToString("HH:mm:ss") + " length: " + ((1/FrameRate)*FrameCount/60.0).ToString("0.0") + " min)";
        }
    }

    [Serializable]
    public class VelodyneDataLine : LongFileDataLine
    {
        public String PcapLocation { get; set; }

        public VelodyneDataLine(String pcapLocation, DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            this.PcapLocation = pcapLocation;
        }

        public VelodyneDataLine(String videoLocation, DateTime timeStamp) : this(videoLocation, timeStamp, TimeType.UNKNOWN)
        {

        }
    }

    [Serializable]
    public class IMUDataLine : LongFileDataLine
    {
        public String PcapLocation { get; set; }

        public IMUDataLine(String pcapLocation, DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            this.PcapLocation = pcapLocation;
        }

        public IMUDataLine(String videoLocation, DateTime timeStamp) : this(videoLocation, timeStamp, TimeType.UNKNOWN)
        {

        }
    }

    [Serializable]
    public class GPSObsFileDataLine : LongFileDataLine
    {
        public String ObsFileLocation { get; set; }

        public GPSObsFileDataLine(String obsFileLocation, DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            this.ObsFileLocation = obsFileLocation;
        }

        public GPSObsFileDataLine(String obsFileLocation, DateTime timeStamp) : this(obsFileLocation, timeStamp, TimeType.UNKNOWN)
        {

        }
    }

    [Serializable]
    public class GPSPositionDataLine : DataLine
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Height { get; set; }
        public double Quality { get; set; }

        public GPSPositionDataLine(DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {

        }

        public GPSPositionDataLine(DateTime timeStamp) : this(timeStamp, TimeType.UNKNOWN)
        {

        }
    }

    [Serializable]
    public class EvenMarkerDataLine : DataLine
    {
        public enum MarkerEventPort
        {
            Unknown,
            EventA,
            EventB
        }

        public MarkerEventPort Port { get; set; }

        public EvenMarkerDataLine(DateTime timeStamp, TimeType timeType) : base(timeStamp, timeType)
        {
            this.Port = MarkerEventPort.Unknown;
        }

        public EvenMarkerDataLine(DateTime timeStamp) : this(timeStamp, TimeType.GPS)
        {

        }
    }


}
