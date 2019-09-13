using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    internal class AppModel
    {
        private readonly INormalizedDistribution _distribution;
        private readonly IAlgorithm _algorithm;

        public AppModel(INormalizedDistribution distribution, IAlgorithm algorithm)
        {
            _distribution = distribution;
            _algorithm = algorithm;
        }

        public IEnumerable<uint> InitializeSequence(int totalValues)
            => _algorithm.GenerateSequence(totalValues);
        

        public IEnumerable<int> GetDistributedValues(IEnumerable<double> values, int totalIntervals)        
            => _distribution.GetDistribution(values, totalIntervals);
        

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)       
            => _distribution.GetStatistics(values);
        

        public double CalculateIndirectEstimation(IEnumerable<double> values)       
            => _distribution.CalculateIndirectEstimation(values);
        

        public bool CheckIndirectEstimation(double estimation, double epsilon)        
            =>  _distribution.CheckIndirectEstimation(estimation, epsilon);
        

        public (int length, int start) FindCycle(uint multiplier, uint initialValue, uint divider)
            => _algorithm.FindCycle(multiplier, initialValue, divider);
        

        public static (uint multiplier, uint initialValue, uint divider) GenerateRandomParameters() 
            => Lehmer.GenerateRandomParameters();

        public IEnumerable<double> Normalize(IEnumerable<uint>seq, uint divider)
            => SequenceNormalizer.Normalize(seq, divider);
    }
}
