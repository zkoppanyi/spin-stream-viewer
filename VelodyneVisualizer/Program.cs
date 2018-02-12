using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyneTest
{   

    class Program
    {      

        static void Main(string[] args)
        {
            //String pcapFile = @"F:\CAR\2017_08_28_2\Velodynes\VHDL\2017-08-28-16-01-45_Velodyne-HDL-32-Data.pcap";
            //String pcapFile = @"E:\CAR\2017_10_13\Velodynes\VWHITE\2017-10-13-12-11-18_Velodyne-VLP-16-Data.pcap";
            //String pcapFile = @"E:\CAR\2017_10_13\Velodynes\VRED\2017-10-13-12-03-19_Velodyne-VLP-16-Data.pcap";
            String pcapFile = @"E:\CAR\2017_10_13\Velodynes\VGREEN\2017-10-13-12-04-11_Velodyne-VLP-16-Data.pcap";
            String indexFile = VelodyneConverter.GetDefaultIndexFile(pcapFile);
            String pointFile = VelodyneConverter.GetDefaultPointFile(pcapFile);

            /*String indexFile = @"E:\CAR\2017_08_28_2\Velodynes\VRED\2017-08-28-15-59-48_Velodyne-VLP-16-Data.pcap.idx";
            String pointFile = @"E:\CAR\2017_08_28_2\Velodynes\VRED\2017-08-28-15-59-48_Velodyne-VLP-16-Data.pcap.bin";
            VelodyneReader veloReader = new VelodyneReader(VelodyneReader.SensorType.VLP16, indexFile, pointFile);*/

            /*VelodyneConverter converter = VelodyneConverter.Create(pcapFile);
            converter.ProgressReport += Converter_ProgressReport;
            converter.Convert();
            Console.WriteLine("Conversion is done!");
            return;*/

            Console.WriteLine("Done.");
            List<VelodynePoint> pts = new List<VelodynePoint>();

            using (VelodyneReader veloReader = VelodyneReader.Open(VelodyneSensorType.VLP16, ReturnMode.LastReturnOnly, indexFile, pointFile))
            {

                //veloReader.AnalysOffset();

                IndexData idx = veloReader.FindIndexByTime(new DateTime(2017, 10, 13, 16, 45, 04, 0), VelodyneReader.SearchType.FLOOR);
                veloReader.SeekByTime(new DateTime(2017, 10, 13, 16, 52, 04, 0));
                Console.WriteLine(idx.PacketTimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + " NMEA: " + idx.InternalTimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + " Pos: " + idx.Position + " NMEA: " + idx.Nmea.NmeaString);

                for (int k = 0; k < 1000; k++)
                {
                    VelodynePacket packet = veloReader.ReadNext();

                    if (packet is VelodynePointPacket)
                    {
                        VelodynePointPacket pointPacket = packet as VelodynePointPacket;
                    }
                }

                Console.WriteLine("done.");
                Console.ReadKey();
                Simple3dViewerWnd viewer = new Simple3dViewerWnd(veloReader);
                viewer.ShowDialog();
            }

           

            Console.ReadKey();
        }

        private static void Converter_ProgressReport(object sender, ProgressReportEventArgs args)
        {
            Console.WriteLine(args.CurrentDataTime.ToString("yyy-MM-dd hh:mm:ss") + " " + args.ReadBytes / 1000000 + " MB " + args.Precentage.ToString("0.00") + "%");
        }
    }
}
