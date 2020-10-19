using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileSort
{
    class InsertionSort: ISortAlgoritm
    {
        private string _tempDirectory;
        private string _sortedFileName;

        public InsertionSort(string tempDirectory)
        {
            _tempDirectory = tempDirectory ?? throw new ArgumentNullException(nameof(tempDirectory));
            _sortedFileName = Path.Combine(tempDirectory, "sorted.txt");
        }

        public void Run(string fileName)
        {
            var sr = new StreamReader(fileName);

            string line = sr.ReadLine();
            if (line == null)
                return;
            // write first line to file
            using (var sw = new StreamWriter(_sortedFileName))
            {
                sw.WriteLine(line);
            }

            while ((line = sr.ReadLine()) != null)
            {
                InsertToSortedFile(line);
            }
        }

        private void InsertToSortedFile(string insertLine)
        {
            bool lineInsered = false;
            var tempFileName = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            using (var sw = new StreamWriter(tempFileName))
            using (var sr = new StreamReader(_sortedFileName))
            {
                var line = string.Empty;
                while((line = sr.ReadLine()) != null)
                {
                    if (string.Compare(line, insertLine) == 1)
                    {
                        lineInsered = true;
                        sw.WriteLine(insertLine);
                        sw.WriteLine(line);
                        break;
                    }
                    else
                        sw.WriteLine(line);
                }

                // insertLine more than all lines in _sortedFileName 
                if (lineInsered == false)
                    sw.WriteLine(insertLine);

                while ((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
            }

            File.Delete(_sortedFileName);
            File.Move(tempFileName, _sortedFileName);
        }
    }
}
