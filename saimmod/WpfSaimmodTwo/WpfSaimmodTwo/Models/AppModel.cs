using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    public class AppModel
    {
        private IEnumerable<double> _normalizedUniformDistributedSequence;
        private readonly NormalizedUniformDistribution _normalizedUniformDistribution;
        private readonly IAperiodicGenerator _aperiodicGenerator;

        public AppModel(NormalizedUniformDistribution distribution, IAperiodicGenerator generator)
        {
            _normalizedUniformDistribution = distribution;
            _aperiodicGenerator = generator;
        }

        public void InitializeModel(int totalValues)
        {


            _normalizedUniformDistributedSequence
                = _aperiodicGenerator.GenerateSequence(totalValues).Select(i => i / (double)_aperiodicGenerator.Divider).ToArray();
        }

        //public IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals)
        //{
        //    return SequenceHelper.GetDistribution(values, begin, end, totalIntervals);
        //}

        //public (double expectedValue, double variance, double standardDeviation) GetStatistics(
        //    IEnumerable<double> values)
        //{
        //    return StatisticsGenerator.GetStatistics(values);
        //}

        //public IEnumerable<double> Normalize(IEnumerable<uint> seq, uint divider)
        //{
        //    return SequenceHelper.Normalize(seq, divider);
        //}
    }
}
