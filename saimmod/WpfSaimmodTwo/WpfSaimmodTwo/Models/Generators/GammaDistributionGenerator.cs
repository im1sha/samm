using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

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
            throw new Exception();

            if (_distribution.AdditionalParameters == null || _distribution.AdditionalParameters.Length != 2)
            {
                throw new ApplicationException();
            }

            double eta = _distribution.AdditionalParameters[0];
            double lambda = _distribution.AdditionalParameters[1];

            return null;
        }
    }
}
