using System.Collections.Generic;
using System.Linq;
using System;
using WpfSaimmodTwo.Interfaces.Generators;
using WpfSaimmodTwo.Interfaces.Distributions;

namespace WpfSaimmodTwo.Models.Generators
{
    public class UniformDistributionGenerator : UniformNormalizedBasedGenerator
    {
    
        public UniformDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            return values.Select(i => _distribution.MinValue + ((_distribution.MaxValue - _distribution.MinValue) * i));
        }
    }
}
