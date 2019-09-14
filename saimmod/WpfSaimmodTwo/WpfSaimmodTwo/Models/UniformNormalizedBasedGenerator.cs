using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;

namespace WpfSaimmodTwo.Models
{
    public abstract class UniformNormalizedBasedGenerator : IUniformNormalizedBasedGenerator
    {

        protected INotNormalizedDistribution _distribution;

        public UniformNormalizedBasedGenerator(INotNormalizedDistribution distribution)
        {
            _distribution = distribution;
        }

        public abstract IEnumerable<double> GenerateSequence(IEnumerable<double> values);
    }
}
