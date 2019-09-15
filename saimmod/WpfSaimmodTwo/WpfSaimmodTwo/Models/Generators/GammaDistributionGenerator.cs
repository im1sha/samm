using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;
using System.Linq;

namespace WpfSaimmodTwo.Models.Generators
{
    public class GammaDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public GammaDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
        }

        public override IEnumerable<double> GenerateSequence(IEnumerable<double> values)
        {
            double Multiple(IEnumerable<double> sequence, int startIndex, int count)
            {
                if (count == 0)
                {
                    return 1.0;
                }
                return sequence.Skip(startIndex).Take(count).Aggregate((x, y) => x * y);
            }

            if (_distribution.AdditionalParameters == null || _distribution.AdditionalParameters.Length != 2)
            {
                throw new ApplicationException();
            }

            int length = values.Count();
            int eta = (int)_distribution.AdditionalParameters[0];
            double lambda = _distribution.AdditionalParameters[1];

            double[] results = new double[length];

            for (int i = 0; i < length - eta; i++)
            {
                results[i] = Multiple(values, i, eta);
            }
            for (int i = length - eta; i < length; i++)
            {
                results[i] = Multiple(values, i, (length - i)) * Multiple(values, 0, eta - (length - i));
            }
            return results.Select(i => -(1 / lambda) * Math.Log(i));
        }
    }
}
