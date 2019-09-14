using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Generators
{
    public interface IAperiodicGenerator : IGenerator
    {
        uint Multiplier { get; }

        uint InitialValue { get; }

        uint Divider { get; }

        (int length, int start) FindCycle(uint multiplier, uint initialValue, uint divider);

        IEnumerable<uint> GenerateSequence(int totalValues);
    }
}
