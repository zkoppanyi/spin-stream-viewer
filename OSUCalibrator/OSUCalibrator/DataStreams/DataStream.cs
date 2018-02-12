using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSUCalibrator.DataStreams
{

    [Serializable]
    public abstract class DataStream
    {
        public String Name { get; protected set; }
        public String ShortName { get; protected set; }
        public String SubFolder { get; protected set; }

        [System.Obsolete("Delay is deprecated, changing this value does not make any effect, and don't trust the value here! Use ApplyDelay instead!")]
        public double Delay { get; protected set; }

        public double Length { get { return (new TimeSpan(this.EndTime.Ticks - this.StartTime.Ticks)).TotalSeconds; } }

        public virtual DateTime StartTime { get { return DataLines.Count > 0 ? DataLines.Min(x => x.TimeStamp) : DateTime.MaxValue;  } }
        public virtual DateTime EndTime { get { return DataLines.Count > 0 ? DataLines.Max(x => x.TimeStamp) : DateTime.MaxValue; } }

        public List<DataLine> DataLines { get; protected set; }
        public int NumOfData { get { return DataLines.Count(); } }

        protected Project project;

        public abstract void WriteMetadata(ILogger logger, CancellationToken cancelletionToken = default(CancellationToken));

        protected DataStream(Project project, String name, String shortName, String subFolder)
        {
            this.project = project;
            this.Name = name;
            this.ShortName = shortName;
            this.SubFolder = subFolder;
            this.DataLines = new List<DataLine>();
        }

        protected void CreateMetadataHeader(ILogger writer)
        {
            writer.WriteLineInfo(" ");
            writer.WriteLineInfo("---------------------------------------------");
            writer.WriteLineInfo("Data stream name        : " + this.Name);
            writer.WriteLineInfo("Data stream short name  : " + this.ShortName);
            writer.WriteLineInfo("Data location subfolder : " + this.SubFolder);
        }

        public override String ToString()
        {
            return this.ShortName  + " (" + this.Name + ") Entries: " + this.DataLines.Count();
        }

        public virtual void OrderDataLines()
        {
            DataLines.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));
        }
    }
}
