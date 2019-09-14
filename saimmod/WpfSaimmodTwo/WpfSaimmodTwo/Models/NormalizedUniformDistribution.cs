using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces;

namespace WpfSaimmodTwo.Models
{
    // Если равномерное распределение в интервале(a, b), то
    // M == (a+b) /2                            // 0.5     
    // D == sqr(b-a) /12                        // 1 /12
    // σ == (b-a) /sqrt(12)                     // 1 /sqrt(12)
    internal class NormalizedUniformDistribution : INormalizedDistribution
    {
        #region expected results
        // M
        public static double RightExpectedValue => 0.5;

        // D 
        public static double RightVariance => 1.0 / 12.0;

        // σ 
        public static double RightStandardDeviation => Math.Sqrt(RightVariance);

        // 2K/N = PI/4 ± ϵ, 
        // where K is all pairs located inside of one forth of circle 
        // and N is total amount of points
        public static double RightUniformityEstimation => Math.PI / 4;

        public double Begin => 0.0;

        public double End => 1.0;

        #endregion

        #region actual values
        // M 
        public double GetExpectedValue(IEnumerable<double> values)
            => values.Average(i => i); 
        

        // D 
        public double GetVariance(IEnumerable<double> values)
        {
            double expValue = GetExpectedValue(values);
            return (1.0 / (values.Count() - 1.0))
                * values.Sum(x => Math.Pow(x - expValue, 2.0));
        }

        // σ 
        public double GetStandardDeviation(double normalizedVariance)        
            => Math.Sqrt(normalizedVariance);

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values)
        {
            double expVal = GetExpectedValue(values);
            double variance = GetVariance(values);
            double stdDeviation = GetStandardDeviation(variance);
            return (expVal, variance, stdDeviation);
        }

        /// <summary>
        /// Calculates indirect estimation. Should be aprox. Pi/4
        /// </summary>
        /// <param name="values">Values distributed in range [0, 1]</param>
        /// <returns>Indirect estimation</returns>
        public double CalculateIndirectEstimation(IEnumerable<double> values)
        {
            var totalPairs = (values.Count() - (values.Count() % 2)) / 2;
            var count = 0;
            for (int i = 0; i < totalPairs; i++)
            {
                if (Math.Pow(values.ElementAt(2 * i), 2.0)
                    + Math.Pow(values.ElementAt(2 * i + 1), 2.0) < 1)
                {
                    count++;
                }
            }
            return (double)count / totalPairs; // 2 * locatedPoints/totalPoints         
        }

        public bool CheckIndirectEstimation(double estimation, double epsilon)
        {
            return (RightUniformityEstimation - epsilon < estimation)
                && (estimation < RightUniformityEstimation + epsilon);
        }

        #endregion

        // if min value == 0 
        // then Length == max value     
        public IEnumerable<int> GetNormalizedDistribution(
            IEnumerable<double> values,
            int totalIntervals)
            => GetDistribution(values, 0.0, 1.0, totalIntervals);
        

        public IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals)
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

