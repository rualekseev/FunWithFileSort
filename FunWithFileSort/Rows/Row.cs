using System;
using System.Text;

namespace FunWithFileSort
{
    public class Row : IRow, IComparable<Row>
    {
        #region static

        private static readonly Row _row = new Row(LeftPartRow.Empty, RightPartRow.Empty);
        public static Row Empty
        {
            get
            {
                return _row;
            }
        }

        public static bool TryParse(string line, out Row row)
        {
            if (line == null)
            {
                row = null;
                return false;
            }

            var split = line.Split(_delimiter);
            if (split.Length != 2)
            {
                row = null;
                return false;
            }

            row = new Row(new LeftPartRow(split[0]), new RightPartRow(split[1]));
            return true;
        }

        public static Row Parse(string line)
        {
            if (TryParse(line, out Row row) == false)
                throw new FormatException($"Can't parse \"{ line}\" to {typeof(Row)}");

            return row;
        }

        #endregion

        private const char _delimiter = '.';
        private readonly string _template = $"{{0}}{_delimiter} {{1}}{Environment.NewLine}";
        private string _rowAsString { get; set; }
        private int? _rowByteCount { get; set; }
        
        public readonly LeftPartRow LeftPart;
        public readonly RightPartRow RightPart;

        public Row(LeftPartRow leftPart, RightPartRow rightPart)
        {
            LeftPart = leftPart ?? throw new ArgumentNullException(nameof(leftPart));
            RightPart = rightPart ?? throw new ArgumentNullException(nameof(rightPart));
        }

        public string GetRowAsString()
        {
            if (_rowAsString != null)
                return _rowAsString;

            // don't use lock
            // multiple create string is possible, but it's safely
            _rowAsString = string.Format(_template, LeftPart.Value, RightPart.Value);
            return _rowAsString;
        }

        public int GetSizeInBytes()
        {
            // Encoding.GetByteCount is slowly
            if (_rowByteCount.HasValue == false)
                _rowByteCount = Encoding.Default.GetByteCount(GetRowAsString());

            return _rowByteCount.Value;
        }

        public int CompareTo(Row other)
        {
            var rightPartCompareResult = this.RightPart.CompareTo(other.RightPart);

            if (rightPartCompareResult == 0)
                return this.LeftPart.CompareTo(other.LeftPart);

            return rightPartCompareResult;
        }
    }
}
