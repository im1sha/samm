using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class SimpsonDistribution : NotNormalizedDistribution
    {
        public SimpsonDistribution(double begin, double end)
            : base(begin, end, (begin + end) / 2.0, Math.Pow(begin - end, 2.0) / 12.0)
        {

        }
    }
}
