using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSUCalibrator.Loggers;
using System.IO;
using System.Threading;

namespace OSUCalibrator.DataStreams
{
    [Serializable]
    public class MicroStrainDataStream : DataStream
    {
        public override DateTime EndTime { get { return DataLines.Count > 0 ? DataLines.Max(x => x.TimeStamp + ((LongFileDataLine)x).Length) : DateTime.MaxValue; } }

        public MicroStrainDataStream(Project project, string name, string shortName, string subFolder) : base(project, name, shortName, subFolder)
        {
        }

        public override void WriteMetadata(ILogger writer, CancellationToken cancelletionToken = default(CancellationToken))
        {
            CreateMetadataHeader(writer);

            String subFolder = this.project.Folder + "\\" + this.SubFolder;
            writer.WriteLineInfo("Data location:           " + subFolder);
            writer.WriteLineInfo(" ");

            if (Directory.Exists(subFolder) == false)
            {
                writer.WriteLineInfo("!!!   Folder does not exist! Data stream is missing!");
                writer.WriteLineWarning(this.Name + ":Folder does not exist! Data stream is missing!");
                return;
            }

            // IMU CSV logs
            string[] allFiles = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.csv");
            string[] files = allFiles;

            if (files.Count() > 0)
            {
                DateTime startTime = DateTime.MaxValue;
                DateTime endTime = DateTime.MinValue;
                foreach (string fpath in files)
                {
                    DateTime startTimef = DateTime.MaxValue;
                    DateTime endTimef = DateTime.MinValue;
                    using (StreamReader reader = File.OpenText(fpath))
                    {
                        // skip header
                        for (int i = 0; i < 17; i++)
                        {
                            reader.ReadLine();
                        }

                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            cancelletionToken.ThrowIfCancellationRequested();

                            string[] linesp = line.Split(',');

                            if (linesp.Count() > 3)
                            {
                                if (linesp[0] == "") continue;
                                int week = Convert.ToInt32(linesp[1]);
                                double tow = Convert.ToDouble(linesp[2]);
                                DateTime dt = Utils.ConvertFromTOW(week, tow);

                                if (dt < startTime) startTime = dt;
                                if (dt > endTime) endTime = dt;
                                if (dt < startTimef) startTimef = dt;
                                if (dt > endTimef) endTimef = dt;
                            }
                        }
                    }

                    IMUDataLine dataLine = new IMUDataLine(fpath, startTimef, TimeType.GPS);
                    dataLine.Length = new TimeSpan(endTimef.Ticks - startTimef.Ticks);
                    DataLines.Add(dataLine);

                }

                if (this.Length == 0)
                {
                    writer.WriteLineInfo("\tIMU observation is too short!");
                    writer.WriteLineWarning(this.Name + ":IMU observation is too short!");
                }
                writer.WriteLineInfo("\t             First observation : " + this.StartTime);
                writer.WriteLineInfo("\t              Last observation : " + this.EndTime);
                writer.WriteLineInfo("\t             Length of observ. : " + (new TimeSpan(endTime.Ticks - startTime.Ticks)).ToString(@"hh\:mm\:ss"));
                this.OrderDataLines();
            }
            else
            {
                writer.WriteLineInfo("\tNo observation file!");
                writer.WriteLineWarning(this.Name + ":No observation file!");
            }
        }
    }
}
