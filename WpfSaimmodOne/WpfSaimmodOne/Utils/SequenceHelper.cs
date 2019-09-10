using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne.Utils
{
    static class SequenceHelper
    {
        // seq.All( i => i > 0 && i < length) && (seq.Max() < length)
        public static IEnumerable<double> Normalize(IEnumerable<uint> seq, double length)
        {
            var result = new List<double>();

            foreach (var item in seq)
            {
                result.Add(item/length);
            }

            return result;
        }

        public static (int period, int aperiodicitySegment)? EstimatePeriod(
            IEnumerable<uint> values)
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
                return ( 0, 0);
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
                             return (foundPeriod, frontNumberPositions[1]);
                        }
                        else
                        {
                            frontNumberPositions.RemoveAt(0);
                        }
                    }
                }
            }

            return (0,0);
        }
    }
}
