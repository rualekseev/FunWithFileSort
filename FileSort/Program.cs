using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace FileSort
{
    class Program
    {
        private const string Args1Key = "-file";
        private const string Args2Key = "-alg";


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteErrorArgs();
                return;
            }

            if (args.Length == 1 &&
                (string.Compare(args[0], "-h", true) == 0 || string.Compare(args[0], "-help", true) == 0))
            {
                WriteHelp();
                return;
            }

            if (TryParseArgs(args, out Algorithm algorithm) == false)
            {
                WriteErrorArgs();
                return;
            }

            algorithm.SortAlgoritm.Run(algorithm.FileName).Wait();
        }

        private static bool TryParseArgs(string[] args, out Algorithm algorithm)
        {
            if (args.Length != 2)
            {
                algorithm = null;
                return false;
            }

            var args1 = args[0].Split('=');
            if (args1.Length != 2 || string.Compare(args1[0], Args1Key) != 0)
            {
                algorithm = null;
                return false;
            }

            var args2 = args[1].Split('=');
            if (args2.Length != 2 || string.Compare(args2[0], Args2Key) != 0)
            {
                algorithm = null;
                return false;
            }

            if (TryParseAlgorithmType(args2[1], out AlgorithmType algorithmType) == false)
            {
                algorithm = null;
                return false;
            }

            algorithm = new Algorithm(algorithmType, args1[1]);
            return true;
        }

        private static bool TryParseAlgorithmType(string algType, out AlgorithmType algorithmType)
        {
            algorithmType = AlgorithmType.Unknown;

            if (string.Compare("insertion", algType, true) == 0)
            {
                algorithmType = AlgorithmType.Insertion;
            }

            if (string.Compare("merge", algType, true) == 0)
            {
                algorithmType = AlgorithmType.Merge;
            }

            return algorithmType != AlgorithmType.Unknown;
        }

        private static void WriteErrorArgs()
        {
            Console.Error.WriteLine("Error args. See help (use -h or -help args)");
        }

        private static void WriteHelp()
        {
            Console.WriteLine("required arguments:");
            Console.WriteLine($"{Args1Key}=[path to result file]");
            Console.WriteLine($"{Args2Key}=[algorithm]");
            Console.WriteLine($"Algorithms:");
            Console.WriteLine($"insertion : sort file use a insertion sort");
            Console.WriteLine($"merge     : sort file use a merge sort");
        }
    }
}
