using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Loggers
{
    public class MultipleLogger : ILogger
    {
        private List<ILogger> loggers;
        public MultipleLogger(List<ILogger> loggers)
        {
            this.loggers = loggers;
        }

        public void Dispose()
        {
            foreach(ILogger logger in loggers)
            {
                logger.Dispose();
            }
        }

        public void Flush()
        {
            foreach (ILogger logger in loggers)
            {
                logger.Flush();
            }
        }

        public void WriteError(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteError(str);
            }
        }

        public void WriteInfo(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteInfo(str);
            }
        }

        public void WriteLineError(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteLineError(str);
            }
        }

        public void WriteLineInfo(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteLineInfo(str);
            }
        }

        public void WriteLineWarning(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteLineWarning(str);
            }
        }

        public void WriteWarning(string str)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteWarning(str);
            }
        }

        public void WriteProgress(double percnt)
        {
            foreach (ILogger logger in loggers)
            {
                logger.WriteProgress(percnt);
            }
        }

    }
}
