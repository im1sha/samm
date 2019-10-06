using NUnit.Framework;
using System.Linq;
using WpfSaimmodTwo.Models.Core;
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

        private const int USUAL_TEST_LENGTH = 50_000;
        private const int USUAL_INTERVALS = 20;

        private void CheckAssert(UniformNormalizedBasedGenerator generator, NotNormalizedDistribution dist,
          double distributionEpsilon, double statEpsilon,
          int totalValues, int totalIntervals)
        {
            // 
            var uniformNotNormalizedSeq = new LehmerGenerator(_multiplier, _initialValue, _divider)
                .GenerateSequence(totalValues);
            var uniformNormalizedSeq = SequenceHelper.Normalize(uniformNotNormalizedSeq, _divider);
            //
            var newNotNormalizedSeq = generator.GenerateSequence(uniformNormalizedSeq);
            (double expVal, double variance, _) = StatisticsGenerator.GetStatistics(newNotNormalizedSeq);

            double begin = dist.MinValue;
            double end = dist.MaxValue;

            var distribution = SequenceHelper.GetDistribution(newNotNormalizedSeq, begin, end, totalIntervals)
                .Select(i => i / (double)totalValues).ToArray();

            var distResults = dist.EstimateDistribution(distribution, distributionEpsilon);
            var statResults = dist.EstimateStatistics(expVal, variance, statEpsilon);
            Assert.IsTrue(distResults && statResults);
        }

        [TestCase(-513, 499, 0.01, 1.5, USUAL_TEST_LENGTH, USUAL_INTERVALS * 2)]
        [TestCase(511, 1477, 0.01, 1.5, USUAL_TEST_LENGTH, USUAL_INTERVALS * 2)]
        [TestCase(-1538, -497, 0.01, 1.5, USUAL_TEST_LENGTH, USUAL_INTERVALS * 2)]
        public void UniformDistributionGeneratorTest(double begin, double end,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new UniformDistribution(begin, end);
            var generator = new UniformDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }

        [TestCase(511, 50, 0.005, 1, USUAL_TEST_LENGTH, USUAL_INTERVALS * 5)]
        public void NormalDistributionGeneratorTest(double expectedValue, double variance,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new NormalDistribution(expectedValue, variance);
            var generator = new NormalDistributionGenerator(dist);
            CheckAssert(
               generator, dist,
               distributionEpsilon, statEpsilon,
               totalValues, totalIntervals);
        }

        [TestCase(0.05, 0.005, 1, USUAL_TEST_LENGTH, USUAL_INTERVALS * 3)]
        public void ExponentialDistributionGeneratorTest(double lambda,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new ExponentialDistribution(lambda);
            var generator = new ExponentialDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }

        [TestCase(2, 0.05, 0.005, 1, USUAL_TEST_LENGTH, USUAL_INTERVALS * 3)]
        public void GammaDistributionGeneratorTest(int eta, double lambda,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new GammaDistribution(eta, lambda);
            var generator = new GammaDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }

        [TestCase(-50, 1555, 0.05, double.NaN, USUAL_TEST_LENGTH, USUAL_INTERVALS * 2)]
        public void TriangularDistributionGeneratorTest(double begin, double end,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new TriangularDistribution(begin, end);
            var generator = new TriangularDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }

        [TestCase(-1421, 2001, 0.05, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        public void SimpsonDistributionGeneratorTest(double begin, double end,
           double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new SimpsonDistribution(begin, end);
            var generator = new SimpsonDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }
    }
}
