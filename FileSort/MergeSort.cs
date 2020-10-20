using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using FunWithFileSort;

namespace FileSort
{
    class MergeSort: ISortAlgoritm
    {
        private readonly string _tempDirectory;
        private readonly string _sortedFileName;

        // last object will be smaller
        private readonly ConcurrentStack<FilePart> _filesToSplit = new ConcurrentStack<FilePart>();
        private volatile bool _splitEnded = false;

        private readonly ConcurrentStack<FilePart> _filesToSort = new ConcurrentStack<FilePart>();
        private volatile bool _sortEnded = false;

        // TODO use bag with sort by filesize
        private readonly ConcurrentQueue<FilePart> _filesToMerge = new ConcurrentQueue<FilePart>();

        public MergeSort(string tempDirectory)
        {
            _tempDirectory = tempDirectory ?? throw new ArgumentNullException(nameof(tempDirectory));
            _sortedFileName = Path.Combine(tempDirectory, "sorted.txt");
        }

        public Task<string> Run(string filename)
        {
            // can't spit file if it has zero or one rows
            if (TryFileSpit(filename).Result == false)
            {
                MoveFileToSortedFileResult(filename);
                return Task.FromResult(_sortedFileName);
            }

            var taskToSplit = Task.Run(() => SplitTasksManager(2));
            var taskToSort = Task.Run(() => SortTasksManager(1));
            var taskToMerge = Task.Run(() => MergeTasksManager(2));

            Task.WaitAll(taskToSplit, taskToSort, taskToMerge);
            return Task.FromResult(_sortedFileName);
        }

        void SplitTasksManager(int maxTasksCount)
        {
            List<Task> tasks = new List<Task>(maxTasksCount);
            for (int i = 0; i < maxTasksCount; i++)
            {
                tasks.Add(Task.Run(SplitFiles));
            }

            do
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].IsCompleted == false)
                        continue;

