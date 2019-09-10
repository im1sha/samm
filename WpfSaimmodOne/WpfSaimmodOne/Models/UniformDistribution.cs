using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodOne.Interfaces;

namespace WpfSaimmodOne.Models
{
    // Если равномерное распределение в интервале(a, b), то
    // M == (a+b) /2                            // 0.5     
    // D == sqr(b-a) /12                        // 1 /12
    // σ == (b-a) /sqrt(12)                     // 1 /sqrt(12)
    internal class UniformDistribution : IDistribution
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

        #endregion

        #region actual values
        // M 
        public double GetExpectedValue(IEnumerable<double> values)
        {
            return values.Average(i => i) / values.Count(); 
        }

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

        //public (bool isCorrect, int period, int aperiodicitySegment) EstimatePeriod(
        //    IEnumerable<uint> values,
        //    int requiredPeriod)
        //{
        //    var totalValues = values.Count();

        //    uint backValue = values.Last();
        //    uint frontValue = values.First();
        //    var backNumberPositions = new List<int>();
        //    var frontNumberPositions = new List<int>();

        //    for (int i = 0; i < totalValues; i++)
        //    {
        //        if (backValue == values.ElementAt(i))
        //        {
        //            backNumberPositions.Add(i);
        //            if (backNumberPositions.Count() == 2)
        //            {
        //                break;
        //            }
        //        }                
        //    }
        //    if (backNumberPositions.Count() < 2)
        //    {
        //        return (true, -1, -1);
        //    }
        //    var period = backNumberPositions[1] - backNumberPositions[0];

        //    for (int i = 0; i < totalValues; i++)
        //    {
        //        if (frontValue == values.ElementAt(i))
        //        {
        //            frontNumberPositions.Add(i);                   
        //            if (frontNumberPositions.Count() == 2)
        //            {
        //                var foundPeriod = frontNumberPositions[1] - frontNumberPositions[0];
        //                if (foundPeriod == period)
        //                {
        //                    return (
        //                        foundPeriod >= requiredPeriod,
        //                        foundPeriod, // period
        //                        frontNumberPositions[1]); // aperiodicity
        //                }
        //                else
        //                {
        //                    frontNumberPositions.RemoveAt(0);
        //                }
        //            }
        //        }
        //    }

        //    return (true, -1, -1);
        //}
      
        #endregion

        // if min value == 0 
        // then Length == max value     
        public IEnumerable<int> GetDistribution(
            IEnumerable<double> values, 
            int totalIntervals)
        {
            if (values == null || totalIntervals <= 0)
            {
                throw new ArgumentException();
            }
    
            var results = new List<int>();
            results.AddRange(Enumerable.Repeat(0, totalIntervals));

            double min = values.Min();
            double max = values.Max();

            double intervalLength = (max - min) / totalIntervals;

            double localMin = min - intervalLength;
            double localMax = min;

            for (int i = 0; i < results.Count; i++)
            {
                if (i == results.Count - 1)
                {
                    localMax = max;
                }
                else
                {
                    localMax += intervalLength;
                }
                localMin += intervalLength;
                
                results[i] = values.Count(j => (j >= localMin) && (j < localMax));                                    
            }

            return results;
        }
    }
}

