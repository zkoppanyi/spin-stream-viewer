using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSUCalibrator
{

    public static class YUVConverter
    {
        /// <summary>
        /// Convert YUV file to BMP without saving
        /// </summary>
        /// <param name="yuvFile"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static Bitmap Convert(String yuvFile, ILogger logger)
        {
            return Convert(yuvFile, null, logger);
        }

        /// <summary>
        /// Convert YUV file to BMP with saving
        /// </summary>
        /// <param name="yuvFile">Source YUV file</param>
        /// <param name="destFile">Destination BMP file</param>
        /// <param name="logger">Logger</param>
        /// <returns>Converted BMP file</returns>
        public static Bitmap Convert(String yuvFile, String destFile, ILogger logger)
        {
            logger.WriteLineInfo("Source file: " + yuvFile);
            logger.WriteLineInfo("Destination file: " + destFile);

            byte[] buffer = File.ReadAllBytes(yuvFile);
            MemoryStream memStream = new MemoryStream(buffer);

            using (BinaryReader reader = new BinaryReader(memStream))
            {
                String line1 = readLine(reader); 
                String line2 = readLine(reader);
                String[] metaLineSplit  = line2.Split(' ');
                double ts = Double.Parse(metaLineSplit[4]);
                String line3 = readLine(reader);
                metaLineSplit = line3.Split(' ');
                int width = int.Parse(metaLineSplit[0]);
                int height = int.Parse(metaLineSplit[1]);
                String line4 = readLine(reader);

                // save metadata
                /*StreamWriter writer = new StreamWriter(destFile + ".txt");
                writer.Write(line1);
                writer.Write(line2);
                writer.Write(line3);
                writer.Write(line4);
                writer.Flush();
                writer.Close();

                logger.WriteLineInfo("Timestamp: " + ts);
                logger.WriteLineInfo("Width: " + width);
                logger.WriteLineInfo("Height: " + height);*/

                Bitmap dest = new Bitmap(width, height);
                int row = 0;
                int col = 0;
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int u0 = reader.ReadByte();
                    int y0 = reader.ReadByte();
                    int v0 = reader.ReadByte();
                    int y1 = reader.ReadByte();
                    u0 = u0 - 128;
                    v0 = v0 - 128;
                    double red1 = 1.0 * y0 + 1.13983 * v0;
                    double grn1 = 1.0 * y0 - 0.39465 * u0 - 0.5806 * v0;
                    double blu1 = 1.0 * y0 + 2.03211 * u0;
                    double red2 = 1.0 * y1 + 1.13983 * v0;
                    double grn2 = 1.0 * y1 - 0.39465 * u0 - 0.5806 * v0;
                    double blu2 = 1.0 * y1 + 2.03211 * u0;

                    if (red1 < 0) red1 = 0;
                    if (grn1 < 0) grn1 = 0;
                    if (blu1 < 0) blu1 = 0;
                    if (red2 < 0) red2 = 0;
                    if (grn2 < 0) grn2 = 0;
                    if (blu2 < 0) blu2 = 0;

                    int r1 = System.Convert.ToUInt16(red1);
                    int g1 = System.Convert.ToUInt16(grn1);
                    int b1 = System.Convert.ToUInt16(blu1);
                    int r2 = System.Convert.ToUInt16(red2);
                    int g2 = System.Convert.ToUInt16(grn2);
                    int b2 = System.Convert.ToUInt16(blu2);

                    if (r1 > 255) r1 = 255;
                    if (g1 > 255) g1 = 255;
                    if (b1 > 255) b1 = 255;

                    if (r2 > 255) r2 = 255;
                    if (g2 > 255) g2 = 255;
                    if (b2 > 255) b2 = 255;

                    dest.SetPixel(row, col, Color.FromArgb(r1, g1, b1));
                    dest.SetPixel(row+1, col, Color.FromArgb(r2, g2, b2));
                    row += 2;

                    if (row >= width)
                    {
                        row = 0;
                        col++;

                        logger.WriteProgress((double)col / (double)height * 100.0);
                    }

                }

                if (destFile != null)
                {
                    dest.Save(destFile + ".bmp");
                }

                return dest;
            }

        }

        private static String readLine(BinaryReader reader)
        {
            string str = "";
            char c = ' ';
            while (c != '\n')
            {
                c = reader.ReadChar();
                str += c;
            }

            return str;
        }
    }
}
