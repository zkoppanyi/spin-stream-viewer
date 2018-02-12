using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Loggers
{
    class FileLogger : ILogger
    {
        StreamWriter swLog = null;
        StreamWriter swWarning = null;

        public String logFile;
        public String warningFile;

        private FileLogger(String logFile, String warningFile)
        {
            this.logFile = logFile;
            this.warningFile = warningFile;
        }

        public static FileLogger Create(String logFile, String warningFile)
        {
            FileLogger ret = new FileLogger(logFile, warningFile); ;
            ret.Open();
            return ret;
        }


        private void Open()
        {
            //this.Assert((swLog != null) || (swLog.BaseStream != null), "Logger is still open! Close the object before call Open()!");
            if ((swLog == null) || (swLog.BaseStream == null))
            {
                swLog = File.CreateText(logFile);
                swWarning = File.CreateText(warningFile);
            }
            else
            {
                new Exception("Logger is still open! Close the object before call Open()!");
            }
        }

        public void Flush()
        {
            swLog.Flush();
            swWarning.Flush();
        }

        public  void WriteError(string str)
        {
            swLog.Write(str);
            swWarning.Write(str);
        }

        public void WriteInfo(string str)
        {
            swLog.Write(str);
        }

        public void WriteWarning(string str)
        {
            swWarning.Write(str);
        }

        public void WriteLineError(string str)
        {
            WriteError(str + Environment.NewLine);
        }

        public void WriteLineInfo(string str)
        {
            WriteInfo(str + Environment.NewLine);
        }

        public void WriteLineWarning(string str)
        {
            WriteWarning(str + Environment.NewLine);
        }

        public void WriteProgress(double percnt)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    swLog.Close();
                    swLog.Dispose();
                    swWarning.Close();
                    swWarning.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FileLogger() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
