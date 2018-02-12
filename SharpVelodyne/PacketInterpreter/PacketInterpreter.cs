using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public enum VelodyneSensorType
    {
        HDL32E,
        VLP16
    }

    public abstract class PacketInterpreter
    {
        protected static byte factoryByte1 = 0;
        protected static byte factoryByte2 = 0;

        public abstract VelodynePacket ReadRecord(byte[] packet, DateTime? baseTime  = null);
        public abstract VelodyneSensorType SensorType { get; }

        public static VelodyneNmeaPacket ReadRecordNMEA(byte[] packet)
        {
            int i = 42 + 198;
            double ts = BitConverter.ToUInt32(new byte[] { packet[i], packet[i + 1], packet[i + 2], packet[i + 3] }, 0) / 1000000.00;

            i = i + 8;
            byte[] nmeaMessageArray = new byte[75];
            for (int j = 0; j < 75; j++) nmeaMessageArray[j] = packet[i + j];

            String nmeaStr = System.Text.Encoding.ASCII.GetString(nmeaMessageArray);
            VelodyneNmeaPacket nmeaPacket = VelodyneNmeaPacket.Parse(nmeaStr);
            return nmeaPacket;
        }

        public static String ConvertFactroyByteToString(byte code)
        {
            switch (code)
            {
                case 0x37:
                    return "Strongest return";
                case 0x38:
                    return "Last return";
                case 0x39:
                    return "Dual return";
                case 0x21:
                    return "HDL-32E";
                case 0x22:
                    return "VLP-16";
                default:
                    return "N/A";
            }
        }

    }
}
