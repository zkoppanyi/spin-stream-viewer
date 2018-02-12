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
    public class NovatelDataStream : GPSDataStream
    {

        public NovatelDataStream(Project project, string name, string shortName, string subFolder) : base(project, name, shortName, subFolder)
        {
        }

        public override void WriteMetadata(ILogger writer, CancellationToken cancelletionToken = default(CancellationToken))
        {
            base.WriteMetadata(writer);

            String subFolder = this.project.Folder + "\\" + this.SubFolder;
            if (Directory.Exists(subFolder) == false)
            {
                writer.WriteLineInfo("!!!   Folder does not exist! Data stream is missing!");
                writer.WriteLineWarning(this.Name + ":Folder does not exist! Data stream is missing!");
                return;
            }

            // MARKERTIME logs
            string[] allFiles = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.*");
            string[] files2 = allFiles.Where(s => s.EndsWith(".ASC")).ToArray();

            if (files2.Count() > 0)
            {
                MarkerEvents.Clear();
                foreach (string fpath in files2)
                {
                    using (StreamReader reader = File.OpenText(fpath))
                    {
                        string line = "";
                        String msg = "#MARKTIMEA";
                        int line_num = 1;
                        while ((line = reader.ReadLine()) != null)
                        {
                            cancelletionToken.ThrowIfCancellationRequested();

                            if (line.Substring(0, msg.Length) == msg)
                            {
                                string[] linep = line.Split(',');
                                if (linep.Count() < 7)
                                {
                                    writer.WriteLineInfo("\tMarker ASC file is invalid : " + fpath + " line: " + line_num);
                                    writer.WriteLineWarning(this.Name + ":Marker ASC file is invalid: " + fpath + " line: " + line_num);
                                    continue;
                                }

                                String tss = linep[6];

                                double tow;
                                if (Double.TryParse(tss, out tow) == false)
                                {
                                    writer.WriteLineInfo("\tMarker ASC file is invalid : " + fpath + " line: " + line_num);
                                    writer.WriteLineWarning(this.Name + ":Marker ASC file is invalid: " + fpath + " line: " + line_num);
                                    break;
                                }

                                double days = Math.Floor(tow / 3600.00 / 24.0);
                                TimeSpan tod = TimeSpan.FromSeconds(tow - days * 24.0 * 3600.0);
                                DateTime ts = this.StartTime.Date + tod;

                                EvenMarkerDataLine evnt = new EvenMarkerDataLine(ts, TimeType.GPS);
                                MarkerEvents.Add(evnt);
                                line_num++;
                            }
                        }
                    }
                }

                this.OrderDataLines();

                //writer.WriteLineInfo("\t             Length of observ. : " + (new TimeSpan(endTime.Ticks - startTime.Ticks)).ToString(@"hh\:mm\:ss"));
                if (MarkerEvents.Count > 0)
                {
                    writer.WriteLineInfo("\t   Number of #MARKTIMEA events : " + MarkerEvents.Count);
                    writer.WriteLineInfo("\t              First event time : " + MarkerEvents.Min(m => m.TimeStamp).ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLineInfo("\t               Last event time : " + MarkerEvents.Max(m => m.TimeStamp).ToString("yyyy-MM-dd HH:mm:ss"));
                }

                // check event consistency
                for (int i = 1; i < this.MarkerEvents.Count(); i++)
                {
                    TimeSpan dt = this.MarkerEvents[i].TimeStamp - this.MarkerEvents[i - 1].TimeStamp;
                    if (dt.TotalSeconds > 10)
                    {
                        writer.WriteLineWarning("-- Event time jump in " + this.ShortName + " between " +
                            this.MarkerEvents[i - 1].TimeStamp.ToString("HH:mm:ss") + " and " + this.MarkerEvents[i].TimeStamp.ToString("HH:mm:ss") + " dt =" + dt.TotalSeconds);
                    }
                }
            }
            else
            {
                writer.WriteLineInfo("\tNo ASC file!");
                writer.WriteLineWarning(this.Name + ": No ASC file!");
            }
        }
    }
}
