using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Models.Core;

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
            var result = values.Select(i => (i <= 0) ? 0.0 : -Math.Log(i) * Distribution.RightExpectedValue);
            var min = result.Min();
            var max = result.Max();
            Distribution.OverrideMinMax(min, max);
            return result;

        }
    }
}
