using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class SimpsonDistribution : NotNormalizedDistribution
    {
        public SimpsonDistribution(double begin, double end)
            : base(begin, end, double.NaN, double.NaN)
        {
        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {

            #region estimation methods
            double EstimateStart(double min, double max, double x)
            {
                return 4.0 * (x - min) / Math.Pow(max - min, 2.0);
            }

            double EstimateEnd(double min, double max, double x)
            {
                return 4.0 * (max - x) / Math.Pow(max - min, 2.0);
            }
            #endregion

            var bounds = new (double min, double max)[2]
            {
                (MinValue, double.NaN), // start
                (double.NaN, MaxValue) // end
            };

            int totalIntervals = values.Count();
            double interval = (MaxValue - MinValue) / totalIntervals;
            double[] intervalStarts = Enumerable.Range(0, totalIntervals).Select(i => i * interval).ToArray();

            bounds[0].max = bounds[1].min = intervalStarts[totalIntervals / 2];

            var startExpectedProbabilites
                = SequenceHelper.GetExpectedProbabilitiesOnIntervals(
                    values.Take(totalIntervals / 2), bounds[0].min, bounds[0].max,
                    i => EstimateStart(MinValue, MaxValue, i));

            var endExpectedProbabilites
                = SequenceHelper.GetExpectedProbabilitiesOnIntervals(
                    values.Skip(totalIntervals - (totalIntervals / 2) - (totalIntervals % 2))
                        .Take((totalIntervals / 2) + (totalIntervals % 2)),
                    bounds[1].min, bounds[1].max,
                    i => EstimateEnd(MinValue, MaxValue, i));
            var totalSum = startExpectedProbabilites.Sum() + endExpectedProbabilites.Sum();
            var expected = startExpectedProbabilites.Concat(endExpectedProbabilites).Select(i => i / totalSum);
            return SequenceHelper.CheckEpsilon(values, expected, epsilon);

        }
    }
}
