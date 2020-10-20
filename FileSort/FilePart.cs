using System;

namespace FileSort
{
    struct FilePart
    {
        public readonly string FileName;
        public readonly int LineCount;
        public readonly long FileSize;
        public bool IsSorted;

        public FilePart(string fileName, int lineCount, long fileSize)
        {
            FileName = fileName;
            LineCount = lineCount;
            FileSize = fileSize;
            IsSorted = LineCount <= 1;
        }

        public FilePart(string fileName, int lineCount, long fileSize,bool IsSorter)
        {
            FileName = fileName;
            LineCount = lineCount;
            FileSize = fileSize;
            IsSorted = IsSorter;
        }

        internal void SetSorted()
        {
            IsSorted = true;
        }
    }
}
