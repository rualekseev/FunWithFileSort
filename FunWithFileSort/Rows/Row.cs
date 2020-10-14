using System;
using System.Text;

namespace FunWithFileSort
{
    public class Row : IRow, IComparable<Row>
    {
        #region static

        public static Row Empty
        {
            get

            {
                return new Row(LeftPartRow.Empty, RightPartRow.Empty);
            }
        }

        #endregion


        private readonly string _template = $"{{0}}. {{1}}{Environment.NewLine}";
        private string _rowAsString { get; set; }
        
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
            return Encoding.Default.GetByteCount(GetRowAsString());
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
