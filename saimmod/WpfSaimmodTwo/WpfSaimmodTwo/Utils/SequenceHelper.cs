using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodTwo.Utils
{
    public static class SequenceHelper
    {
        // seq.All( i => i > 0 && i < length) && (seq.Max() < length)
        public static IEnumerable<double> Normalize(IEnumerable<uint> seq, double length)
        {
            var result = new List<double>();

            foreach (var item in seq)
            {
                result.Add(item / length);
            }

            return result;
        }

        public static IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals)
        {
            if (values == null || totalIntervals <= 0
                || begin > end || values.Min() < begin || values.Max() > end)
            {
                throw new ArgumentException();
            }

            var results = new List<int>();
            results.AddRange(Enumerable.Repeat(0, totalIntervals));

            double intervalLength = (end - begin) / totalIntervals;

            for (int i = 0; i < results.Count; i++)
            {
                double localMin = begin + (i * intervalLength);
                double localMax = begin + ((i + 1) * intervalLength);

                results[i] = values.Count(j => (j >= localMin) && (j < localMax));
            }
            return results;
        }

        public static IEnumerable<double> GetExpectedProbabilitiesOnIntervals(IEnumerable<double> values, 
            double minValue, double maxValue, Func<double, double> calculationCallback)
        {
            double length = maxValue - minValue;
            double interval = length / values.Count();

            var arguments = Enumerable.Range(0, values.Count()).Select(i => (minValue + interval / 2.0) + i * interval);
            var probabilites = arguments.Select(calculationCallback);
            var probabilitesSum = probabilites.Sum();
            var expectedProbabilites = probabilites.Select(i => i / probabilitesSum);

            return expectedProbabilites;
        }

        public static bool CheckEpsilon(IEnumerable<double> values, IEnumerable<double> expected, double epsilon) {
            for (int i = 0; i < values.Count(); i++)
            {
                if (values.ElementAt(i) < expected.ElementAt(i) + epsilon
                    && values.ElementAt(i) > expected.ElementAt(i) - epsilon)
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
