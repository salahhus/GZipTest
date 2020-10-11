using Microsoft.Extensions.DependencyInjection;
using System;
using GZipTest.Interfaces;

namespace GZipTest
{
    class Program
    {
        /// <summary>
        /// Author:     Husniddin Salahiddinov
        /// Project:    C# test task for VEEAM
        /// Project description: Compressing and decompressing of files using class System.IO.Compression.GzipStream 
        ///                      wich is given from user
        /// Date:       10.11.2020        
        /// </summary>

        static void Main(string[] args)
        {
            try
            {
                // register Dependency Injection
                var serviceProvider = new ServiceCollection()
                    .AddTransient<ICompressService, CompressingService>()
                    .AddTransient<IDecompressService, DecompressService>()
                    .BuildServiceProvider();


                // check for input arguments
                if (args.Length != 3)
                {
                    throw new Exception($"ERROR: {Constansts.Constants.INPUT_ARGUMENT_MISSING}");
                }

                // switch operations  mode
                if (args[0] == OperationEnum.compress.ToString())
                {
                    if (args[1] != string.Empty && args[2] != string.Empty)
                    {
                        var splitFileService = serviceProvider.GetService<ICompressService>();
                        splitFileService.SplitFile(args[1], args[2]);
                    }
                }
                else if (args[0] == OperationEnum.decompress.ToString())
                {
                    var decompressService = serviceProvider.GetService<IDecompressService>();
                    decompressService.CombineMultipleFilesIntoSingleFile(args[1], args[2]);
                }

                // success return code
                Console.WriteLine("0");
            }
            catch (Exception  ex)
            {
                // unsuccess return code
                Console.Error.WriteLine("1");
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
