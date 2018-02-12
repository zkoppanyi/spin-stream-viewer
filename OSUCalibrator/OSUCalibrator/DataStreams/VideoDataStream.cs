using Accord.Video.FFMPEG;
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
    public class VideoDataStream : DataStream
    {
        public override DateTime EndTime { get { return DataLines.Count > 0 ? DataLines.Max(x => x.TimeStamp + ((LongFileDataLine)x).Length) : DateTime.MaxValue; } }

        public VideoDataStream(Project project, string name, string shortName, string subFolder) : base(project, name, shortName, subFolder)
        {

        }

        public override void WriteMetadata(ILogger writer, CancellationToken cancelletionToken = default(CancellationToken))
        {
            CreateMetadataHeader(writer);

            String subFolder = this.project.Folder + "\\" + this.SubFolder;
            writer?.WriteLineInfo("Data location:           " + subFolder);
            writer?.WriteLineInfo(" ");

            if (Directory.Exists(subFolder) == false)
            {
                writer?.WriteLineInfo("!!!   Folder does not exist! Data stream is missing!");
                writer?.WriteLineWarning(this.Name + ":Folder does not exist! Data stream is missing!");
                return;
            }

            string[] allFiles = Directory.GetFiles(project.Folder + "\\" + this.SubFolder, "*.*");
            string[] files = allFiles.Where(s => s.ToUpper().EndsWith(".MOV") ||  s.ToUpper().EndsWith(".MP4")).ToArray();

            writer?.WriteLineInfo(" Files: ");
            double sumLength = 0;
            DataLines.Clear();
            foreach (String file in files)
            {
                cancelletionToken.ThrowIfCancellationRequested();

                try
                {
                    if (Path.GetExtension(file).ToUpper() == ".MOV")
                    {
                        writer?.WriteWarning("Enviroment cannot handle MOV files!");
                        continue;
                    }

                    writer?.WriteLineInfo("\t" + file);
                    VideoFileReader reader = new VideoFileReader();
                    reader.Open(file);
                    DateTime ft = File.GetLastWriteTime(file);

                    writer?.WriteLineInfo("\t   File modified :  " + ft);
                    DateTime utcf = File.GetLastWriteTimeUtc(file);

                    writer?.WriteLineInfo("\t   File modified :  " + utcf + " UTC");
                    writer?.WriteLineInfo("\t   Frame width   :  " + reader.Width);
                    writer?.WriteLineInfo("\t   Frame height  :  " + reader.Height);
                    writer?.WriteLineInfo("\t   Fps           :  " + reader.FrameRate.ToDouble());
                    writer?.WriteLineInfo("\t   Codec         :  " + reader.CodecName);
                    double length = reader.FrameCount / reader.FrameRate.ToDouble();
                    sumLength += length;
                    TimeSpan t = TimeSpan.FromSeconds(length);
                    writer?.WriteLineInfo("\t   Length        :  " + t.ToString(@"hh\:mm\:ss"));
                    writer?.WriteLineInfo(" ");

                    string rootFolder = project.Folder + "\\" + this.SubFolder + "\\";
                    String subFile = file.Replace(rootFolder, "");
                    VideoDataLine dataLine = new VideoDataLine(subFile, new DateTime(utcf.Ticks - TimeSpan.FromSeconds(length).Ticks), TimeType.FILE);
                    dataLine.FrameCount = reader.FrameCount;
                    dataLine.FrameRate = reader.FrameRate.ToDouble();

                    if (this.ShortName.Contains("GPR"))
                    {
                        dataLine.TimeType = TimeType.UNKNOWN;
                    }

                    dataLine.Length = TimeSpan.FromSeconds(length);
                    this.DataLines.Add(dataLine);
                    reader.Close();
                }
                catch(Exception ex)
                {
                    writer.WriteLineError(ex.Message);
                }
            }

            if (DataLines.Count() == 0)
            {
                writer.WriteLineInfo("Empty folder!");
                writer.WriteLineWarning(this.Name + ": Empty folder!");
            }
            this.OrderDataLines();

            int dn = Math.Abs(allFiles.Count() - DataLines.Count());
            if (dn != 0)
            {
                writer.WriteLineInfo("\tNumber of unidentified files in the folder: " + dn);
                writer.WriteLineWarning(this.Name + ": Number of unidentified files in the folder: " + dn);
            }
        }   

        /// <summary>
        /// It fixes the time tags according to the length of the videos relative to the first video.
        /// This method assumes that the videos are recorded sequnetially. 
        /// </summary>
        public void FixTimestampsByOrder(ILogger writer = null)
        {
            writer?.WriteLineInfo("Fix timestamps for " + this.Name);
            for(int i = 0; i < DataLines.Count - 1; i++)
            {
                VideoDataLine currLine = DataLines[i] as VideoDataLine;
                VideoDataLine nextLine = DataLines[i+1] as VideoDataLine;
                double dt = currLine.FrameCount * (1 / currLine.FrameRate);
                DateTime original = nextLine.TimeStamp;
                nextLine.TimeStamp = currLine.TimeStamp.AddSeconds(dt);

                writer?.WriteLineInfo("File: " + nextLine.VideoFileName);
                writer?.WriteLineInfo(" Original: " + original.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                writer?.WriteLineInfo("      New: " + nextLine.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                writer?.WriteLineInfo("     Diff: " + new TimeSpan(nextLine.TimeStamp.Ticks - original.Ticks).ToString(@"hh\:mm\:ss\.fff"));

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
                var videoDataLine = dataLine as VideoDataLine;
                String file = rootFolder + videoDataLine.VideoFileName;
                DateTime fileTime = File.GetLastWriteTimeUtc(file);
                videoDataLine.FileTimeStamp = fileTime;
            }

        }

        /// <summary>
        /// Add constant delay to all video files
        /// </summary>
        /// <param name="delay">Delay in seconds</param>
        /// <param name="timeType">Time type</param>
        public void ApplyDelay(double delay, TimeType timeType)
        {
            foreach (VideoDataLine dataLine in DataLines)
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
            foreach (VideoDataLine dataLine in DataLines)
            {
                dataLine.TimeStamp = dataLine.FileTimeStamp.AddSeconds(delay);
                dataLine.TimeType = timeType;
            }
        }

    }
}
