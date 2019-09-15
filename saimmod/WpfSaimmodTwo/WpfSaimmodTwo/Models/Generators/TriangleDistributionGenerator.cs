using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Generators
{
    public class TriangleDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public TriangleDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            var min = _distribution.MinValue;
            var max = _distribution.MaxValue;

            var length = values.Count();
            var results = new double[length];

            for (int i = 0; i < length - 1; i++)
            {
                results[i] = min + ((max - min) * Math.Max(values.ElementAt(i), values.ElementAt(i + 1)));
            }

            results[length - 1] = min + ((max - min) * Math.Max(values.ElementAt(0), values.ElementAt(length - 1)));

            return results;
        }
    }

}