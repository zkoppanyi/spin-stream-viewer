using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    public abstract class Feature
    {
        public DataStream DataStream { get; protected set; }
    }
}
