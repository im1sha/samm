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

        private IEnumerable<uint> _data;

        public IEnumerable<uint> Initialize()
        {
            _data = _algorithm.Perform();
            return _data;
        }

        public (bool isCorrect, double value) EstimateData()
        {
            return _distribution.EstimateDistribution(_data);
        }

        public IEnumerable<uint> GetChart()
        {
            return _distribution.GetChartData(_data);
        }

        public (double expectedValue, double variance, double standardDeviation) GetNormalizedStatistics()
        {
            return _distribution.GetNormalizedStatistics(_data);
        }
    }
}
