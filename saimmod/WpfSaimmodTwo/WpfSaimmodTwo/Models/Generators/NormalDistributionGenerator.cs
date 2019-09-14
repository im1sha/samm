using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models.Generators
{
    public class NormalDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public NormalDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            int length = values.Count();
            double[] results = new double[length];

            double mult;
            double stdDeviation = Math.Sqrt(_distribution.RightVariance);

            int startTake, middleSkip, endTake;

            const int totalTake = 6; // enough precision. it should be in range 6..12

            for (int i = 0; i < length; i++)
            {
                if (i < length - totalTake)
                {
                    startTake = i;
                    middleSkip = 0;
                    endTake = 0;
                }
                else
                {
                    endTake = length - i;
                    middleSkip = i;
                    startTake = totalTake - endTake;
                }

                mult = Math.Sqrt(2.0) * (values.Take(startTake).Concat(values.Skip(middleSkip).Take(endTake)).Sum() - 3.0);
                results[i] = _distribution.RightExpectedValue + (stdDeviation * mult);
            }
            return results;
        }
    }
}
