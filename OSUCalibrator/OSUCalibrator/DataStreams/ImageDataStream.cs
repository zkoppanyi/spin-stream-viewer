using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSUCalibrator.DataStreams
{
    [Serializable]
    public class ImageDataStream : DataStream
    {

        public ImageDataStream(Project project, string name, string shortName, string subFolder) : base(project, name, shortName, subFolder)
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

            string rootFolder = project.Folder + "\\" + this.SubFolder + "\\";
            string[] allFiles = Directory.GetFiles(rootFolder, "*.*", SearchOption.AllDirectories);
            string[] files = allFiles.Where(s => s.EndsWith(".JPG")).ToArray();

            foreach (String file in files)
            {
                cancelletionToken.ThrowIfCancellationRequested();

                DateTime fileTime = File.GetLastWriteTimeUtc(file);
                String subFile = file.Replace(rootFolder, "");
                this.DataLines.Add(new ImageDataLine(subFile, fileTime, TimeType.FILE));
            }


            writer.WriteLineInfo("\t        Total number of images : " + DataLines.Count());
            writer.WriteLineInfo("\t            First file created : " + this.StartTime);
            writer.WriteLineInfo("\t             Last file created : " + this.EndTime);
            writer.WriteLineInfo("\t              Frame per second : " + (DataLines.Count() /this.Length).ToString("0.000") + " [fps]");
            writer.WriteLineInfo("\t                     Frequency : " + (this.Length / files.Count()).ToString("0.000") + " [Hz]");

            this.OrderDataLines();

            if (files.Count() == 0)
            {
                writer.WriteLineInfo("Empty folder!");
                writer.WriteLineWarning(this.Name + ":Empty folder!");
            }

            int dn = Math.Abs(allFiles.Count() - files.Count());
            if (dn != 0)
            {
                writer.WriteLineInfo("\t     No. of unidentified files : " + dn);
                writer.WriteLineWarning(this.Name + ":Number of unidentified files in the folder: " + dn);
            }
        }

        /// <summary>
        /// Update DataLines filetime field
        /// </summary>  
        public void UpdateFileTimes()
        {          
            String subFolder = this.project.Folder + "\\" + this.SubFolder;
            
            if (Directory.Exists(subFolder) == false)
            {
                throw new Exception("Directory does not exist: " + subFolder);
            }

            string rootFolder = project.Folder + "\\" + this.SubFolder + "\\";

            foreach (DataLine dataLine in this.DataLines)
            {
                ImageDataLine imageDataLine = dataLine as ImageDataLine;
                String file = rootFolder + imageDataLine.ImageFileName;
                DateTime fileTime = File.GetLastWriteTimeUtc(file);
                imageDataLine.FileTimeStamp = fileTime;
            }

        }

        /// <summary>
        /// Add constant delay to all video files from the current timestamp
        /// </summary>
        /// <param name="delay">Delay in seconds</param>
        /// <param name="timeType">Time type</param>
        public void ApplyDelay(double delay, TimeType timeType)
        {
            foreach (ImageDataLine dataLine in DataLines)
            {
                dataLine.TimeStamp = dataLine.TimeStamp.AddSeconds(delay);
                dataLine.TimeType = timeType;
            }

        }

        /// <summary>
        /// Add constant delay to all video files from the current file timestamp
        /// </summary>
        /// <param name="delay">Delay in seconds</param>
        /// <param name="timeType">Time type</param>
        public void ApplyDelayFromFileTime(double delay, TimeType timeType)
        {
            foreach (ImageDataLine dataLine in DataLines)
            {
                dataLine.TimeStamp = dataLine.FileTimeStamp.AddSeconds(delay);
                dataLine.TimeType = timeType;
            }

        }

    }
}
