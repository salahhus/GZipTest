using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest.Interfaces
{
    public interface IDecompressService
    {
        /// <summary>
        /// Combine multiple file operation
        /// </summary>
        /// <param name="inputDirectoryPath"></param>
        /// <param name="outputFilePath"></param>
        public void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string outputFilePath);
        /// <summary>
        /// Decompress file operation
        /// </summary>
        /// <param name="fileToDecompress"></param>
        public void Decompress(FileInfo fileToDecompress);
    }
}
