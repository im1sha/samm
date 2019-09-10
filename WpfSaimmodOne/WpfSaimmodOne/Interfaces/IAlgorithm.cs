using System.Collections.Generic;

namespace WpfSaimmodOne.Interfaces
{
    internal interface IAlgorithm
    {
        IEnumerable<uint> GenerateSequence(int totalValues);
    }
}
