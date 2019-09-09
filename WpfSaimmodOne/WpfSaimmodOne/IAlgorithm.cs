using System.Collections.Generic;

namespace WpfSaimmodOne
{
    internal interface IAlgorithm
    {
        IEnumerable<uint> Perform();
    }
}
