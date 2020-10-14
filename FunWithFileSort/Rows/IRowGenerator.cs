using System.Collections.Generic;
namespace FunWithFileSort
{
    public interface IRowGenerator<T> where T : class
    {
        long GetRowMinSize();

        IEnumerable<T> GetEnumerator();
        T GenerateRow(long size);
    }
}