using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    public class ImageFeature : Feature
    {
        public List<ImagePoint> Points { get; private set; }

        [NonSerialized]
        private Image image;

        public Image Image { get { return image; } set { image = value; } }
        public DateTime TimeStamp { get; set; }

        private ImageFeature()
        {
            Points = new List<ImagePoint>();
            image = null;
        }

        public ImageFeature(ImageDataStream dataStream) : this()
        {
            this.DataStream = dataStream;
        }

        public ImageFeature(VideoDataStream dataStream) : this()
        {
            this.DataStream = dataStream;
        }

    }
   
}
