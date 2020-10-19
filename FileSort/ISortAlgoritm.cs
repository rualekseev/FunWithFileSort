using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileSort
{
    interface ISortAlgoritm
    {
        Task Run(string filename);
    }
}
