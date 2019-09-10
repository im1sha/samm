using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodOne
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

        // ϵ 
        public static double UniformityEstimationEpsilon => 0.001;


        #endregion

        #region actual values
        // M 
        public double GetNormalizedExpectedValue(IEnumerable<uint> values)
        {
            return values.Average(i => (double)i / Length);
        }

        // D 
        public double GetNormalizedVariance(IEnumerable<uint> values)
        {
            double expValue = GetNormalizedExpectedValue(values);
            return (1.0 / (values.Count() - 1.0))
                * values.Sum(x => Math.Pow(((double)x / Length) - expValue, 2.0));
        }

        // σ 
        public double GetNormalizedStandardDeviation(IEnumerable<uint> values)
        {
            return Math.Sqrt(GetNormalizedVariance(values));
        }

        public (double expectedValue, double variance, double standardDeviation) GetNormalizedStatistics(
            IEnumerable<uint> values)
        {
            return (GetNormalizedExpectedValue(values),
                GetNormalizedVariance(values),
                GetNormalizedStandardDeviation(values));
        }

        public (bool isCorrect, double value) EstimateDistribution(IEnumerable<uint> values)
        {
            var totalPairs = (values.Count() - (values.Count() % 2)) / 2;
            var count = 0;
            for (int i = 0; i < totalPairs; i++)
            {
                if (Math.Pow(values.ElementAt(2 * i) / (double)Length, 2.0)
                    + Math.Pow(values.ElementAt(2 * i + 1) / (double)Length, 2.0) < 1)
                {
                    count++;
                }
            }
            var estimation = (double)count / totalPairs; // 2 * locatedPoints/totalPoints

            var result = (RightUniformityEstimation - UniformityEstimationEpsilon < estimation)
                && (estimation < RightUniformityEstimation + UniformityEstimationEpsilon);

            return (result, estimation);
        }

        public (bool isCorrect, int period, int aperiodicitySegment) EstimatePeriod(
            IEnumerable<uint> values,
            int requiredPeriod)
        {
            var totalValues = values.Count();

            uint backValue = values.Last();
            uint frontValue = values.First();
            var backNumberPositions = new List<int>();
            var frontNumberPositions = new List<int>();

            for (int i = 0; i < totalValues; i++)
            {
                if (backValue == values.ElementAt(i))
                {
                    backNumberPositions.Add(i);
                    if (backNumberPositions.Count() == 2)
                    {
                        break;
                    }
                }                
            }
            if (backNumberPositions.Count() < 2)
            {
                return (true, -1, -1);
            }
            var period = backNumberPositions[1] - backNumberPositions[0];

            for (int i = 0; i < totalValues; i++)
            {
                if (frontValue == values.ElementAt(i))
                {
                    frontNumberPositions.Add(i);                   
                    if (frontNumberPositions.Count() == 2)
                    {
                        var foundPeriod = frontNumberPositions[1] - frontNumberPositions[0];
                        if (foundPeriod == period)
                        {
                            return (
                                foundPeriod >= requiredPeriod,
                                foundPeriod, // period
                                frontNumberPositions[1]); // aperiodicity
                        }
                        else
                        {
                            frontNumberPositions.RemoveAt(0);
                        }
                    }
                }
            }

            return (true, -1, -1);
        }
        #endregion

        private const int TOTAL_INTERVALS = 20;

        // min value == 0
        // Length == max value
        public uint Length { get; private set; }
        public int TotalIntervals { get; private set; }

        public UniformDistribution(
            uint length,
            int totalIntervals = TOTAL_INTERVALS)
        {
            if (totalIntervals <= 0
               || length == 0)
            {
                throw new ArgumentException();
            }

            Length = length;
            TotalIntervals = totalIntervals;
        }

        public IEnumerable<uint> GetChartData(IEnumerable<uint> values)
        {
            if (values == null)
            {
                throw new ArgumentException();
            }

            var results = new List<uint>();
            results.AddRange(Enumerable.Repeat<uint>(0, TotalIntervals));

            double intervalLength = (double)Length / TotalIntervals;
            foreach (var item in values)
            {
                results[(int)(item / intervalLength)]++;
            }

            return results;
        }
    }
}

