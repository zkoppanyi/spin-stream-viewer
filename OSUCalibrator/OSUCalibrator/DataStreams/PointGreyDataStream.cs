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
    public class PointGreyDataStream : ImageDataStream
    {
        private String cameraPrefix = "";
        public String CameraPrefix { get { return cameraPrefix;  } }

        public PointGreyDataStream(Project project, string name, string shortName, string subFolder, string cameraPrefix) : base(project, name, shortName, subFolder)
        {
            this.cameraPrefix = cameraPrefix;
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
            string[] files = Directory.GetFiles(rootFolder, cameraPrefix + "*.yuv", SearchOption.AllDirectories);

            foreach (String file in files)
            {
                cancelletionToken.ThrowIfCancellationRequested();

                DateTime fileTime = File.GetLastWriteTimeUtc(file);
                String subFile = file.Replace(rootFolder, "");
                this.DataLines.Add(new ImageDataLine(subFile, fileTime, TimeType.FILE));
            }

            this.OrderDataLines();

            writer.WriteLineInfo("\t        Total number of images : " + DataLines.Count());
            writer.WriteLineInfo("\t            First file created : " + StartTime);
            writer.WriteLineInfo("\t             Last file created : " + EndTime);
            writer.WriteLineInfo("\t              Frame per second : " + (DataLines.Count() / Length).ToString("0.000") + " [fps]");
            writer.WriteLineInfo("\t                     Frequency : " + (Length / files.Count()).ToString("0.000") + " [Hz]");



            if (files.Count() == 0)
            {
                writer.WriteLineInfo("Empty folder!");
                writer.WriteLineWarning(this.Name + ":Empty folder!");
            }


        }
    }
}
