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

        #endregion


        private readonly string _template = $"{{0}}. {{1}}{Environment.NewLine}";
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
