using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces
{
    internal interface IAlgorithm
    {
        IEnumerable<uint> GenerateSequence(int totalValues);

        (int length, int start) FindCycle(uint multiplier, uint initialValue, uint divider);
    }
}
