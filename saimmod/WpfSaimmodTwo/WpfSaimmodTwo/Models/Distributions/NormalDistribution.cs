using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class NormalDistribution : NotNormalizedDistribution
    {
        public NormalDistribution(double expectedValue, double variance)
            : base(double.NaN, double.NaN, expectedValue, variance, null, true)
        {
        }
        // sum of values == 1.0
        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            double Estimate(double x)
            {
                return (1 / Math.Sqrt(Math.PI * 2.0 * RightVariance))
                    * Math.Pow(Math.E, -Math.Pow(RightExpectedValue - x, 2.0) / (2.0 * RightVariance));
            }
       
            var expectedProbabilites = 
                SequenceHelper.GetExpectedProbabilitiesOnIntervals(values, MinValue, MaxValue, i => Estimate(i));

            return SequenceHelper.CheckEpsilon(values, expectedProbabilites, epsilon);

        }
    }
}
