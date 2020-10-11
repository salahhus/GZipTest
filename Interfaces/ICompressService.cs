using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest.Interfaces
{
    public interface ICompressService
    {
        /// <summary>
        /// Splits  input file into  multiple files
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="path"></param>
        public void SplitFile(string inputFile, string path);
        /// <summary>
        /// Compress file operation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="chunkSize"></param>
        /// <param name="path"></param>
        public void Compress(Stream input, int index, int chunkSize, string path);
    }
}
