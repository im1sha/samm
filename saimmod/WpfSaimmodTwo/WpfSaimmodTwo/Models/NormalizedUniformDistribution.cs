using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Utils;

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
        public double RightExpectedValue => 0.5;

        // D 
        public double RightVariance => 1.0 / 12.0;

        // σ 
        public double RightStandardDeviation => Math.Sqrt(RightVariance);

        // 2K/N = PI/4 ± ϵ, 
        // where K is all pairs located inside of one forth of circle 
        // and N is total amount of points
        public static double RightUniformityEstimation => Math.PI / 4;

        #endregion

        #region actual values

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values)
        {
            return StatisticsGenerator.GetStatistics(values);
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

        public double Begin => 0.0;

        public double End => 1.0;
    }
}

