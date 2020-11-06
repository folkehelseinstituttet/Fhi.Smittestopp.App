using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NDB.Covid19.Interfaces;
using NDB.Covid19.ProtoModels;

namespace NDB.Covid19.ExposureNotifications.Helpers
{
    public abstract class BatchFileHelper
    {
        public static IEnumerable<string> SaveZipStreamToBinAndSig(Stream zipStream)
        {
            ZipArchive zipArchive = new ZipArchive(zipStream);

            ZipArchiveEntry exportBinEntry = zipArchive.GetEntry("export.bin");
            ZipArchiveEntry exportSigEntry = zipArchive.GetEntry("export.sig");

            Stream exportBinStream = exportBinEntry.Open();
            Stream exportSigStream = exportSigEntry.Open();

            // Get a temp file for each of the Streams
            string exportBinTmp = Path.Combine(CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileSystem>().CacheDirectory, Guid.NewGuid().ToString() + ".bin");
            string exportSigTmp = Path.Combine(CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileSystem>().CacheDirectory, Guid.NewGuid().ToString() + ".sig");

            // Write the Streams to the temp files
            FileStream exportBinFileStream = File.Create(exportBinTmp);
            FileStream exportSigFileStream = File.Create(exportSigTmp);
            exportBinStream.CopyTo(exportBinFileStream);
            exportSigStream.CopyTo(exportSigFileStream);

            exportBinFileStream.Close();
            exportSigFileStream.Close();

            return new List<string>()
            {
                exportBinTmp, exportSigTmp
            };
        }

        public static ZipArchive UrlToZipArchive(string localFileUrl)
        {
            var fileStream = new FileStream(localFileUrl, FileMode.Open, FileAccess.Read);
            return new ZipArchive(fileStream);
        }

        // Takes the URL of a zip with a .sig and .bin, where the .bin is a
        // 16 byte header followed by a batch of exposure keys following the
        // protobuf format of the TemporaryExposureKeyExport class
        public static TemporaryExposureKeyExport ZipToTemporaryExposureKeyExport(ZipArchive zipArchive)
        {
            ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry("export.bin");
            Stream stream = zipArchiveEntry.Open();
            byte[] bytes = ReadToEnd(stream);
            IEnumerable<byte> bytesSliced = bytes.Skip(16);
            return TemporaryExposureKeyExport.Parser.ParseFrom(bytesSliced.ToArray());
        }

        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}