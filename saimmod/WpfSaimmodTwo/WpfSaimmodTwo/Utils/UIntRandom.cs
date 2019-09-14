using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodTwo.Utils
{
    internal static class UIntRandom
    {
        public static uint GenerateValue(uint min, uint max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            uint uintRand = BitConverter.ToUInt32(buf, 0);

            return (uintRand % (max - min)) + min;
        }

        public static uint GenerateValue(
                uint intervalBegin,
                uint intervalEnd,
                Random random,
                IEnumerable<uint> deniedValues,
                bool checkPrime = false,
                bool checkBits = false)
        {
            bool CheckBinaryRepresentation(uint val)
            {
                const int ENOUGH_ONES_IN_REPRESENTATION = 24;
                return Convert.ToString(val, 2).Count(i => i == '1') >= ENOUGH_ONES_IN_REPRESENTATION;
            }

            while (true)
            {
                uint val = UIntRandom.GenerateValue(intervalBegin, intervalEnd, random);

                if ((deniedValues != null && deniedValues.Contains(val))
                    || (checkBits && !CheckBinaryRepresentation(val))
                    || (checkPrime && !PrimeTests.IsPrime(val)))
                {
                    continue;
                }

                return val;
            }
        }
    }
}
