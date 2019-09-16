using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Models.Core;

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
            return values.Select(i => Distribution.MinValue + ((Distribution.MaxValue - Distribution.MinValue) * i));
        }
    }
}
