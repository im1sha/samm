using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    public class AppModel
    {
        private readonly INormalizedDistribution _distribution;
        private readonly IAperiodicGenerator _algorithm;

        public AppModel(INormalizedDistribution distribution, IAperiodicGenerator algorithm)
        {
            _distribution = distribution;
            _algorithm = algorithm;
        }

        public IEnumerable<uint> GenerateSequence(int totalValues)
            => _algorithm.GenerateSequence(totalValues);
        

        public IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals)        
            => SequenceHelper.GetDistribution(values, begin, end, totalIntervals);
        

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values)       
            => _distribution.GetStatistics(values);
        

        public double CalculateIndirectEstimation(IEnumerable<double> values)       
            => _distribution.CalculateIndirectEstimation(values);
        

        public bool CheckIndirectEstimation(double estimation, double epsilon)        
            =>  _distribution.CheckIndirectEstimation(estimation, epsilon);
        

        public (int length, int start) FindCycle(uint multiplier, uint initialValue, uint divider)
            => _algorithm.FindCycle(multiplier, initialValue, divider);
        

        public static (uint multiplier, uint initialValue, uint divider) GenerateRandomParameters() 
            => LehmerGenerator.GenerateRandomParameters();

        public IEnumerable<double> Normalize(IEnumerable<uint>seq, uint divider)
            => SequenceHelper.Normalize(seq, divider);
    }
}
