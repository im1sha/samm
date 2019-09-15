using NUnit.Framework;
using System.Linq;
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

        private const int USUAL_TEST_LENGTH = 30_000;
        private const int USUAL_INTERVALS = 20;

        [TestCase(-513, 499, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        [TestCase(511, 1477, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        [TestCase(-1538, -497, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]

        public void UniformDistributionGeneratorTest(double min, double max,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new UniformDistribution(min, max);
            var generator = new UniformDistributionGenerator(dist);
            // 
            var uniformNotNormalizedSeq = new LehmerGenerator(_multiplier, _initialValue, _divider).GenerateSequence(totalValues);
            var uniformNormalizedSeq = SequenceHelper.Normalize(uniformNotNormalizedSeq, _divider);
            //
            var newNotNormalizedSeq = generator.GenerateSequence(uniformNormalizedSeq);
            (double expVal, double variance, _) = StatisticsGenerator.GetStatistics(newNotNormalizedSeq);
            var distribution = SequenceHelper.GetDistribution(newNotNormalizedSeq, min, max, totalIntervals).Select(i => i / (double)totalValues);

            var distResults = dist.EstimateDistribution(distribution, distributionEpsilon);
            var statResults = dist.EstimateStatistics(expVal, variance, statEpsilon);
            Assert.IsTrue(distResults && statResults);
        }

        //[TestCase(5, 2, 0.02, 30_000)]
        //[TestCase(-49, 5, 0.05, 30_000)]
        //[TestCase(-101, 10, 0.1, 30_000)]
        //public void NormalDistributionGeneratorTest(double expectedValue, double variance, double epsilon, int totalValues)
        //{
        //    var dist = new NormalDistribution(expectedValue, variance);
        //    var generator = new NormalDistributionGenerator(dist);
        //    (double resultExp, double resultVariance, _) = GetStat(generator, totalValues);
        //    Assert.IsTrue((resultExp < expectedValue + epsilon)
        //        && (resultExp > expectedValue - epsilon)
        //        && (Math.Sqrt(variance) + epsilon > Math.Sqrt(resultVariance))
        //        && (Math.Sqrt(variance) - epsilon < Math.Sqrt(resultVariance)));
        //}

        //[TestCase(0, 100, 0.5, 1, 30_000)]
        //[TestCase(-50, 50, 1, 1, 30_000)]
        //[TestCase(-101, -1, 2, 1, 30_000)]
        //public void ExponentialDistributionGeneratorTest(double begin, double end, double lambda, double epsilon, int totalValues)
        //{
        //    var dist = new ExponentialDistribution(begin, end, lambda);
        //    var generator = new ExponentialDistributionGenerator(dist);
        //    (double resultExp, double resultVariance, _) = GetStat(generator, totalValues);
        //    Assert.IsTrue((resultExp < dist.RightExpectedValue + epsilon)
        //        && (resultExp > dist.RightExpectedValue - epsilon)
        //        && (Math.Sqrt(dist.RightVariance) + epsilon > Math.Sqrt(resultVariance))
        //        && (Math.Sqrt(dist.RightVariance) - epsilon < Math.Sqrt(resultVariance)));
        //}

        //[TestCase(0, 100, 2, 5000, 0.1, 30_000)]
        //[TestCase(-50, 50, 11, 1, 0.1, 30_000)]
        //[TestCase(-101, -1, 19, -5000, 0.1, 30_000)]
        //public void GammaDistributionGeneratorTest(double begin, double end, double eta, double lambda, double epsilon, int totalValues)
        //{
        //    var dist = new GammaDistribution(begin, end, eta, lambda);
        //    var generator = new GammaDistributionGenerator(dist);
        //    (double resultExp, double resultVariance, _) = GetStat(generator, totalValues);
        //    Assert.IsTrue((resultExp < dist.RightExpectedValue + epsilon)
        //        && (resultExp > dist.RightExpectedValue - epsilon)
        //        && (Math.Sqrt(dist.RightVariance) + epsilon > Math.Sqrt(resultVariance))
        //        && (Math.Sqrt(dist.RightVariance) - epsilon < Math.Sqrt(resultVariance)));
        //}

        //[TestCase(0, 10000, 30_000)]
        //[TestCase(-5005, 4995, 30_000)]
        //[TestCase(-10001, -1, 30_000)]

        //public void TriangleDistributionGeneratorTest(double begin, double end, int totalValues)
        //{
        //    var dist = new TriangleDistribution(begin, end);
        //    var generator = new TriangleDistributionGenerator(dist);
        //    Assert.DoesNotThrow(() => GetStat(generator, totalValues));
        //}

    }
}
