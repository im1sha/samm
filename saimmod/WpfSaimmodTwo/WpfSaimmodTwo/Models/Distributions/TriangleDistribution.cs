using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class TriangleDistribution : NotNormalizedDistribution
    {
        public TriangleDistribution(double begin, double end)
            : base(begin, end, double.NaN, double.NaN)
        {
        }
    }
}
