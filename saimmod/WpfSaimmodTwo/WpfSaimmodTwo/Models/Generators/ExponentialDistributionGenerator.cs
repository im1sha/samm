using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Generators
{
    public class ExponentialDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public ExponentialDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            return values.Select(i => -Math.Log(i) * _distribution.RightExpectedValue);
        }
    }
}
