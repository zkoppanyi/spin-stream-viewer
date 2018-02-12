using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator
{
    enum StreamWindowState
    {
        Normal,
        DigitizePoints
    }

    public interface IStreamWindow
    {
        bool HasToSetFrame { get; }
        void SetFrameByTime(DateTime time);
        void SetHotFrame(HotFrame frame);

        Image GetSnapshot();
        DataStream DataStream { get; }
    }
}
