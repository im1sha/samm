using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;
using WpfSaimmodTwo.Models.Core;

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
            double stdDeviation = Math.Sqrt(_distribution.RightVariance);        
            int length = values.Count();
            double[] results = new double[length];

            double mult;
            double sqrt2 = Math.Sqrt(2.0);

            const int totalTake = 6; // enough precision. it should be in range 6..12

            for (int i = 0; i < length - totalTake; i++)
            {             
                mult = sqrt2 * (values.Skip(i).Take(totalTake).Sum() - 3.0);
                results[i] = _distribution.RightExpectedValue + (stdDeviation * mult);
            }

            int startTake, middleSkip, endTake;
                                
            for (int i = length - totalTake; i < length; i++)
            {
                endTake = length - i;
                middleSkip = i;
                startTake = totalTake - endTake;
                mult = sqrt2 * (values.Take(startTake).Concat(values.Skip(middleSkip).Take(endTake)).Sum() - 3.0);
                results[i] = _distribution.RightExpectedValue + (stdDeviation * mult);
            }

            _distribution.OverrideMinMax(results.Min(), results.Max());

            return results;
        }
    }
}
