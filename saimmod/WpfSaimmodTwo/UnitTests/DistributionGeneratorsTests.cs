using NUnit.Framework;
using System;
using WpfSaimmodTwo.Models;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;
using WpfSaimmodTwo.Utils;

namespace UnitTests
{
    [TestFixture]
    public class DistributionGeneratorsTests
    {
        private static readonly uint _initialValue = 2684222873;
        private static readonly uint _divider = 4291343633;
        private static readonly uint _multiplier = 2595211397;

        [TestCase(5, 105, 0.1, 30_000)]
        [TestCase(-49, 51, 0.1, 30_000)]
        [TestCase(-101, -1, 0.1, 30_000)]
        public void UniformDistributionGeneratorTest(double min, double max, double epsilon, int totalValues)
        {
            var generator = new UniformDistributionGenerator(new UniformDistribution(min, max));  
            (double exp, _, double stdDiv) = GetStat(generator, totalValues);
            Assert.IsTrue((exp < ((min + max) / 2) + epsilon)
                && (exp > ((min + max) / 2) - epsilon));
        }

        [TestCase(5, 2, 0.02, 30_000)]
        [TestCase(-49, 5, 0.05, 30_000)]
        [TestCase(-101, 10, 0.1, 30_000)]
        public void NormalDistributionGeneratorTest(double expectedValue, double variance, double epsilon, int totalValues)
        {
            var generator = new NormalDistributionGenerator(new NormalDistribution(expectedValue, variance));
            (double resultExp, double resultVariance, _) = GetStat(generator, totalValues);
            Assert.IsTrue((resultExp < expectedValue + epsilon) 
                && (resultExp > expectedValue - epsilon)
                && (Math.Sqrt(variance) + epsilon > Math.Sqrt(resultVariance))
                && (Math.Sqrt(variance) - epsilon < Math.Sqrt(resultVariance)));
        }

        [TestCase(0, 100, 0.5, 1, 30_000)]
        [TestCase(-50, 50, 1, 1, 30_000)]
        [TestCase(-101, -1, 2, 1, 30_000)]
        public void ExponentialDistributionGeneratorTest(double begin, double end, double lambda, double epsilon, int totalValues)
        {
            var generator = new ExponentialDistributionGenerator(new ExponentialDistribution(begin, end, lambda));
            (double resultExp, double resultVariance, _) = GetStat(generator, totalValues);
            Assert.IsTrue((resultExp < 1 / lambda + epsilon)
                && (resultExp > 1 / lambda - epsilon)
                && (Math.Sqrt(1 / lambda) + epsilon > Math.Sqrt(resultVariance))
                && (Math.Sqrt(1 / lambda) - epsilon < Math.Sqrt(resultVariance)));
        }

        private (double expectedValue, double variance, double standardDeviation) GetStat(
            UniformNormalizedBasedGenerator generator, int totalValues)
        {
            var seq = new LehmerGenerator(_multiplier, _initialValue, _divider).GenerateSequence(totalValues);
            var normalized = SequenceHelper.Normalize(seq, _divider);
            var newNotNormalized = generator.GenerateSequence(normalized);
            return StatisticsGenerator.GetStatistics(newNotNormalized);
        }
    }
}
