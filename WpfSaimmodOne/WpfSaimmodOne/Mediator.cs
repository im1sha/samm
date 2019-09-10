using System;
using System.Collections.Generic;

namespace WpfSaimmodOne
{
    internal class Mediator
    {
        private readonly IDistribution _distribution;

        private readonly IAlgorithm _algorithm;

        public Mediator(IDistribution distribution, IAlgorithm algorithm, int period = 50_000)
        {
            _distribution = distribution;
            _algorithm = algorithm;
            if (period <= 0)
            {
                throw new ArgumentException();
            }
            _period = period;
        }

        private IEnumerable<uint> _data;
        private readonly int _period;

        public IEnumerable<uint> Initialize()
        {
            _data = _algorithm.Perform();
            return _data;
        }

        public (bool isCorrect, double value) EstimateDistribution()
        {
            return _distribution.EstimateDistribution(_data);
        }

        public (bool isCorrect, int period, int aperiodicitySegment) EstimatePeriod()
        {
            
            return _distribution.EstimatePeriod(_data, _period);
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
