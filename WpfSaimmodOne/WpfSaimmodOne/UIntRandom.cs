using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne
{
    public static class UIntRandom
    {
        public static uint GetValue(uint min, uint max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            uint uintRand = BitConverter.ToUInt32(buf, 0);

            return (uintRand % (max - min)) + min;
        }
    }
}
