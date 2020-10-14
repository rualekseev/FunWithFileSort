using System;
using System.Collections;
using System.Collections.Generic;

namespace FunWithFileSort
{
    /// <summary>
    /// Generate text with random rows
    /// </summary>
    public class RandomTextGenerator: ITextGenerator
    {
        private long _fileSizeMax;
        private long _fileSizeCurrent;
        private IRowGenerator<Row> _rowGenerator;
        private IEnumerator<Row> _rowCurrentEnumerator;

        public RandomTextGenerator(long fileSizeMax, IRowGenerator<Row> rowGenerator)
        {
            _rowGenerator = rowGenerator ?? throw new ArgumentNullException(nameof(rowGenerator));

            var minRowSize = rowGenerator.GetRowMinSize();
            if (minRowSize > fileSizeMax)
            {
                throw new ArgumentException($"file size must be more or equal {minRowSize}, but current value {fileSizeMax}");
            }
            
            var rowGeneratorEnumerable = rowGenerator.GetEnumerator() ?? throw new ArgumentException($"IEnumerable for {nameof(rowGenerator.GetEnumerator)} is null");
            _rowCurrentEnumerator = rowGeneratorEnumerable.GetEnumerator() ?? throw new ArgumentException($"IEnumerator for {nameof(rowGenerator.GetEnumerator)} is null");
            _fileSizeMax = fileSizeMax;
            _current = null;
        }


        private Row _current;
        public string Current
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException();

                return _current.GetRowAsString();
            }
        }
            

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            ;
        }

        public bool MoveNext()
        {
            if (_fileSizeCurrent >= _fileSizeMax)
                return false;

            if (_rowCurrentEnumerator.MoveNext() == false)
                return false;

            var line = _rowCurrentEnumerator.Current;

            // create last line manually, because text size is constant
            if (_fileSizeCurrent + line.GetSizeInBytes() > _fileSizeMax - _rowGenerator.GetRowMinSize())
            {
                line = _rowGenerator.GenerateRow(_fileSizeMax - _fileSizeCurrent);
            }

            _current = line;
            
            _fileSizeCurrent += _current.GetSizeInBytes();
            return true;
        }

        public void Reset()
        {
            _fileSizeCurrent = 0;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
