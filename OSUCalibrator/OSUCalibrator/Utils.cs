using Accord.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator
{
    public static class Utils
    {
        /// <summary>
        /// Convert TOW to DateTime
        /// </summary>
        /// <param name="weeknumber">Number of GPS week</param>
        /// <param name="seconds">TOW in seconds</param>
        /// <returns>DateTime object</returns>
        public static DateTime ConvertFromTOW(int weeknumber, double seconds)
        {
            DateTime datum = new DateTime(1980, 1, 6, 0, 0, 0);
            DateTime week = datum.AddDays(weeknumber * 7);
            DateTime time = week.AddSeconds(seconds);
            return time;
        }

        /// <summary>
        /// Convert DateTime to Unix time (UTC) in seconds
        /// </summary>
        /// <param name="time">DateTime object</param>
        /// <returns>Unix time in UTC and seconds</returns>
        public static double ConvertToUnixTimestamp(DateTime time)
        {
            return (time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Write Accord.NET matrix to text file
        /// </summary>
        /// <param name="ptsMat">Matrix to be saved</param>
        /// <param name="saveTo">File path</param>
        public static void WriteMatToText(double[,] ptsMat, String saveTo)
        {
            TextWriter tw = new StreamWriter(saveTo);
            for (int i = 0; i < ptsMat.Rows(); i++)
            {
                for (int j = 0; j < ptsMat.Columns(); j++)
                {
                    if (j != 0) tw.Write(" ");
                    tw.Write(ptsMat[i, j]);
                }
                tw.WriteLine();
            }
            tw.Close();
        }
    }
}
