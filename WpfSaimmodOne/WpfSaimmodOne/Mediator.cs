using System.Collections.Generic;

namespace WpfSaimmodOne
{

    internal class Mediator
    {
        private readonly IDistribution _distribution;
        private readonly IAlgorithm _algorithm;
        public Mediator(IDistribution distribution, IAlgorithm algorithm)
        {
            _distribution = distribution;
            _algorithm = algorithm;
        }

        public IEnumerable<uint> GetDistibution() =>  _distribution.Calculate(_algorithm.Perform());        
    }
}
