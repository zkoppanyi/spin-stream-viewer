using Accord.Video.FFMPEG;
using OSUCalibrator.DataStreams;
using OSUCalibrator.Loggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSUCalibrator
{ 
    public static class MetadataBuilder
    {

        public static void Create(Project project, ILogger logger, CancellationToken cancelletionToken = default(CancellationToken))
        {
            logger.WriteLineInfo("----------------------------------");
            logger.WriteLineInfo("  CAR METADATA file v0.1");
            logger.WriteLineInfo("  The Ohio State University, 2017");
            logger.WriteLineInfo("----------------------------------");
            logger.WriteLineInfo(" ");
            logger.WriteLineInfo(" ");

            int pi = 0;
            foreach (DataStream dataStream in project.DataStreams)
            {
                pi++;
                cancelletionToken.ThrowIfCancellationRequested();

                logger.WriteProgress((double)pi / (double)project.DataStreams.Count() * 100.0);
                dataStream.WriteMetadata(logger);
            }

            PrintSummary(project, logger);

            logger.WriteLineInfo(" ");
            logger.WriteLineInfo("Checking: ");

            // Checking Nikon's Novatel MARKTIMEA
            ImageDataStream nikonDataStream = (ImageDataStream)project.GetDataStreamByShortName("NIKON");
            NovatelDataStream novatelDataStream = (NovatelDataStream)project.GetDataStreamByShortName("NOVATEL");
            CheckEvents(nikonDataStream, novatelDataStream, EvenMarkerDataLine.MarkerEventPort.Unknown, logger);
            /*if ((nikonDataStream != null) && (novatelDataStream != null))
            {
                if (nikonDataStream.NumOfFiles != novatelDataStream.MarkerEvents.Count())
                {
                    logger.WriteLineInfo("WRONG -- Number of #MARKTIME events from the Novatel ASC and the number of Nikon images are different!");
                    logger.WriteLineWarning("Number of #MARKTIME events from the Novatel ASC and the number of Nikon images are different!");
                }
                else
                {
                    logger.WriteLineInfo("OK -- Number of #MARKTIME events from the Novatel ASC and the number of Nikon images !");
                }
            }*/

            // Checking Sony's Septentrio MARKTIME
            ImageDataStream son1DataStream = (ImageDataStream)project.GetDataStreamByShortName("SON1");
            ImageDataStream son2DataStream = (ImageDataStream)project.GetDataStreamByShortName("SON2");
            GPSDataStream septDataStream = (GPSDataStream)project.GetDataStreamByShortName("SEPT");
            CheckEvents(son1DataStream, septDataStream, EvenMarkerDataLine.MarkerEventPort.EventA, logger);
            CheckEvents(son2DataStream, septDataStream, EvenMarkerDataLine.MarkerEventPort.EventB, logger);

            logger.Flush();

        }

        public static void PrintSummary(Project project, ILogger logger)
        {

            // Summary
            logger.WriteLineInfo(" ");
            logger.WriteLineInfo("Summary: ");
            logger.WriteLineInfo(" ");
            foreach (DataStream dataStream in project.DataStreams)
            {
                String line = string.Format("{0,8}", dataStream.ShortName);
                line += string.Format("{0,13}", dataStream.NumOfData);
                if (!Double.IsNaN(dataStream.Length))
                {
                    line += string.Format("{0,15}", TimeSpan.FromSeconds(dataStream.Length).ToString(@"hh\:mm\:ss"));
                }
                line += string.Format("{0,25}", dataStream.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                line += string.Format("{0,25}", dataStream.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));

                if (dataStream.DataLines.Count() > 0)
                {
                    line += string.Format("{0,15}", dataStream.DataLines[0].TimeType.ToString());
                }


                logger.WriteLineInfo(line);

            }
            logger.WriteLineInfo(" ");

        }

        public static void CheckEvents(ImageDataStream imgDataStream, GPSDataStream gpsDataStream, EvenMarkerDataLine.MarkerEventPort port,  ILogger logger)
        {
            if ((imgDataStream != null) && (gpsDataStream != null))
            {
                if (imgDataStream.DataLines.Count() == 0) return;

                DateTime startTime = imgDataStream.StartTime - TimeSpan.FromMinutes(5);
                DateTime endTime = imgDataStream.EndTime + TimeSpan.FromMinutes(5);
                int numMarkers = 0;

                // check events and image numbers
                foreach (EvenMarkerDataLine evnt in gpsDataStream.MarkerEvents)
                {
                    if ((startTime <= evnt.TimeStamp) && (evnt.TimeStamp <= endTime) && (evnt.Port == port))
                    {
                        numMarkers++;
                    }
                }

                if (imgDataStream.NumOfData != numMarkers)
                {
                    logger.WriteLineWarning("Number of events from the " + gpsDataStream.ShortName + " and the number of " + imgDataStream.ShortName + " images are different!");
                    logger.WriteLineWarning("Number of images: " + imgDataStream.NumOfData);
                    logger.WriteLineWarning("Number of markers: " + numMarkers);
                }
                else
                {
                    logger.WriteLineInfo("OK -- Number of events from the " + gpsDataStream.ShortName + " and the number of " + imgDataStream.ShortName + " images are same");
                    logger.WriteLineInfo("Number of images: " + imgDataStream.NumOfData);
                    logger.WriteLineInfo("Number of markers: " + numMarkers);
                }
            }
        }

        
    }
}
