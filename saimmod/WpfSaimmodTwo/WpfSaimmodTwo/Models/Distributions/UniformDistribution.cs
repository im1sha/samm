using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    internal class UniformDistribution : NotNormalizedDistribution
    {
        public UniformDistribution(double begin, double end)
            : base(begin, end, (begin + end) / 2.0, Math.Pow(begin - end, 2.0) / 12.0)
        {
        }
    }
}
