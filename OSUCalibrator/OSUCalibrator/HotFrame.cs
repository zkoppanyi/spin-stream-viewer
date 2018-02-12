using OSUCalibrator.DataStreams;
using OSUCalibrator.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator
{
    [Serializable]
    public class HotFrame
    {
        public DateTime Timestamp { get; private set; }
        public List<Feature> Features { get; private set; }

        public HotFrame(DateTime timestamp)
        {
            this.Timestamp = timestamp;
            this.Features = new List<Feature>();
        }

        public override string ToString()
        {
            return this.Timestamp.ToString("HH:mm:ss.fff (" + Features.Count + ")");
        }

        public ImageFeature GetImageFeatureByDataStream(DataStream dataStream)
        {
            ImageFeature feature = (ImageFeature)this.Features.Find(x => (x is ImageFeature) && (x.DataStream == dataStream));

            // add feature is needed
            if (feature == null)
            {
                if (dataStream is ImageDataStream)
                {
                    feature = new ImageFeature(dataStream as ImageDataStream);
                }
                else if (dataStream is VideoDataStream)
                {
                    feature = new ImageFeature(dataStream as VideoDataStream);
                }
                else
                {
                    throw new NotImplementedException();
                }

                this.Features.Add(feature);
            }

            return feature;
        }
    }
}
