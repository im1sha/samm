using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodTwo.Utils
{
    internal static class StatisticsGenerator
    {
        public static double GetExpectedValue(IEnumerable<double> values)
            => values.Average(i => i);

        public static double GetVariance(IEnumerable<double> values)
        {
            double expValue = GetExpectedValue(values);
            return (1.0 / (values.Count() - 1.0))
                * values.Sum(x => Math.Pow(x - expValue, 2.0));
        }

        // σ 
        public static double GetStandardDeviation(double variance)
            => Math.Sqrt(variance);

        public static (double expectedValue, double variance, double standardDeviation) GetStatistics(
           IEnumerable<double> values)
        {
            double expVal = GetExpectedValue(values);
            double variance = GetVariance(values);
            double stdDeviation = GetStandardDeviation(variance);
            return (expVal, variance, stdDeviation);
        }
    }
}
