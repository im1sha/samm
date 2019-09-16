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
                = SequenceHelper.Normalize(
                    _aperiodicGenerator.GenerateSequence(totalValues),
                    _aperiodicGenerator.Divider)
                .ToArray();
        }

        public (double expectedValue, double variance, double standardDeviation, IEnumerable<double> distribution) Run(
            UniformNormalizedBasedGenerator generator,
            NotNormalizedDistribution dist,
            int totalValues, int totalIntervals)
        {
            return Core(
                generator, dist,
                totalValues, totalIntervals,
                _normalizedUniformDistributedSequence);
        }

        private (double expectedValue, double variance, double standardDeviation, IEnumerable<double> distribution) Core(
            UniformNormalizedBasedGenerator generator, 
            NotNormalizedDistribution dist,
            int totalValues,
            int totalIntervals, 
            IEnumerable<double> uniformNormalizedSeq)
        {
            var newNotNormalizedSeq = generator.GenerateSequence(uniformNormalizedSeq);
            (double expVal, double variance, double standardDeviation) = StatisticsGenerator.GetStatistics(newNotNormalizedSeq);

            double begin = dist.MinValue;
            double end = dist.MaxValue;

            var distribution = SequenceHelper.GetDistribution(newNotNormalizedSeq, begin, end, totalIntervals)
                .Select(i => i / (double)totalValues).ToArray();

            return (expVal, variance, standardDeviation, distribution);
        }
    }
}
