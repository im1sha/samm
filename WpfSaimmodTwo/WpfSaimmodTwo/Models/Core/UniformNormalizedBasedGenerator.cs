using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Core
{
    public abstract class UniformNormalizedBasedGenerator : IUniformNormalizedBasedGenerator
    {
        public INotNormalizedDistribution Distribution { get; }

        public UniformNormalizedBasedGenerator(INotNormalizedDistribution distribution)
        {
            Distribution = distribution;
        }

        public abstract IEnumerable<double> GenerateSequence(IEnumerable<double> values);
    }
}
