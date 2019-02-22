using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public class PacketInterpreterHDL32E : PacketInterpreter
    {
        private static double[] firingAngleHDL32 = { -30.67, -9.33, -29.33, -8.00, -28.00, -6.66, -26.66, -5.33, -25.33, -4.00, -24.00, -2.67, -22.67, -1.33, -21.33, 0.00, -20.00, 1.33, -18.67, 2.67, -17.33, 4.00, -16.00, 5.33, -14.67, 6.67, -13.33, 8.00, -12.00, 9.33, -10.67, 10.67 };

        public override VelodyneSensorType SensorType
        {
            get
            {
                return VelodyneSensorType.HDL32E;
            }
        }

        // Ref: http://velodynelidar.com/docs/manuals/VLP-16%20User%20Manual%20and%20Programming%20Guide%2063-9243%20Rev%20A.pdf
        public override VelodynePacket ReadRecord(byte[] packet, DateTime? baseTime = null)
        {
            if (packet.Length == 554)
            {
                return ReadRecordNMEA(packet);
            }
            else if (packet.Length == 1248)
            {
                // Get timestamp and factory bytes
                int l = packet.Length - 6; // this is the end of the pack!
                double ts = BitConverter.ToUInt32(new byte[] { packet[l], packet[l + 1], packet[l + 2], packet[l + 3] }, 0) / 1000000.00;
                factoryByte1 = packet[l + 4];
                factoryByte2 = packet[l + 5];

                if (ConvertFactroyByteToString(factoryByte2) != "HDL-32E")
                {
                    throw new Exception("Packet seems to come from other then HDL sensor!");
                }

                //byte[] packet = reader.ReadBytes(1248);
                VelodynePointPacket pointPacket = new VelodynePointPacket();

                // Data packet
                int i = 42;
                for (int datai = 0; datai < 12; datai++)
                {
                    if ((packet[i] == 0xFF) && (packet[i + 1] == 0xEE))
                    {
                        //i += 0;
                        byte lower_azimuth = packet[i + 2];
                        byte upper_azimuth = packet[i + 3];
                        double azimuth = BitConverter.ToUInt16(new byte[] { lower_azimuth, upper_azimuth }, 0) / 100.00;

                        i = i + 4;
                        int k_iter = 2;
                        int j_iter = 16 * 3;
                        int i_jump = 16;
                        double[] firingAngle = firingAngleHDL32;

                        k_iter = 1;
                        j_iter = 32 * 3;
                        i_jump = 32;
                        firingAngle = firingAngleHDL32;

                        for (int k = 0; k < k_iter; k++)
                        {
                            for (int j = 0; j < j_iter; j = j + 3)
                            {
                                byte lower_distance = packet[i + j];
                                byte upper_distance = packet[i + j + 1];
                                double distance = (double)BitConverter.ToUInt16(new byte[] { lower_distance, upper_distance }, 0) * 2.0 / 1000.00;
                                byte intensity = packet[i + j + 2];

                                int diodId = j / 3;
                                double omega = firingAngle[diodId];
                                double omega_rad = omega / 180.0 * Math.PI;
                                double azimuth_rad = azimuth / 180.0 * Math.PI;

                                double x = Math.Cos(omega_rad) * Math.Sin(azimuth_rad) * distance;
                                double y = Math.Cos(omega_rad) * Math.Cos(azimuth_rad) * distance;
                                double z = Math.Sin(omega_rad) * distance;

                                VelodynePoint pt = new VelodynePoint(x, y, z, distance, azimuth_rad, omega_rad, intensity, diodId, ReturnType.StrongestReturn);
                                pt.InternalTime = ts;

                                if (baseTime != null)
                                {
                                    pt.Timestamp = baseTime.Value.AddSeconds(pt.InternalTime);
                                }

                                pointPacket.Points.Add(pt);

                                //if (ConvertFactroyByteToString(factoryByte2) == "HDL-32E")
                                //{
                                //    Console.WriteLine("Time (past the hour): " + ts + " s " + ts / 60 + "min \nFactory bytes: " + factoryByte1.ToString("X") + ": " + ConvertFactroyByteToString(factoryByte1) + ", "
                                //          + factoryByte2.ToString("X") + ": " + ConvertFactroyByteToString(factoryByte2));
                                    //Console.WriteLine("Azimuth: " + azimuth + " Omega: " + omega + " Distance: " + distance);
                                //}
                                //Console.WriteLine(pt.intensity);
                            }
                            i = i + i_jump * 3;
                        }
                    }
                    else
                    {
                        throw new Exception("Error in the point data packet structure!");
                    }
                }

                pointPacket.PacketTimestamp = pointPacket.Points[0].Timestamp;
                return pointPacket;
            }

            return null;
        }
    }
}
