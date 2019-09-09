using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne
{
    class Lehmer : IAlgorithm
    {      
        const int TOTAL_VALUES = 500000;
        public int TotalValues { get; private set; }
        public uint Multiplier { get; private set; }
        public uint InitialValue { get; private set; }
        public uint Divider { get; private set; }

        public Lehmer(uint multiplier,
            uint initialValue,
            uint divider,
            int totalValues = TOTAL_VALUES)
        {
            if (multiplier <= 0 || divider <= 0 || totalValues <= 0)
            {
                throw new ArgumentException ();
            }
    
            Multiplier = multiplier;
            InitialValue = initialValue;
            Divider = divider;
            TotalValues = totalValues;
        }

        public IEnumerable<uint> Perform()
        {
            return CreateSequence(Multiplier, InitialValue, Divider, TotalValues);
        }

        // Left bound of sequence == 0
        // Right bound of sequence == m
        // R[n+1] = (R[n]*a) %m
        private IEnumerable<uint> CreateSequence(
            uint multiplier,
            uint initialValue,
            uint divider,
            int length = TOTAL_VALUES)
        {
            var result = new List<uint>();

            var currMember = initialValue;
            uint multiplication;

            for (int i = 0; i < length; i++)
            {
                // a * R[n] 
                multiplication = multiplier * currMember;
                currMember = multiplication % divider;
                result.Add(currMember);
            }

            return result;
        }

        // multiplier(a)        ~ 2^(n-1)
        // initialValue(R[0])   < (2^n)-1, prime
        // divider(m)           < (2^n)-1
        // initialValue < divider
        // a < R[0] < m
        public static (uint multiplier, uint initialValue, uint divider) GenerateParameters()
        {
            uint multiplier;
            uint initialValue;
            uint divider;

            var rand = new Random();

            // set values bounds
            var data = new (uint intervalBegin, uint intervalEnd, uint? generatedValue)[3] {
                ((uint)(0.75 * (uint)Math.Pow(2, 31)), (uint)(1.5 * (uint)Math.Pow(2, 31)), null),
                ((uint)(1.5 * (uint)Math.Pow(2, 31)), uint.MaxValue, null),
                ((uint)(1.5 * (uint)Math.Pow(2, 31)), uint.MaxValue, null),
            };

            // values generation 
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

            #region sort values
            //a < R[0] < m

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

            #endregion

            return (multiplier, initialValue, divider);
        }

    }
}
