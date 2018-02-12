using OSUCalibrator.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Loggers
{
    public interface ILogger : IDisposable
    {
        void Flush();

        void WriteInfo(String str);
        void WriteLineInfo(String str);

        void WriteWarning(String str);
        void WriteLineWarning(String str);

        void WriteError(String str);
        void WriteLineError(String str);

        void WriteProgress(double percnt);



    }
}
