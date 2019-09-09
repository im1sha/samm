using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne
{
    interface IAlgorithm
    {
        IEnumerable<uint> Perform();
    }
}
