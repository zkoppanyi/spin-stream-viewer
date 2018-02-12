using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSUCalibrator.Loggers;
using System.IO;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using SharpVelodyne;
using System.Threading;

namespace OSUCalibrator.DataStreams
{
    [Serializable]
    public partial class VelodyneDataStream : DataStream
    {
        public override DateTime EndTime { get { return DataLines.Count > 0 ? DataLines.Max(x => x.TimeStamp + ((LongFileDataLine)x).Length) : DateTime.MaxValue; } }
        public VelodyneSensorType SensorType { get; private set; }
        public double[,] Tp { get; set; }

        public VelodyneDataStream(Project project, string name, string shortName, string subFolder, VelodyneSensorType sensorType) : base(project, name, shortName, subFolder)
        {
            this.SensorType = sensorType;
        }

        public bool HasIndexedFiles()
        {
            foreach(VelodyneDataLine dataLine in this.DataLines)
            {
                String pcapFile = this.project.Folder + "\\" + this.SubFolder + "\\" + dataLine.PcapLocation;
                if ( (!(File.Exists(VelodyneConverter.GetDefaultIndexFile(pcapFile)))) || (!(File.Exists(VelodyneConverter.GetDefaultIndexFile(pcapFile)))))
                {
                    return false;
                }
            }

            return true;
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

            string[] files = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.pcap");

            writer.WriteLineInfo(" Files: ");
            foreach (String file in files)
            {
                cancelletionToken.ThrowIfCancellationRequested();
                writer.WriteLineInfo("\t" + file);

                String nmea = null;
                try
                {
                    //nmea = this.ReadFirstNMEA(file);
                }
                catch (Exception ex)
                {
                    writer.WriteLineWarning("Cannot open PCAP file: " + file);
                    writer.WriteLineWarning("Exception: " + ex.ToString());
                    writer.WriteLineWarning("Is the WINPCAP installed? Download from https://www.winpcap.org/install/!");
                    continue;
                }
                //break;

                FileInfo fileInfo = new System.IO.FileInfo(file);
                DateTime ft = File.GetLastWriteTime(file);

                writer.WriteLineInfo("\t   File modified :  " + ft);
                DateTime uft = File.GetLastWriteTimeUtc(file);
                writer.WriteLineInfo("\t   File modified :  " + uft + " UTC");
                writer.WriteLineInfo("\t            Size :  " + fileInfo.Length / 1e6 + " MB");
                writer.WriteLineInfo("\t      first NMEA :  " + nmea);

                if (nmea != null)
                {                    
                    String[] tsnmeas = nmea.Split(',');
                    String dateStr = uft.ToString("yyyy-MM-dd ") + tsnmeas[1].Substring(0, 2) + ":" + tsnmeas[1].Substring(2, 2) + ":" + tsnmeas[1].Substring(4, 2);
                    DateTime time_nmea = DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    writer.WriteLineInfo("\t       Date time :  " + time_nmea);

                    writer.WriteLineInfo(" ");
                    VelodyneDataLine dataLine = new VelodyneDataLine(Path.GetFileName(file), time_nmea, TimeType.GPS);
                    dataLine.Length = (new TimeSpan(uft.Ticks - time_nmea.Ticks));
                    this.DataLines.Add(dataLine);
                }
                else
                {
                    writer.WriteLineWarning("No NMEA could not be extracted from " + file);
                    VelodyneDataLine dataLine = new VelodyneDataLine(Path.GetFileName(file), uft, TimeType.CPU);
                    dataLine.Length = (new TimeSpan(0));
                    this.DataLines.Add(dataLine);
                }
            }

            this.OrderDataLines();

            if (DataLines.Count() == 0)
            {
                writer.WriteLineInfo("Empty folder!");
                writer.WriteLineWarning(this.Name + ":Empty folder!");
            }

            string[] allFiles = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.*");
            int dn = Math.Abs(allFiles.Count() - DataLines.Count());
            if (dn != 0)
            {
                writer.WriteLineInfo("\tNumber of unidentified files in the folder: " + dn);
                writer.WriteLineWarning(this.Name + ":Number of unidentified files in the folder: " + dn);
            }


        }

    }
}
