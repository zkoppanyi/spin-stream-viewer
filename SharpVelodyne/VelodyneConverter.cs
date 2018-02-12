using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public class ProgressReportEventArgs
    {
        public double Precentage { get; private set; }
        public long ReadBytes { get; private set; }
        public DateTime CurrentDataTime { get; private set; }

        public ProgressReportEventArgs(double precentage, long readBytes, DateTime currentDataTime)
        {
            this.Precentage = precentage;
            this.ReadBytes = readBytes;
            this.CurrentDataTime = currentDataTime;
        }
    }

    public class VelodyneConverter
    {       
        public delegate void ProgressReportEventHandler(object sender, ProgressReportEventArgs args);
        public event ProgressReportEventHandler ProgressReport;
        public static int NMEA_LENGTH { get; private set; }

        protected virtual void OnReportProgress(ProgressReportEventArgs e)
        {
            ProgressReport?.Invoke(this, e);
        }


        public String PcapFile { get; private set; }

        private VelodyneConverter()
        {

        }

        public static VelodyneConverter Create(String pcapFile)
        {
            VelodyneConverter converter = new VelodyneConverter();
            converter.PcapFile = pcapFile;
            return converter;
        }

        public static String GetDefaultIndexFile(String pcapFile)
        {
            String baseFileName = (new FileInfo(pcapFile)).Directory.FullName + "\\" + Path.GetFileNameWithoutExtension(pcapFile);
            return baseFileName + ".idx";
        }

        public String GetDefaultIndexFile()
        {
            return GetDefaultIndexFile(PcapFile);
        }

        public static String GetDefaultPointFile(String pcapFile)
        {
            String baseFileName = (new FileInfo(pcapFile)).Directory.FullName + "\\" + Path.GetFileNameWithoutExtension(pcapFile);
            return baseFileName + ".bin";
        }

        public String GetDefaultPointFile()
        {
            return GetDefaultPointFile(PcapFile);
        }


        public void Convert(CancellationToken cancellationToken = default(CancellationToken))
        {

            FileInfo fi = new FileInfo(PcapFile);
            long fileSize = fi.Length;
            BinaryWriter idxWriter = new BinaryWriter(File.Open(GetDefaultIndexFile(PcapFile), FileMode.Create));

            OfflinePacketDevice selectedDevice = new OfflinePacketDevice(PcapFile);
            PacketCommunicator communicator =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                            // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000);                                  // read timeout


            using (BinaryWriter pointWriter = new BinaryWriter(File.Open(GetDefaultPointFile(PcapFile), FileMode.Create)))
            {
                Packet packet;
                bool isEof = false;
                VelodyneNmeaPacket lastNmea = null;
                long sumBytes = 0;
                long indexId = 0;

                while (!isEof)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        idxWriter.Close();
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:

                        case PacketCommunicatorReceiveResult.Ok:

                            sumBytes += packet.Length;

                            if (packet.Length == 554)
                            {
                                lastNmea = PacketInterpreter.ReadRecordNMEA(packet.Buffer);
                                indexId++;
                            }
                            else
                            {
                                pointWriter.Write(packet.Timestamp.Ticks);
                                pointWriter.Write(packet.Buffer);

                                if (lastNmea != null)
                                {

                                    int l = packet.Length - 6; // this is the end of the pack!
                                    double internal_time = BitConverter.ToUInt32(new byte[] { packet[l], packet[l + 1], packet[l + 2], packet[l + 3] }, 0) / 1000000.00;
                                    DateTime utcPacketTimestamp = packet.Timestamp.ToUniversalTime();
                                    DateTime time = (new DateTime(utcPacketTimestamp.Year, utcPacketTimestamp.Month, utcPacketTimestamp.Day, utcPacketTimestamp.Hour, 0, 0)).AddSeconds(internal_time);

                                    idxWriter.Write(time.Ticks);
                                    idxWriter.Write(packet.Timestamp.Ticks);
                                    idxWriter.Write(pointWriter.BaseStream.Position);
                                    byte[] nmea_byte = Encoding.ASCII.GetBytes(lastNmea.NmeaString.PadRight(NMEA_LENGTH, ' '));
                                    idxWriter.Write(nmea_byte);

                                    if (indexId % 100 == 0)
                                    {
                                        ProgressReportEventArgs args = new ProgressReportEventArgs((((double)sumBytes / (double)fileSize) * 100.0), sumBytes, utcPacketTimestamp);
                                        OnReportProgress(args);
                                    }

                                    lastNmea = null;
                                }

                            }
                            break;

                        case PacketCommunicatorReceiveResult.Eof:
                            isEof = true;
                            break;

                        default:
                            throw new InvalidOperationException("The result " + result + " should never be reached here");
                    }
                }
            }

            idxWriter.Close();

        }        

    }
}