                    tasks[i] = Task.Run(SplitFiles);
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }
            while ((_filesToSplit.IsEmpty == true && tasks.All(x => x.IsCompleted == true)) == false);

            _splitEnded = true;
        }
        void SplitFiles()
        {
            while (_filesToSplit.TryPop(out FilePart file))
            {
                if (TryFileSpit(file.FileName).Result == false)
                {
                    AddFilePart(file);
                }
                else
                {
                    File.Delete(file.FileName);
                }
            }
        }

        void SortTasksManager(int maxTasksCount)
        {
            var tasks = new List<Task>(maxTasksCount);
            for (int i = 0; i < maxTasksCount; i++)
                tasks.Add(null);

            do
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i] == null)
                    {
                        tasks[i] = Task.Run(SortFiles);
                        continue;
                    }

                    if (tasks[i].IsCompleted == false)
                        continue;

                    tasks[i] = Task.Run(SortFiles);
                }
            }
            while (
        (_splitEnded == true
        && _filesToSort.IsEmpty == true
        && tasks.Where(x => x != null).All(x => x.IsCompleted == true)) == false);

            _sortEnded = true;
        }
        void SortFiles()
        {
            while (_filesToSort.TryPop(out FilePart file))
            {
                using(var sr = new StreamReader(file.FileName))
                {
                    var lines = File.ReadAllLines(file.FileName).Select(x=>Row.Parse(x)).ToList();
                    lines.Sort();

                    var fileName = Path.Combine(_tempDirectory, Path.GetRandomFileName());
                    using (var sw = new StreamWriter(fileName))
                    {
                        foreach (var line in lines)
                            sw.Write(line.GetRowAsString());
                    }
                    AddFilePart(new FilePart(fileName, lines.Count, new FileInfo(fileName).Length, true));
                }
                File.Delete(file.FileName);
            }
        }

        void MergeTasksManager(int maxTasksCount)
        {
            var files = new List<FilePart>();
            List<Task> tasks = new List<Task>(maxTasksCount);
            for (int i = 0; i < maxTasksCount; i++)
            {
                tasks.Add(null);
            }

            do
            {
                while (_filesToMerge.TryDequeue(out FilePart filePart))
                    files.Add(filePart);

                files = files.OrderBy(x => x.LineCount).ToList();

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i] == null)
                    {
                        if (files.Count > 1)
                        {
                            var file1 = files[0];
                            var file2 = files[1];
                            files.Remove(file1);
                            files.Remove(file2);
                            tasks[i] = Task.Run(() => Merge(file1, file2));
                        }
                        continue;
                    }

                    if (tasks[i].IsCompleted == false)
                        continue;

                    if (files.Count > 1)
                    {
                        var file1 = files[0];
                        var file2 = files[1];
                        files.Remove(file1);
                        files.Remove(file2);
                        tasks[i] = Task.Run(() => Merge(file1, file2));
                    }
                }
            } while (
            (_splitEnded == true
            && _sortEnded == true
            && files.Count == 1
            && _filesToMerge.Count == 0
            && tasks.Where(x => x != null).All(x => x.IsCompleted == true)) == false);


            MoveFileToSortedFileResult(files.Single().FileName);
        }

        async Task<bool> TryFileSpit(string fileToSplitPath)
        {
            var file1Name = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            var file2Name = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            int file1LineCount = 0;
            int file2LineCount = 0;

            using (var sr = new StreamReader(fileToSplitPath))
            using (var sw1 = new StreamWriter(file1Name))
            using (var sw2 = new StreamWriter(file2Name))
            {
                bool even = false;
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (even)
                    {
                        await sw2.WriteLineAsync(line);
                        file2LineCount++;
                    }
                    else
                    {
                        await sw1.WriteLineAsync(line);
                        file1LineCount++;
                    }
                    even = !even;
                }
            }

            if (file1LineCount > 0 && file2LineCount > 0)
            {
                AddFilePart(new FilePart(file1Name, file1LineCount, new FileInfo(file1Name).Length));
                AddFilePart(new FilePart(file2Name, file2LineCount, new FileInfo(file2Name).Length));
                return true;
            }

            return false;
        }

        async Task Merge(FilePart filePart1, FilePart filePart2)
        {
            string path = Path.Combine(_tempDirectory, Path.GetRandomFileName());
            int lineCount = 0;
            using (var sr1 = new StreamReader(filePart1.FileName))
            using (var sr2 = new StreamReader(filePart2.FileName))
            using (var sw = new StreamWriter(path))
            {
                var linFromFile1 = await sr1.ReadLineAsync();
                var linFromFile2 = await sr2.ReadLineAsync();
                while ((linFromFile1 == null && linFromFile2 == null) == false)
                {
                    if (linFromFile1 == null && linFromFile2 != null)
                    {
                        await sw.WriteLineAsync(linFromFile2);
                        lineCount++;
                        linFromFile2 = await sr2.ReadLineAsync();
                        continue;
                    }

                    if (linFromFile1 != null && linFromFile2 == null)
                    {
                        await sw.WriteLineAsync(linFromFile1);
                        lineCount++;
                        linFromFile1 = await sr1.ReadLineAsync();
                        continue;
                    }

                    var row1 = Row.Parse(linFromFile1);
                    var row2 = Row.Parse(linFromFile2);
                    var compareResult = row1.CompareTo(row2);
                    if (compareResult == -1)
                    {
                        await sw.WriteLineAsync(linFromFile1);
                        lineCount++;
                        linFromFile1 = await sr1.ReadLineAsync();
                        continue;
                    }

                    await sw.WriteLineAsync(linFromFile2);
                    lineCount++;
                    linFromFile2 = await sr2.ReadLineAsync();
                    continue;

                }
            }
            File.Delete(filePart1.FileName);
            File.Delete(filePart2.FileName);
            // filePart1 and filePart2 is empty
            if (lineCount == 0)
            {
                File.Delete(path);
                return;
            }
            AddFilePart(new FilePart(path, lineCount, new FileInfo(path).Length, true));
        }

        void AddFilePart(FilePart filePart)
        {
            if (filePart.IsSorted)
            {
                _filesToMerge.Enqueue(filePart);
                return;
            }

            if (filePart.FileSize <= 25*1024*1024)
            {
                _filesToSort.Push(filePart);
                return;
            }

            _filesToSplit.Push(filePart);
        }

        void MoveFileToSortedFileResult(string filename)
        {
            File.Move(filename, _sortedFileName);
        }
    }
}
