using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSort
{
    class MergeSort: ISortAlgoritm
    {
        private readonly string _tempDirectory;
        private string _sortedFileName;

        public MergeSort(string tempDirectory)
        {
            _tempDirectory = tempDirectory ?? throw new ArgumentNullException(nameof(tempDirectory));
            _sortedFileName = Path.Combine(tempDirectory, "sorted.txt");
        }

        public void Run(string filename)
        {
            var files = new List<FilePart>();
            var newfileName1 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            var newfileName2 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            var filesplitResult = FileSpit(filename, newfileName1, newfileName2);
            if (filesplitResult.Success == false)
            {
                File.Delete(newfileName1);
                File.Delete(newfileName2);
            }

            while (filesplitResult.Success)
            {
                var prevFile1Split = filesplitResult.FilePart1;
                var prevFile2Split = filesplitResult.FilePart2;

                newfileName1 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
                newfileName2 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
                filesplitResult = FileSpit(prevFile1Split.FileName, newfileName1, newfileName2);
                if (filesplitResult.Success == false)
                {
                    files.Add(prevFile1Split);
                    files.Add(prevFile2Split);

                    File.Delete(newfileName1);
                    File.Delete(newfileName2);
                }
                else
                {
                    File.Delete(prevFile1Split.FileName);
                    files.Add(prevFile2Split);
                }
            }

            while (files.Count(x => x.IsSorted == false) != 0)
            {
                var fileForMergeGroups = files.Where(x => x.IsSorted).GroupBy(x => x.LineCount).ToList();
                foreach (var fileForMerge in fileForMergeGroups)
                {
                    if (fileForMerge.Count() < 2)
                        continue;
                    var firstFile = fileForMerge.First();
                    var lastFile = fileForMerge.Last();

                    var mergeResultfile = Merge(firstFile, lastFile);
                    files.Remove(firstFile);
                    files.Remove(lastFile);
                    File.Delete(firstFile.FileName);
                    File.Delete(lastFile.FileName);
                    files.Add(mergeResultfile);
                }

                var minUnsortedFile = files.First(x => x.IsSorted == false && x.LineCount == (files.Where(x => x.IsSorted == false).Min(x => x.LineCount)));
                if (minUnsortedFile != null)
                {
                    newfileName1 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
                    newfileName2 = Path.Combine(_tempDirectory, Path.GetRandomFileName());
                    var fileSplitResult = FileSpit(minUnsortedFile.FileName, newfileName1, newfileName2);
                    if (fileSplitResult.Success)
                    {
                        files.Add(fileSplitResult.FilePart1);
                        files.Add(fileSplitResult.FilePart2);
                        files.Remove(minUnsortedFile);
                        File.Delete(minUnsortedFile.FileName);
                    }
                }
            }

            while(files.Count() > 0)
            {
                if (files.Count() == 1)
                {
                    File.Move(files.Single().FileName, _sortedFileName);
                    return;
                }
                var firstFile = files.First();
                var lastFile = files.Last();

                var mergeResultfile = Merge(firstFile, lastFile);
                files.Remove(firstFile);
                files.Remove(lastFile);
                File.Delete(firstFile.FileName);
                File.Delete(lastFile.FileName);
                files.Add(mergeResultfile);
            }
        }

        FileSplitResult FileSpit(string filePath, string newFile1, string newFile2)
        {


            using var sr = new StreamReader(filePath);
            using var sw1 = new StreamWriter(newFile1);
            using var sw2 = new StreamWriter(newFile2);
            int lineCountFile1 = 0;
            int lineCountFile2 = 0;
            bool even = false;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (even)
                {
                    sw2.WriteLine(line);
                    lineCountFile2++;
                }
                else
                {
                    sw1.WriteLine(line);
                    lineCountFile1++;
                }
                even = !even;
            }

            if (lineCountFile1 > 0 && lineCountFile2 > 0)
            {
                return new FileSplitResult(true, new FilePart(newFile1, lineCountFile1), new FilePart(newFile2, lineCountFile2));
            }
            return new FileSplitResult(false, null, null);
        }

        FilePart Merge(FilePart filePart1, FilePart filePart2)
        {
            string path = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            using var sr1 = new StreamReader(filePart1.FileName);
            using var sr2 = new StreamReader(filePart2.FileName);
            using var sw = new StreamWriter(path);
            int lineCount = 0;
            var linFromFile1 = sr1.ReadLine();
            var linFromFile2 = sr2.ReadLine();
            while ((linFromFile1 == null && linFromFile2 == null) == false)
            {
                switch (string.Compare(linFromFile1, linFromFile2))
                {
                    case -1:
                        {
                            sw.WriteLine(linFromFile2);
                            lineCount++;
                            linFromFile2 = null;
                            break;
                        }
                    case 0:
                        {
                            if (linFromFile1 == null && linFromFile2 == null)
                                break;

                            sw.WriteLine(linFromFile1);
                            lineCount++;
                            linFromFile1 = null;
                            break;
                        }
                    case 1:
                        {
                            sw.WriteLine(linFromFile1);
                            lineCount++;
                            linFromFile1 = null;
                            break;
                        }
                    default:
                        {
                            throw new Exception($"unknown compare result");
                        }
                }

                if (linFromFile1 == null)
                    linFromFile1 = sr1.ReadLine();
                if (linFromFile2 == null)
                    linFromFile2 = sr2.ReadLine();
            }

            if (filePart1.LineCount + filePart2.LineCount != lineCount)
            {
                int zzzz = 2;
                zzzz++;
            }
            return new FilePart(path, lineCount, true);
        }

    }
}
