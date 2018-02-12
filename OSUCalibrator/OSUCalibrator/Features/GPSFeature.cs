using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    public class GPSFeature : Feature
    {
        public GPSPositionDataLine Position { get; set; }

        public GPSFeature(DataStream stream, GPSPositionDataLine position)
        {
            this.DataStream = stream;
            this.Position = position;
        }


    }
}
