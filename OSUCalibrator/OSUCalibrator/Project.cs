using Accord.IO;
using Accord.Math;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace OSUCalibrator
{
    [Serializable]
    public class Project
    {
        public string Folder { get; set; }
        public List<DataStream> DataStreams { get; private set; }
        public int GPSTLeapSeconds { get; private set; }
        public TimeSpan UTCOffset { get; private set; }
        public List<HotFrame> HotFrames { get; private set; }

        public static string MetadataFolder { get { return "METADATA"; } }
        public static string GeorefFolder { get { return "METADATA\\GEOREF"; } }
        public static string LiDARFrameFolder { get { return "METADATA\\LiDARFrames"; } }
        private int idSequence = 0;

        public Project(string folder)
        {
            this.Folder = folder;
            this.HotFrames = new List<HotFrame>();
            this.DataStreams = new List<DataStream>();
            this.UTCOffset = TimeSpan.FromHours(4);
            this.GPSTLeapSeconds = -18;
        }

        public void Save()
        {
            var stream = File.Create(Folder + "\\" + MetadataFolder + "\\project.bin");
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public static Project Load(String folder, ILogger logger = null, CancellationToken cancelletionToken = default(CancellationToken))
        {
            var stream = File.OpenRead(folder + "\\" + MetadataFolder + "\\project.bin");
            var formatter = new BinaryFormatter();
            Project project = (Project)formatter.Deserialize(stream);
            stream.Close();

            project.Folder = folder;
            project.CreateFolderIfNotExists(MetadataFolder);
            project.CreateFolderIfNotExists(GeorefFolder);
            project.CreateFolderIfNotExists(LiDARFrameFolder);

            // update transformation matrices for sensors
            foreach (DataStream dataStream in project.DataStreams)
            {
                cancelletionToken.ThrowIfCancellationRequested();

                if (dataStream is VelodyneDataStream)
                {
                    var filePath = folder + "\\" + GeorefFolder + "\\" + dataStream.ShortName + "_Tp.txt";
                    if (File.Exists(filePath))
                    {
                        VelodyneDataStream velodyneDataStream  = dataStream as VelodyneDataStream;

                        CsvReader reader = new CsvReader(filePath, false);
                        var Tp = reader.ToMatrix();
                        velodyneDataStream.Tp = Tp;
                        logger?.WriteLineInfo(dataStream.ShortName + ": sensor-platform transformation matrix file is loaded!");
                    }
                    else
                    {
                        logger?.WriteLineWarning(dataStream.ShortName + ": sensor-platform transformation matrix file does not exist: " + filePath);
                    }
                }
            }

            return project;
        }

        public DataStream GetDataStreamByShortName(String shortName)
        {
            return DataStreams.Find(d => d.ShortName == shortName);
        }

        private void CreateFolderIfNotExists(String subFolderName)
        {
            var filePath = this.Folder + "\\" + subFolderName;
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        public void StandardInit()
        {
            this.DataStreams.Add(new NovatelDataStream(this, "Novatel GPS", "NOVATEL", "GPS\\Novatel"));
            this.DataStreams.Add(new SeptentrioDataStream(this, "Septentrio GPS", "SEPT", "GPS\\Septentrio"));
            this.DataStreams.Add(new MicroStrainDataStream(this, "Microstrain IMU GPS", "MIMU", "IMU\\MicroStrain"));

            this.DataStreams.Add(new VideoDataStream(this, "Canon 1", "CAN1", "Cameras\\CAN1"));
            this.DataStreams.Add(new VideoDataStream(this, "Canon 2", "CAN2", "Cameras\\CAN2"));
            this.DataStreams.Add(new VideoDataStream(this, "Casio 1", "CAS1", "Cameras\\CAS1"));
            this.DataStreams.Add(new VideoDataStream(this, "Casio 2", "CAS2", "Cameras\\CAS2"));
            this.DataStreams.Add(new VideoDataStream(this, "Samsung S5", "S5", "Cameras\\S5"));
            this.DataStreams.Add(new VideoDataStream(this, "Samsung S7", "S7", "Cameras\\S7"));

            this.DataStreams.Add(new ImageDataStream(this, "Nikon D600", "NIKON", "Cameras\\NIKON"));
            this.DataStreams.Add(new ImageDataStream(this, "Sony A6000 1", "SON1", "Cameras\\SON1"));
            this.DataStreams.Add(new ImageDataStream(this, "Sony A6000 2", "SON2", "Cameras\\SON2"));

            this.DataStreams.Add(new PointGreyDataStream(this, "Point Grey", "PTGREY0", "Cameras\\PTGREY", "A0"));
            this.DataStreams.Add(new PointGreyDataStream(this, "Point Grey", "PTGREY1", "Cameras\\PTGREY", "A1"));
            this.DataStreams.Add(new PointGreyDataStream(this, "Point Grey", "PTGREY2", "Cameras\\PTGREY", "A2"));
            this.DataStreams.Add(new PointGreyDataStream(this, "Point Grey", "PTGREY3", "Cameras\\PTGREY", "A3"));

            this.DataStreams.Add(new VideoDataStream(this, "GoPro 1", "GPR1", "Cameras\\GPR1"));
            this.DataStreams.Add(new VideoDataStream(this, "GoPro 2", "GPR2", "Cameras\\GPR2"));
            this.DataStreams.Add(new VideoDataStream(this, "GoPro 3", "GPR3", "Cameras\\GPR3"));

            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne HDL", "VHDL", "Velodynes\\VHDL", SharpVelodyne.VelodyneSensorType.HDL32E));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne Red", "VRED", "Velodynes\\VRED", SharpVelodyne.VelodyneSensorType.VLP16));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne Green", "VGREEN", "Velodynes\\VGREEN", SharpVelodyne.VelodyneSensorType.VLP16));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne Yellow", "VYELLOW", "Velodynes\\VYELLOW", SharpVelodyne.VelodyneSensorType.VLP16));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne Blue", "VBLUE", "Velodynes\\VBLUE", SharpVelodyne.VelodyneSensorType.VLP16));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne White", "VWHITE", "Velodynes\\VWHITE", SharpVelodyne.VelodyneSensorType.VLP16));
            this.DataStreams.Add(new VelodyneDataStream(this, "Velodyne Black", "VBLACK", "Velodynes\\VBLACK", SharpVelodyne.VelodyneSensorType.VLP16));

        }


        public int GetNextId()
        {
            return idSequence++;
        }

    }
}
