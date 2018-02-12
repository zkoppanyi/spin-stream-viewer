using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator.Loggers
{
    public class ConsoleWndLogger : ILogger
    {
        private MainForm mainForm;
        private StringBuilder consoleText;
        public StringBuilder ConsoleText { get { return consoleText;  } }

        private ConsoleWndLogger(MainForm mainForm)
        {
            this.mainForm = mainForm;
            consoleText = new StringBuilder();
        }

        public static ConsoleWndLogger Create(MainForm mainForm)
        {
            ConsoleWndLogger ret = new ConsoleWndLogger(mainForm);
            return ret;
        }

        public void Dispose()
        {
           
        }

        public void Flush()
        {
            
        }

        public void WriteError(string str)
        {
            String strApp = str;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;            

            mainForm.Console.Write(strApp);
        }

        public void WriteInfo(string str)
        {
            String strApp = str;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;

            mainForm.Console.Write(strApp);
        }

        public void WriteLineError(string str)
        {
            String strApp = "!! ERR: " + str + Environment.NewLine;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;

            mainForm.Console.Write(strApp);
        }

        public void WriteLineInfo(string str)
        {
            String strApp = "-- INF: " + str + Environment.NewLine;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;

            mainForm.Console.Write(strApp);
        }

        public void WriteLineWarning(string str)
        {
            String strApp = "-! WRN: " + str + Environment.NewLine;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;

            mainForm.Console.Write(strApp);
        }

        public void WriteWarning(string str)
        {
            String strApp = str;
            consoleText.Append(strApp);
            if (mainForm.Console == null) return;
            if (mainForm.Console.IsDisposed) return;

            mainForm.Console.Write(strApp);
        }

        public void WriteProgress(double percnt)
        {
            mainForm.ReportProgress(percnt);
        }
    }
}
