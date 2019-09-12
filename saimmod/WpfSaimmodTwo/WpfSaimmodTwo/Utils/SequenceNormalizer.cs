using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodTwo.Utils
{
    static class SequenceNormalizer
    {
        // seq.All( i => i > 0 && i < length) && (seq.Max() < length)
        public static IEnumerable<double> Normalize(IEnumerable<uint> seq, double length)
        {
            var result = new List<double>();

            foreach (var item in seq)
            {
                result.Add(item / length);
            }

            return result;
        }
    }
}
