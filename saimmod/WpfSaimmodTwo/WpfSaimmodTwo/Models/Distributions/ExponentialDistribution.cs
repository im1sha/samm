using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class ExponentialDistribution : NotNormalizedDistribution
    {
        public ExponentialDistribution(double lambda)
            : base(double.NaN, double.NaN, 1 / lambda, 1 / lambda / lambda, null, true)
        {
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            double Estimate(double lambdaParam, double x)
            {
                if (x <= 0)
                {
                    return 0.0;
                }
                return lambdaParam * Math.Pow(Math.E, -lambdaParam * x);
            }

            double lambda = 1 / RightExpectedValue;

            var expectedProbabilites =
                SequenceHelper.GetExpectedProbabilitiesOnIntervals(values, MinValue, MaxValue, i => Estimate(lambda, i));

            return SequenceHelper.CheckEpsilon(values, expectedProbabilites, epsilon);
        }
    }
}
