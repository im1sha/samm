using System;
using System.Collections.Generic;

namespace WpfSaimmodOne
{
    internal class Algorithm
    {
        private readonly long _multiplier;
        private readonly long _inintialValue;
        private readonly long _divider;
        private readonly IEnumerable<long> _seq;

        /// <summary>
        /// R[n+1] = (R[n] *a) %m. m > a
        /// </summary>
        /// <param name="multiplier">a</param>
        /// <param name="initialValue">R[0]</param>
        /// <param name="divider">m</param>
        public Algorithm(long multiplier, long initialValue, long divider)
        {
            _multiplier = multiplier;
            _inintialValue = initialValue;
            _divider = divider;
            _seq = CreateSequenceOfInt64(multiplier, initialValue, divider);
        }

        public long Divider => _divider;

        // Gets sequnce of generated members 
        private IEnumerable<long> CreateSequenceOfInt64(long multiplier, long initialValue, long divider)
        {
            var currMember = initialValue;
            long multiplication;

            while (true)
            {           
                // a * R[n] 
                multiplication = multiplier * currMember;
                currMember = multiplication % divider;
                yield return currMember;
            }
        }

        public IEnumerable<long> GetSequenceOfInt64()
        {
            foreach (var item in _seq)
            {
                yield return item;
            }
        }

       
       
        // Если равномерное распределение в интервале(a, b), то
        // M == (a+b) /2                            // 0.5     
        // D == sqr(b-a) /12                        // 1 /12
        // σ == (b-a) /sqrt(12)                     // 1 /sqrt(12)

        // M == {Expected value}
        public double RightExpectedValue => 0.5;

        // D == {Variance}
        public double RightVariance => 1.0 / 12.0;

        // σ == {Standard deviation}
        public double RightStandardDeviation => 1.0 / Math.Sqrt(12.0);

    }
}
