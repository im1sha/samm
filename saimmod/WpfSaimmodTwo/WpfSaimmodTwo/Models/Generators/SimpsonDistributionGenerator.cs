using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Models.Core;

namespace WpfSaimmodTwo.Models.Generators
{
    public class SimpsonDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public SimpsonDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            var length = values.Count();
            var results = new double[length];
            var min = _distribution.MinValue;
            var max = _distribution.MaxValue;

            for (int i = 0; i < length - 1; i++)
            {
                results[i] = min + (max - min) * ((values.ElementAt(i) + values.ElementAt(i + 1)) / 2.0);
            }
            results[length - 1] = min + (max - min) * ((values.ElementAt(0) + values.ElementAt(length - 1)) / 2.0);

            return results;
        }
    }
}
