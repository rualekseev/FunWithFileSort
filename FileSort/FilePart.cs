using System;
using System.Collections.Generic;
using System.Text;

namespace FileSort
{
    class FilePart
    {
        public readonly string FileName;
        public readonly int LineCount;

        public readonly bool IsSorted;

        public FilePart(string fileName, int lineCount)
        {
            FileName = fileName;
            LineCount = lineCount;
            IsSorted = LineCount == 1;
        }

        public FilePart(string fileName, int lineCount, bool IsSorter)
        {
            FileName = fileName;
            LineCount = lineCount;
            IsSorted = IsSorter;
        }

    }
}
