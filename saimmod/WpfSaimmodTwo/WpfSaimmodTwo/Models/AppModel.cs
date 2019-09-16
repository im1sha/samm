using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Interfaces.Generators;
using WpfSaimmodTwo.Models.Core;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    public class AppModel
    {
        private IEnumerable<double> _normalizedUniformDistributedSequence;
        private readonly IAperiodicGenerator _aperiodicGenerator;

        public AppModel(IAperiodicGenerator generator)
        {
            _aperiodicGenerator = generator;
        }

        public void InitializeModel(int totalValues)
        {
            _normalizedUniformDistributedSequence
                = SequenceHelper.Normalize(
                    _aperiodicGenerator.GenerateSequence(totalValues),
                    _aperiodicGenerator.Divider)
                .ToArray();
        }

        public (double expectedValue, double variance, double standardDeviation, IEnumerable<double> distribution) Run(
            UniformNormalizedBasedGenerator generator,
            int totalValues,
            int totalIntervals)
        {
            return Core(
                generator,
                totalValues, 
                totalIntervals,
                _normalizedUniformDistributedSequence);
        }

        private (double expectedValue, double variance, double standardDeviation, IEnumerable<double> distribution) Core(
            UniformNormalizedBasedGenerator generator, 
            int totalValues,
            int totalIntervals, 
            IEnumerable<double> uniformNormalizedSeq)
        {
            var newNotNormalizedSeq = generator.GenerateSequence(uniformNormalizedSeq);
            (double expVal, double variance, double standardDeviation) = StatisticsGenerator.GetStatistics(newNotNormalizedSeq);

            var dist = generator.Distribution;
            double begin = dist.MinValue;
            double end = dist.MaxValue;

            var distribution = SequenceHelper.GetDistribution(newNotNormalizedSeq, begin, end, totalIntervals)
                .Select(i => i / (double)totalValues).ToArray();

            return (expVal, variance, standardDeviation, distribution);
        }
    }
}
