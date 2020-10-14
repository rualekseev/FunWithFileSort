using FunWithFileSort;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;

namespace FileGenerator
{
    class Program
    {
        private const string Args1Key = "-size";
        private const string Args2Key = "-alg";

        static async Task Main(string[] args)
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

            try
            {
                foreach (var line in algorithm.TextGenerator)
                {
                    await Console.Out.WriteAsync(line);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Complited with Error.");
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
            }

            return;
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

            if (long.TryParse(args1[1], out long fileSize) == false)
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

            algorithm = new Algorithm(algorithmType, fileSize);
            return true;
        }

        private static bool TryParseAlgorithmType(string algType, out AlgorithmType algorithmType)
        {
            algorithmType = AlgorithmType.Unknown;

            if (string.Compare("fast", algType, true) == 0)
            {
                algorithmType = AlgorithmType.Fast;
            }

            if (string.Compare("rand", algType, true) == 0)
            {
                algorithmType = AlgorithmType.Rand;
            }

            if (string.Compare("rand+dubl", algType, true) == 0)
            {
                algorithmType = AlgorithmType.RandWithDuplicates;
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
            Console.WriteLine($"{Args1Key}=[size of file]");
            Console.WriteLine($"{Args2Key}=[algorithm]");
            Console.WriteLine($"Algorithms:" );
            Console.WriteLine($"fast      : generate file repeat one phrase");
            Console.WriteLine($"rand      : generate file use random phrase");
            Console.WriteLine($"rand+dubl : generate file use random phrase with duplicates");
        }
    }
}
