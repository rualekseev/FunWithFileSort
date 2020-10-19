using System;
using System.Collections.Generic;
using System.Text;

namespace FileSort
{
    class FileSplitResult
    {
        public bool Success { get; }
        public FilePart FilePart1 { get; }
        public FilePart FilePart2 { get; }

        public FileSplitResult(bool success, FilePart filePart1, FilePart filePart2)
        {
            Success = success;
            FilePart1 = filePart1;
            FilePart2 = filePart2;
        }
    }
}


