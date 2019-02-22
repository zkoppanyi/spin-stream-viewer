using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public enum ReturnMode
    {
        AllReturns,
        StrongestReturnOnly,
        LastReturnOnly
    }

    public class PacketInterpreterVLP16 : PacketInterpreter
    {
        private static double[] firingAngleVLP16 = { -15, 1, -13, -3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15 };
        public ReturnMode ReturnMode { get; set; }

        public override VelodyneSensorType SensorType
        {
            get
            {
                return VelodyneSensorType.VLP16;
            }
        }

        public PacketInterpreterVLP16() : this(ReturnMode.AllReturns)
        {

        }

        public PacketInterpreterVLP16(ReturnMode returnMode)
        {
            ReturnMode = returnMode;
        }


        // ref: http://velodynelidar.com/docs/manuals/VLP-16%20User%20Manual%20and%20Programming%20Guide%2063-9243%20Rev%20A.pdf
        public override VelodynePacket ReadRecord(byte[] packet, DateTime? baseTime = null)
        {
            if (packet.Length == 554)
            {
                return ReadRecordNMEA(packet);
            }
            else if (packet.Length == 1248)
            {
                DateTime? dateTopOfTheHour = null;
                if (baseTime != null)
                {
                    dateTopOfTheHour = new DateTime(baseTime.Value.Year, baseTime.Value.Month, baseTime.Value.Day, baseTime.Value.Hour, 0, 0);
                }
                //byte[] packet = reader.ReadBytes(1248);
                VelodynePointPacket pointPacket = new VelodynePointPacket();

                // Get timestamp and factory bytes
                int l = packet.Length - 6; // this is the end of the pack!
                double ts = BitConverter.ToUInt32(new byte[] { packet[l], packet[l + 1], packet[l + 2], packet[l + 3] }, 0) / 1000000.00;
                factoryByte1 = packet[l + 4];
                factoryByte2 = packet[l + 5];

                if (ConvertFactroyByteToString(factoryByte2) != "VLP-16")
                {
                    throw new Exception("Packet seems to come from other than VLP-16 sensor!");
                }

                if ((factoryByte1 == 0x38) && (this.ReturnMode == ReturnMode.StrongestReturnOnly))
                {
                    throw new ArgumentException("Asked for StrongestReturnOnly but the factory byte indicates that the sensor was set to last return mode!");
                }

                if ((factoryByte1 == 0x37) && (this.ReturnMode == ReturnMode.LastReturnOnly))
                {
                    throw new ArgumentException("Asked for StrongestReturnOnly but the factory byte indicates that the sensor was set to last return mode!");
                }


                // settigns for all returns
                int i = 42; // header bytes
                int k_iter = 2;
                int j_iter = 16 * 3;
                double[] firingAngle = firingAngleVLP16; // angles for vertical firing sequence


                // Data packet
                for (int datai = 0; datai < 12; datai++)
                {
                    if ((packet[i] == 0xFF) && (packet[i + 1] == 0xEE))
                    {
                        byte lower_azimuth = packet[i + 2];
                        byte upper_azimuth = packet[i + 3];
                        double azimuth = BitConverter.ToUInt16(new byte[] { lower_azimuth, upper_azimuth }, 0) / 100.00;
                        //Console.WriteLine(i + " Azimuth: " + azimuth);
                        i = i + 4;

                        ReturnType returnType = datai % 2 == 0 ? ReturnType.StrongestReturn : ReturnType.LastReturn;
                        if (factoryByte1 == 0x39) // sensor in dual return, but we need only strongest or last returns
                        {

                            // skip all odd data block for strongest return
                            if ((ReturnMode == ReturnMode.StrongestReturnOnly) && (returnType == ReturnType.LastReturn))
                            {
                                i += j_iter * k_iter;
                                continue;
                            }
                            else if ((ReturnMode == ReturnMode.LastReturnOnly) && (returnType == ReturnType.StrongestReturn)) // skip all even data block for strongest return
                            {
                                i += j_iter * k_iter;
                                continue;
                            }
                        }
                        else if (factoryByte1 == 0x37) // strongest return
                        {
                            returnType = ReturnType.StrongestReturn;
                        }
                        else if (factoryByte1 == 0x38) // last return
                        {
                            returnType = ReturnType.LastReturn;
                        }


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

                                if (k == 1)
                                {
                                    azimuth_rad = azimuth / 180.0 * Math.PI + 0.0035;
                                }

                                // VLP-16
                                double x = Math.Cos(azimuth_rad) * Math.Sin(omega_rad) * distance;
                                double y = Math.Cos(azimuth_rad) * Math.Cos(omega_rad) * distance;
                                double z = Math.Sin(azimuth_rad) * distance;

                                VelodynePoint pt = new VelodynePoint(x, y, z, distance, azimuth_rad, omega_rad, intensity, diodId, returnType);

                                pt.InternalTime = ts;
                                if (dateTopOfTheHour != null)
                                {
                                    pt.Timestamp = dateTopOfTheHour.Value.AddSeconds(pt.InternalTime);
                                }

                                pointPacket.Points.Add(pt);

                                //if (factoryByte1 != 0x39)
                                //if (distance != 0)
                                //if (azimuth == 318.78)
                                //if (k == 0)
                                {
                                    /*Console.WriteLine("Time (past the hour): " + ts + " s " + ts / 60 + "min \nFactory bytes: " + factoryByte1.ToString("X") + ": " + ConvertFactroyByteToString(factoryByte1) + ", "
                                          + factoryByte2.ToString("X") + ": " + ConvertFactroyByteToString(factoryByte2));
                                    Console.WriteLine("Azimuth: " + azimuth + " Omega: " + omega + " Distance: " + distance);*/
                                    //Console.WriteLine(datai + " " + k + " " + (j / 3) + " Azimuth: " + azimuth.ToString("0.00000") + " Omega: " + omega + " Distance: " + distance + " " + returnType + " " + ConvertFactroyByteToString(factoryByte1));
                                }
                            }
                            i += j_iter;
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
