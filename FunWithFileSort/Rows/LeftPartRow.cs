using System;
using System.Collections.Generic;
using System.Text;

namespace FunWithFileSort
{
    public class LeftPartRow:IComparable<LeftPartRow>
    {
        private static readonly LeftPartRow _leftPartRow = new LeftPartRow("0");
        private static readonly Random _random = new Random();

        public static LeftPartRow GetRandom()
        {
            return new LeftPartRow(_random.Next().ToString());
        }
        /// <summary>
        /// Generate object with rnak symbols 
        /// Sample: rank = 5 generate xxxxx where x is digit 
        /// </summary>
        /// <param name="rank">count symbols</param>
        public static LeftPartRow GetRandom(int rank)
        {
            if (rank < 1)
            {
                throw new ArgumentOutOfRangeException("rank must be more than 0");
            }

            StringBuilder sb = new StringBuilder(rank);
            // first digit nust be more than 0
            sb.Append(_random.Next(1,9));
            for (int i = 0; i < rank-1; i++)
            {
                sb.Append(_random.Next(9));
            }

            return new LeftPartRow(sb.ToString());
        }


        public static LeftPartRow Empty
        {
            get
            {
                return _leftPartRow;
            }
        }

        public string Value { get; }
        public LeftPartRow(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Rules: 
        /// 415 < 1
        /// 1 = 1
        /// 001 < 1
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(LeftPartRow other)
        {
            var lenghtDiff = this.Value.Length - other.Value.Length;

            if (lenghtDiff < 0)
                return 1;

            if (lenghtDiff > 0)
                return -1;

            // need to compare by value;
            return this.Value.CompareTo(other.Value) * -1;
        }
    }
}
