using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodOne
{
    internal class Algorithm
    {
        private readonly uint _multiplier;
        private readonly uint _inintialValue;
        private readonly uint _divider;
        private readonly IEnumerable<uint> _seq;

        /// <summary>
        /// R[n+1] = (R[n] *a) %m. m > a
        /// </summary>
        /// <param name="multiplier">a</param>
        /// <param name="initialValue">R[0]</param>
        /// <param name="divider">m</param>
        public Algorithm(uint multiplier, uint initialValue, uint divider)
        {
            _multiplier = multiplier;
            _inintialValue = initialValue;
            _divider = divider;
            _seq = CreateSequenceOfUInt(multiplier, initialValue, divider);
        }

        public uint Divider => _divider;

        // Gets sequnce of generated members 
        private IEnumerable<uint> CreateSequenceOfUInt(uint multiplier, uint initialValue, uint divider)
        {
            var currMember = initialValue;
            uint multiplication;

            while (true)
            {           
                // a * R[n] 
                multiplication = multiplier * currMember;
                currMember = multiplication % divider;
                yield return currMember;
            }
        }

        public IEnumerable<uint> GetSequenceOfInt64()
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

       

        // multiplier(a)        ~ 2^(n-1)
        // initialValue(R[0])   < (2^n)-1, prime
        // divider(m)           < (2^n)-1
        // initialValue < divider
        // a < R[0] < m
        public void GenerateParameters(out uint multiplier, out uint initialValue , out uint divider)
        {
            var rand = new Random();

            var data = new (uint intervalBegin, uint intervalEnd, uint? generatedValue)[3] {
                ((uint)(0.75 * (uint)Math.Pow(2, 31)), (uint)(1.5 * (uint)Math.Pow(2, 31)), null),
                ((uint)(1.5 * (uint)Math.Pow(2, 31)), uint.MaxValue, null),
                ((uint)(1.5 * (uint)Math.Pow(2, 31)), uint.MaxValue, null),
            };

            for (int i = 0; i < data.Length; i++)
            {       
                while (data[i].generatedValue == null)
                {
                    uint val = UIntRandom.GetValue(data[i].intervalBegin, data[i].intervalEnd, rand);

                    var allValues = data.Select(d => d.generatedValue).Where(e => e != null).ToList();
                    allValues.Add(val);
                    /// check if this value {val} is already represented
                    if (allValues.Distinct().Count() == allValues.Count())
                    {
                        if (PrimeTests.IsPrime(val))
                        {
                            data[i].generatedValue = val;
                        }
                    }                  
                }
            }

            multiplier = (uint)data[0].generatedValue;

            if (data[1].generatedValue < data[2].generatedValue)
            {
                initialValue = (uint)data[1].generatedValue;
                divider = (uint)data[2].generatedValue;
            }
            else
            {
                initialValue = (uint)data[2].generatedValue;
                divider = (uint)data[1].generatedValue;
            }


        }
    }
}
