using System;
using System.Collections.Generic;
using System.Text;

namespace FunWithFileSort
{
    public interface ITextGenerator : IEnumerator<string>, IEnumerable<string>
    {
    }
}
