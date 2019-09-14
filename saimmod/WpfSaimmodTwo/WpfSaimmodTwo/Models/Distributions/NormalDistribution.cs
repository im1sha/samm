using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class NormalDistribution : NotNormalizedDistribution
    {
        public NormalDistribution(double begin, double end, double expectedValue, double variance)
            : base(begin, end, expectedValue, variance)
        {
        }
    }
}
