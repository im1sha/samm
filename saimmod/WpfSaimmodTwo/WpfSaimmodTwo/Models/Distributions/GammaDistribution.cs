using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    internal class GammaDistribution : NotNormalizedDistribution
    {
        public GammaDistribution(double begin, double end)
            : base(begin, end, (begin + end) / 2.0, Math.Pow(begin - end, 2.0) / 12.0)
        {
        }
    }
}
