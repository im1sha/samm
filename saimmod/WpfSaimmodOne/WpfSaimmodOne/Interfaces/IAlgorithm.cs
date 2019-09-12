using System.Collections.Generic;

namespace WpfSaimmodOne.Interfaces
{
    internal interface IAlgorithm
    {
        IEnumerable<uint> GenerateSequence(int totalValues);

        (int clength, int cstart) FindCycle(uint multiplier, uint initialValue, uint divider);
    }
}
