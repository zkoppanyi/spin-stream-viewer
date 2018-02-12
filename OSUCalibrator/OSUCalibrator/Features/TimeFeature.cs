using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    class TimeFeature : Feature
    {
        public DateTime OriginalTime { get; private set; }
        public DateTime GlobalTime { get; private set; }
        public String FileName { get; private set; }
        public DateTime FileTime { get; private set; }

        public TimeFeature(DataStream stream, VideoDataLine dataLine, DateTime currentTime, HotFrame hotFrame)
        {
            this.DataStream = stream;
            this.OriginalTime = currentTime;
            this.FileTime = dataLine.FileTimeStamp;
            this.FileName = dataLine.VideoFileName;
            this.GlobalTime = hotFrame.Timestamp;
        }

        public TimeFeature(DataStream stream, ImageDataLine dataLine, HotFrame hotFrame)
        {
            this.DataStream = stream;
            this.OriginalTime = dataLine.TimeStamp;
            this.FileTime = dataLine.TimeStamp;
            this.FileName = dataLine.ImageFileName;
            this.GlobalTime = hotFrame.Timestamp;
        }

    }
}
