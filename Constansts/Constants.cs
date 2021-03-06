﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GZipTest.Constansts
{
    /// <summary>
    /// Global  constants
    /// </summary>
    public static class Constants
    {
        public const string INPUT_ARGUMENT_MISSING = "Input arguments  must be  more than 3!";
        public const string INPUT_OPERATION_ARGUMENT_MISSING = "Operation argument is missing! [compress/decompress]";
        public const int BUFFER_SIZE = 20 * 1024;
    }
}
