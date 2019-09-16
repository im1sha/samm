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
          
            double length = MaxValue - MinValue;
            double interval = length / values.Count();

            var arguments = Enumerable.Range(0, values.Count()).Select(i => (MinValue + interval / 2.0) + i * interval);
            var probabilites = arguments.Select(i => Estimate(lambda, eta, i)).ToArray();
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
