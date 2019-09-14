using System.Collections.Generic;
using System.Linq;
using System;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Generators
{
    internal class UniformDistributionGenerator : IUniformNormalizedBasedGenerator
    {
        double MinValue { get; }

        double MaxValue { get; }

        public UniformDistributionGenerator(double min, double max)
        {
            if (min >= max)
            {
                throw new ArgumentException();
            }
            MinValue = min;
            MaxValue = max;
        }

        public IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            return values.Select(i => MinValue + ((MaxValue - MinValue) * i));
        }
    }
}
