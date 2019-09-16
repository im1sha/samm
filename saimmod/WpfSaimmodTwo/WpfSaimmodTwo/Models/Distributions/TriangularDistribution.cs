using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class TriangularDistribution : NotNormalizedDistribution
    {
        public TriangularDistribution(double begin, double end)
            : base(begin, end, double.NaN, double.NaN)
        {
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            double Estimate(double min, double max, double x)
            {
                return 2.0 * (x - min) / Math.Pow(max - min, 2.0);
            }

            var expectedProbabilites =
                SequenceHelper.GetExpectedProbabilitiesOnIntervals(values, MinValue, MaxValue, i => Estimate(MinValue, MaxValue, i));

            return SequenceHelper.CheckEpsilon(values, expectedProbabilites, epsilon);
        }
    }
}
