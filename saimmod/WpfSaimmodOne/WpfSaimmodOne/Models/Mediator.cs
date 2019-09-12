using System.Collections.Generic;
using WpfSaimmodOne.Interfaces;

namespace WpfSaimmodOne.Models
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

        public IEnumerable<uint> InitializeSequence(int totalValues)
        {
            return _algorithm.GenerateSequence(totalValues);
        }

        public IEnumerable<int> GetDistributedValues(IEnumerable<double> values, double min, double max, int totalIntervals)
        {
            return _distribution.GetDistribution(values, min, max, totalIntervals);
        }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values)
        {
            return _distribution.GetStatistics(values);
        }

        public double CalculateIndirectEstimation(IEnumerable<double> values)
        {
            return _distribution.CalculateIndirectEstimation( values);
        }

        public bool CheckIndirectEstimation(double estimation, double epsilon)
        {
            return  _distribution.CheckIndirectEstimation(estimation, epsilon);
        }

        public (int clength, int cstart) FindCycle(uint multiplier, uint initialValue, uint divider)
        {
            return _algorithm.FindCycle(multiplier, initialValue, divider);
        }
    }
}
