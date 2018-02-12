using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    public class ImagePoint
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public ImagePoint()
        {
            ID = 0;
            X = 0;
            Y = 0;
        }

        public ImagePoint(int ID, int X, int Y) : this()
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
        }

        public Point GetPoint()
        {
            return new Point(this.X, this.Y);
        }
    }
}
