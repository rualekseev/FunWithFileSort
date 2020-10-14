using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace FunWithFileSort
{
    /// <summary>
    /// repeate one rows
    /// </summary>
    public class FastTextGenerator : ITextGenerator
    {
        private readonly string _template = $"The quick brown fox jumps over the lazy dog.{Environment.NewLine}";
        private readonly int _templateSize;


        private long _fileSizeMax;
        private long _fileSizeCurrent;
        public FastTextGenerator(long fileSizeMax)
        {
            _fileSizeMax = fileSizeMax;
            _templateSize = Encoding.Default.GetByteCount(_template);
            _current = null;
        }


        private string _current;
        public string Current
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException();

                return _current;
            }
        }

    object IEnumerator.Current => Current;

        public void Dispose()
        {
            ;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (_fileSizeCurrent >= _fileSizeMax)
                return false;

            if (_fileSizeCurrent + _templateSize > _fileSizeMax)
            {
                // TODO: don't work with symbols more than 1 byte
                var diffSize = _fileSizeMax - _fileSizeCurrent;
                _current = _template.Substring(0, (int)diffSize);
                _fileSizeCurrent += Encoding.Default.GetByteCount(_current);
            }
            else
            {
                _current = _template;
                _fileSizeCurrent += _templateSize;
            }

            return true;
        }

        public void Reset()
        {
            _fileSizeCurrent = 0;
            _current = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
