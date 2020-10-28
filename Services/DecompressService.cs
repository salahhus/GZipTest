using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using GZipTest.Interfaces;
using System.IO;
using System.Threading;

namespace GZipTest
{
    public class DecompressService : IDecompressService
    {
        /// <summary>
        /// object using for locking multithreads
        /// </summary>
        private readonly Object obj = new object();

        /// <summary>
        /// Combine multiple files into  single  file than save it 
        /// by param outputFilePath
        /// </summary>
        /// <param name="inputDirectoryPath"></param>
        /// <param name="outputFilePath"></param>
        public void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string outputFilePath)
        {
            try
            {
                //  decompress compressed files
                string directoryPath = Path.GetFileNameWithoutExtension(inputDirectoryPath);
                string tmpPath = Path.GetDirectoryName(inputDirectoryPath);

                Console.WriteLine("In progress decompressing..");

                DirectoryInfo directorySelected = new DirectoryInfo(tmpPath + "\\" + directoryPath);
                foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
                {
                    Thread thread = new Thread(() => Decompress(fileToDecompress));
                    thread.Start();
                    Thread.Sleep(500);
                }

                // combine decompressed  file into one file
                string[] inputFilePaths = Directory.GetFiles(tmpPath + "\\" + directoryPath);
                Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
                using (var outputStream = File.Create(outputFilePath))
                {
                    foreach (var inputFilePath in inputFilePaths)
                    {
                        if (!inputFilePath.EndsWith(".gz"))
                        {
                            using (var inputStream = File.OpenRead(inputFilePath))
                            {
                                inputStream.CopyTo(outputStream);
                            }
                            File.Delete(inputFilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        /// <summary>
        /// Decompress file from  .gz  file
        /// </summary>
        /// <param name="fileToDecompress"></param>
        public void Decompress(FileInfo fileToDecompress)
        {
            lock (obj)
            {
                try
                {
                    using (FileStream originalFileStream = fileToDecompress.OpenRead())
                    {
                        string currentFileName = fileToDecompress.FullName;
                        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                        using (FileStream decompressedFileStream = File.Create(newFileName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            }
        }
    }
}
