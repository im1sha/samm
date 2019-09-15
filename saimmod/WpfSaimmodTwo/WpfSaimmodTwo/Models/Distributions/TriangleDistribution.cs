using System;
using System.Collections.Generic;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class TriangleDistribution : NotNormalizedDistribution
    {
        public TriangleDistribution(double begin, double end)
            : base(begin, end, double.NaN, double.NaN)
        {
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            throw new NotImplementedException();
        }
    }
}
