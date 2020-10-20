using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FunWithFileSort
{
    public class RowGeneratorWithRepeat : IRowGenerator<Row>
    {
        private readonly int _percentRepeat;
        private RightPartRow _storedForRepeat;
        private readonly Random _random;

        /// <summary>
        /// return random row with constant size. Can throw ArgumentException
        /// </summary>
        /// <param name="size">must be event, must be more than row min size</param>
        public Row GenerateRow(long size)
        {
            if (size == GetRowMinSize())
                return Row.Empty;

            if (size < GetRowMinSize())
                throw new ArgumentException($"{nameof(size)} must be more or equals than min size of rows");


            // TODO: need to think about balanced algorithm

            // generate smalled row
            var row = new Row(LeftPartRow.Empty, RightPartRow.Empty);

            bool evenStep = false;
            while (row.GetSizeInBytes() < size)
            {
                if (evenStep)
                    row = new Row(row.LeftPart, RightPartRow.GetRandom(row.RightPart.Value.Length + 1));
                else
                    row = new Row(LeftPartRow.GetRandom(row.LeftPart.Value.Length + 1), row.RightPart);
                evenStep = !evenStep;
            }

            return row;

        }

        public RowGeneratorWithRepeat(int percentRepeat)
        {
            if (percentRepeat < 0 || percentRepeat > 100)
                throw new ArgumentException($"{nameof(percentRepeat)} must be in range from 0 to 100");
            _percentRepeat = percentRepeat;
            _random = new Random();
        }


        public IEnumerable<Row> GetEnumerator()
        {
            while (true)
            {
                bool needToStoreValue = false;
                bool needToRepeateValue = false;
                if (_percentRepeat != 0)
                {
                   needToStoreValue = _random.Next(0, 101) <= _percentRepeat;
                   needToRepeateValue = _random.Next(0, 101) <= _percentRepeat;
                }

                var rightValue = needToRepeateValue
                    ? _storedForRepeat ?? RightPartRow.GetRandom()
                    : RightPartRow.GetRandom();

                var row = new Row(LeftPartRow.GetRandom(), rightValue);

                if (needToStoreValue)
                    _storedForRepeat = row.RightPart;

                yield return row;
            }
        }

        public long GetRowMinSize()
        {
            return Row.Empty.GetSizeInBytes();
        }
    }
}
