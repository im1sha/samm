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


        //public static IEnumerable<double> GetAverageOnIntervals(IEnumerable<double> sortedValues, IEnumerable<int> distribution)
        //{
        //    if (sortedValues.Count() != distribution.Sum())
        //    {
        //        throw new ArgumentException();
        //    }
        //    int length = distribution.Count();
        //    double[] results = new double[length];

        //    for (int i = 0; i < length; i++)
        //    {
        //        results[i] = sortedValues.Skip(distribution.Take(i).Sum()).Take(distribution.ElementAt(i)).Average();
        //    }

        //    return results;
        //}
    }
}
