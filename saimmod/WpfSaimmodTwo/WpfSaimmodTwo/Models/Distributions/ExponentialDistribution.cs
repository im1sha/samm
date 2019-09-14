using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    internal class ExponentialDistribution : NotNormalizedDistribution
    {
        public ExponentialDistribution(double begin, double end, double lambda)
            : base(begin, end, 1 / lambda, 1 / Math.Pow(lambda, 2.0))
        {
        }
    }
}
