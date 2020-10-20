using FunWithFileSort;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerator
{
    class Algorithm
    {
        public readonly AlgorithmType AlgorithmType;

        public readonly ITextGenerator TextGenerator;

        // TODO: check filesize by zero or less than min rows
        public Algorithm(AlgorithmType algorithmType, long filesize)
        {
            AlgorithmType = algorithmType;

            switch(algorithmType)
            {
                case AlgorithmType.Fast:
                    {
                        TextGenerator = new FastTextGenerator(filesize);
                        break;
                    }
                case AlgorithmType.Rand:
                    {
                        TextGenerator = new RandomTextGenerator(filesize, new RowGenerator());
                        break;
                    }
                case AlgorithmType.RandWithDuplicates:
                    {
                        TextGenerator = new RandomTextGenerator(filesize, new RowGeneratorWithRepeat(10));
                        break;
                    }
                default:
                    throw new ArgumentException($"unknown algorithm {algorithmType}");
            }
        }
    }
}
