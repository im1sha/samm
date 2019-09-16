using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class GammaDistribution : NotNormalizedDistribution
    {
        public GammaDistribution(int eta, double lambda)
            : base(double.NaN, double.NaN, eta / lambda, eta / lambda / lambda, new[] { eta, lambda }, true)
        {
            if (eta < 1)
            {
                throw new ArgumentException(nameof(eta));
            }
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            double Estimate(double lambdaParam, int etaParam, double x)
            {
                if (x <= 0)
                {
                    return 0.0;
                }
                return (Math.Pow(lambdaParam, etaParam) / MathUtils.Factorial(etaParam - 1))
                    * Math.Pow(x, etaParam - 1) * Math.Pow(Math.E, -lambdaParam * x);
            }

            int eta = (int)AdditionalParameters[0];
            double lambda = AdditionalParameters[1];

            var expectedProbabilites =
                SequenceHelper.GetExpectedProbabilitiesOnIntervals(values, MinValue, MaxValue, i => Estimate(lambda, eta, i));

            return SequenceHelper.CheckEpsilon(values, expectedProbabilites, epsilon);

        }
    }
}
