using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace FunWithFileSort
{
    public class RightPartRow:IComparable<RightPartRow>
    {
        private static readonly Random _random = new Random();


        /// <summary>
        /// Return random from 1 to 30 characters
        /// </summary>
        public static RightPartRow GetRandom()
        {
            return RightPartRow.GetRandom(_random.Next(1, 30));
        }

        public static RightPartRow GetRandom(int lenght)
        {
            const string allowChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";

            if (lenght == 0)
                return RightPartRow.Empty;

            var chars = new char[lenght];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = allowChars[_random.Next(allowChars.Length)];
            }

            return new RightPartRow(new string(chars));
        }

        public static RightPartRow Empty
        {
            get
            {
                return new RightPartRow(string.Empty);
            }
        }

        /// <summary>
        /// ignore case and reverse string compare
        /// Sample: 
        /// XXX=xXx;
        /// A > B;
        /// xxx > xxxx;
        /// str.Empty > a
        /// </summary>
        public int CompareTo(RightPartRow other)
        {
            return string.Compare(this.Value, other.Value, true) * -1;
        }

        public string Value { get; }

        public RightPartRow(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
