using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne
{
    internal class Lehmer : IAlgorithm
    {      
        const int TOTAL_VALUES = 1000000;
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
            bool CheckBinaryRepresentation(uint val)
            {
                const int ENOUGH_ONES_IN_REPRESENTATION = 24; // 
                return Convert.ToString(val, 2).Count(i => i == '1') >= ENOUGH_ONES_IN_REPRESENTATION;
            }

            uint GenerateRandomValue(
                uint intervalBegin, 
                uint intervalEnd, 
                Random random,
                IEnumerable<uint> deniedValues,
                bool checkPrime = false,
                bool checkBits = false)
            {
                while (true)
                {
                    uint val = UIntRandom.GetValue(intervalBegin, intervalEnd, random);

                    if ((deniedValues != null && deniedValues.Contains(val))
                        || (checkBits && !CheckBinaryRepresentation(val))
                        || (checkPrime && !PrimeTests.IsPrime(val)))
                    {
                        continue;
                    }

                    return val;
                }
            }
                   
            var rand = new Random();
            IEnumerable<uint> deniedVals = new List<uint>();

            // set values bounds
            var data = new (uint intervalBegin, uint intervalEnd, bool prime, bool bitCheck, uint value)[3] {
                ((uint)(1.5 * (uint)Math.Pow(2, 31)), uint.MaxValue, true, false, 0), // divider              
                ((uint)(0.75 * (uint)Math.Pow(2, 31)), (uint)(1.5 * (uint)Math.Pow(2, 31)), true, false, 0), // mult
                ((uint)Math.Pow(2, 30), uint.MaxValue, true, true, 0), // initialValue
            };

            // values generation 
            for (int i = 0; i < data.Length; i++)
            {
                data[i].value = GenerateRandomValue(
                    data[i].intervalBegin, data[i].intervalEnd, 
                    rand, deniedVals, 
                    data[i].prime, data[i].bitCheck);

                deniedVals.Append(data[i].value);
            }

            return (data[1].value, data[2].value, data[0].value);
        }
    }
}
