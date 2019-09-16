using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Models.Core;

namespace WpfSaimmodTwo.Models.Generators
{
    public class GammaDistributionGenerator : UniformNormalizedBasedGenerator
    {
        public GammaDistributionGenerator(INotNormalizedDistribution distribution)
            : base(distribution)
        {
            if (_distribution.AdditionalParameters == null || _distribution.AdditionalParameters.Length != 2)
            {
                throw new ApplicationException($"Expected {nameof(_distribution.AdditionalParameters)} format: " +
                    $"double[2] {{ eta, lambda }}");
            }
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
         
            if (values.Where(i => i < 0).Count() > 0)
            {
                throw new ArgumentException($"Gamma-distributed values are non-negative. " +
                    $"Parameter {nameof(values)} should contain only non-negative values.");
            }

            int length = values.Count();
            int eta = (int)_distribution.AdditionalParameters[0];
            double lambda = _distribution.AdditionalParameters[1];

            double[] multiplications = new double[length];

            for (int i = 0; i < length - eta; i++)
            {
                multiplications[i] = Multiple(values, i, eta);
            }
            for (int i = length - eta; i < length; i++)
            {
                multiplications[i] = Multiple(values, i, (length - i)) * Multiple(values, 0, eta - (length - i));
            }

            var results = multiplications.Select(i => -(1 / lambda) * Math.Log(i));

            _distribution.OverrideMinMax(results.Min(), results.Max());

            return results;
        }
    }
}
