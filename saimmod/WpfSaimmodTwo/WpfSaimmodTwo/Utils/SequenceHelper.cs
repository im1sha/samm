using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodTwo.Utils
{
    internal static class SequenceHelper
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
                double localMin = i * intervalLength;
                double localMax = (i + 1) * intervalLength;

                results[i] = values.Count(j => (j >= localMin) && (j < localMax));
            }
            return results;
        }
    }
}
