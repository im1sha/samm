using System;
using System.Collections.Generic;
using System.Linq;

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
                return (1 / Math.Sqrt(Math.PI * 2 * RightVariance))
                    * Math.Pow(Math.E, (RightExpectedValue - x) / (2 * RightVariance));
            }

            double length = MaxValue - MinValue;
            double interval = length / values.Count();

            var arguments = Enumerable.Range(0, values.Count()).Select(i => (MinValue + interval / 2.0) + i * interval);
            var expectedProbabilites = arguments.Select(i => Estimate(i));

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
