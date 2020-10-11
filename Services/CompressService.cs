using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using GZipTest.Interfaces;

namespace GZipTest
{
    public class CompressingService : ICompressService
    {
        private readonly Mutex mutex = new Mutex();
        /// <summary>
        /// Split  file into multiple files
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="path"></param>
        public void SplitFile(string inputFile, string path)
        {
            string getFileName = Path.GetFileNameWithoutExtension(inputFile);
            string archiveFilePath = path + "\\" + getFileName;
            Directory.CreateDirectory(archiveFilePath);
            path = archiveFilePath;

            // get chunkSize from configuration file
            int chunkSize = int.Parse(Configuration.ConfigurationManager.AppSetting["FileSplitSizeOption"]);
            using (Stream input = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    Thread thread = new Thread(() => Compress(input, index, chunkSize, path));
                    thread.Start();
                    index++;
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// File compress operation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="chunkSize"></param>
        /// <param name="path"></param>
        public void Compress(Stream input, int index, int chunkSize, string path)
        {
            try
            {
                mutex.WaitOne();
                byte[] buffer = new byte[Constansts.Constants.BUFFER_SIZE];
                string preparedFilePath = path + "\\" + index;
                using (Stream output = File.Create(preparedFilePath))
                {
                    int remaining = chunkSize, bytesRead;
                    while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                            Math.Min(remaining, Constansts.Constants.BUFFER_SIZE))) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                        remaining -= bytesRead;
                    }
                }

                FileInfo fileToCompress = new FileInfo(preparedFilePath);
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                    }
                }

                File.Delete(preparedFilePath);
                Thread.Sleep(500);
            }
            finally
            {
                //Call the ReleaseMutex method to unblock so that other threads
                //that are trying to gain ownership of the mutex.  
                mutex.ReleaseMutex();
            }
        }

    }
}
