using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodOne
{   
    // Если равномерное распределение в интервале(a, b), то
    // M == (a+b) /2                            // 0.5     
    // D == sqr(b-a) /12                        // 1 /12
    // σ == (b-a) /sqrt(12)                     // 1 /sqrt(12)
    public class UniformDistribution : IDistribution
    {
        #region expected results
        // M == {Expected value}
        public double RightExpectedValue => 0.5;

        // D == {Variance}
        public double RightVariance => 1.0 / 12.0;

        // σ == {Standard deviation}
        public double RightStandardDeviation => 1.0 / Math.Sqrt(12.0);
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

        public IEnumerable<uint> Calculate(IEnumerable<uint> values)
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

