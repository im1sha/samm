using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Generators
{
    internal class NormalDistributionGenerator : IUniformNormalizedBasedGenerator
    {
        public IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
    }
}
