using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class UniformDistribution : NotNormalizedDistribution
    {
        public UniformDistribution(double begin, double end)
            : base(begin, end, (begin + end) / 2.0, Math.Pow(begin - end, 2.0) / 12.0)
        {
        }

        
        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            var expectedEstimation = 1.0 / values.Count();
            return values.All(i => i < expectedEstimation + epsilon && i > expectedEstimation - epsilon);
        }
    }
}
