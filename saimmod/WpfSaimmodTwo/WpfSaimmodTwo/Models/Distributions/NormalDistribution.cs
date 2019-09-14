using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class NormalDistribution : NotNormalizedDistribution
    {
        public NormalDistribution(double expectedValue, double variance)
            : base(double.NaN, double.NaN, expectedValue, variance)
        {
        }
    }
}
