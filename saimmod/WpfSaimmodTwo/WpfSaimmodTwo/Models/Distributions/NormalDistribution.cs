using System;
using System.Collections.Generic;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class NormalDistribution : NotNormalizedDistribution
    {
        public NormalDistribution(double expectedValue, double variance)
            : base(double.NaN, double.NaN, expectedValue, variance)
        {
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            throw new NotImplementedException();
        }
    }
}
