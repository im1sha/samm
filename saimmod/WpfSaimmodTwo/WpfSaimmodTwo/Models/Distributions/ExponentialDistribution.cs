using System;
using System.Collections.Generic;
using System.Linq;

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

            double max = MaxValue;
            double min = MinValue;

            double length = max - min;
            double interval = length / values.Count();

            var arguments = Enumerable.Range(0, values.Count()).Select(i => (MinValue + interval / 2.0) + i * interval);
            var probabilites = arguments.Select(i => Estimate(lambda, i)).ToArray();
            var probabilitesSum = probabilites.Sum();
            var expectedProbabilites = probabilites.Select(i => i / probabilitesSum).ToArray();

            for (int i = 0; i < values.Count(); i++)
            {
                if (values.ElementAt(i) < expectedProbabilites.ElementAt(i) + epsilon
                    && values.ElementAt(i) > expectedProbabilites.ElementAt(i) - epsilon)
                {
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
