using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FunWithFileSort
{
    public class RowGenerator : IRowGenerator<Row>
    {
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

        public IEnumerable<Row> GetEnumerator()
        {
            while (true)
                yield return new Row(LeftPartRow.GetRandom(), RightPartRow.GetRandom());

        }

        public long GetRowMinSize()
        {
            return Row.Empty.GetSizeInBytes();
        }
    }
}
