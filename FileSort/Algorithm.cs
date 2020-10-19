using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileSort
{
    class Algorithm
    {
        public readonly ISortAlgoritm SortAlgoritm;

        public readonly string FileName;

        private string tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
        public Algorithm(AlgorithmType algorithmType, string fileName)
        {
            FileName = fileName;

            switch (algorithmType)
            {
                case AlgorithmType.Insertion:
                    {
                        SortAlgoritm = new InsertionSort(tempDirectory);
                        break;
                    }
                case AlgorithmType.Merge:
                    {
                        SortAlgoritm = new MergeSort(tempDirectory);
                        break;
                    }
                default:
                    throw new ArgumentException($"unknown algorithm {algorithmType}");
            }


        }
    }
}
