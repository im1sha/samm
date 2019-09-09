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

        IEnumerable<uint> _data;
        IEnumerable<uint> _chart;

        public IEnumerable<uint> GetDistibution()
        {
            _data = _algorithm.Perform();
            _chart = _distribution.GetChartData(_data);
            return _chart;
        }

        public (double expectedValue, double variance, double standardDeviation) GetNormalizedStatistics()
        {
            return _distribution.GetNormalizedStatistics(_data);
        }
    }
}
