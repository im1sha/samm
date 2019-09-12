using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSaimmodOne.Interfaces;
using WpfSaimmodOne.Utils;

namespace WpfSaimmodOne.Models
{
    internal class Lehmer : IAlgorithm
    {
        public uint Multiplier { get; }
        public uint InitialValue { get; }
        public uint Divider { get; }

        public Lehmer(
            uint multiplier,
            uint initialValue,
            uint divider)
        {
            if (multiplier <= 0 || divider <= 0 || initialValue <= 0)
            {
                throw new ArgumentException();
            }

            Multiplier = multiplier;
            InitialValue = initialValue;
            Divider = divider;
        }

        public IEnumerable<uint> GenerateSequence(int totalValues)
        {
            return GenerateSequence(Multiplier, InitialValue, Divider, totalValues, _generator);
        }

        // Left bound of sequence == 0
        // Right bound of sequence == m
        // R[n+1] = (R[n]*a) %m
        private IEnumerable<uint> GenerateSequence(
            uint multiplier,
            uint initialValue,
            uint divider,
            int length,
            Func<uint, uint, uint, uint> generator)
        {
            if (length <= 0)
            {
                throw new ArgumentException();
            }

            var result = new List<uint>();
            var current = initialValue;

            for (int i = 0; i < length; i++)
            {
                current = generator(multiplier, current, divider);
                result.Add(current);
            }

            return result;
        }

        // multiplier(a)        ~ 2^(n-1)
        // initialValue(R[0])   < (2^n)-1, prime
        // divider(m)           < (2^n)-1
        // initialValue < divider
        // a < R[0] < m
        public static (uint multiplier, uint initialValue, uint divider) GenerateRandomParameters()
        {
            bool CheckBinaryRepresentation(uint val)
            {
                const int ENOUGH_ONES_IN_REPRESENTATION = 24;
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

        public (int clength, int cstart) FindCycle(uint multiplier, uint initialValue, uint divider)
        {
            var periodResults = CycleDetector<uint>.FindCycle(
                     multiplier, initialValue, divider,
                     _generator);

            return (periodResults.clength, periodResults.cstart);
        }

        // USAGE: var next = _generator(multiplier, current, divider);
        private readonly Func<uint, uint, uint, uint> _generator = (a, r0, mod) => (a * r0) % mod;
    }
}
