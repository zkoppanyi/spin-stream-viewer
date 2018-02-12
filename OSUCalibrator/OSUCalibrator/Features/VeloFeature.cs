using OSUCalibrator.DataStreams;
using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Features
{
    [Serializable]
    public class VeloFeature : Feature
    {
        public int ID { get; set; }
        public List<VelodynePoint> Points { get; private set; }

        private VeloFeature()
        {
            this.ID = 1;
            this.Points = new List<VelodynePoint>();
        }

        public VeloFeature(VelodyneDataStream dataStream) : this()
        {
            this.DataStream = dataStream;
        }

        public VelodyneAnnotation GetAnnotiation()
        {
            double sx = 0;
            double sy = 0;
            double sz = 0;

            foreach (VelodynePoint pt in Points)
            {
                sx += pt.X;
                sy += pt.Y;
                sz += pt.Z;
            }

            int n = Points.Count();
            VelodyneAnnotation annot = new VelodyneAnnotation(sx / n, sy / n, sz / n);
            annot.ID = ID;
            annot.Object = this;

            return annot;
        }
    }
}
