using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVelodyne
{
    public class IndexData
    {
        public const int NMEA_LENGTH = 75;

        /// <summary>
        /// Timestamp of the packet
        /// </summary>
        public DateTime PacketTimeStamp { get; private set; }

        /// <summary>
        /// Internal timestamp
        /// </summary>
        public DateTime InternalTimeStamp { get; private set; }

        /// <summary>
        /// Correspnding position in the point file
        /// </summary>
        public long Position { get; private set; }

        /// <summary>
        /// Timestamp in the NMEA message
        /// </summary>
        public VelodyneNmeaPacket Nmea { get; private set; }

        public IndexData(DateTime packetTimeStamp, DateTime internalTimeStamp, long position, String nmea)
        {
            this.PacketTimeStamp = packetTimeStamp;
            this.Nmea = VelodyneNmeaPacket.Parse(nmea);
            this.InternalTimeStamp = internalTimeStamp;
            this.Position = position;
        }

    }
}
