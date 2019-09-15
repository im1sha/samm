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

        private void CheckAssert(UniformNormalizedBasedGenerator generator, NotNormalizedDistribution dist,
          double? begin, double? end,
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

            double castBegin, castEnd;
            if (begin == null || end == null)
            {
                castBegin = newNotNormalizedSeq.Min();
                castEnd = newNotNormalizedSeq.Max();
            }
            else
            {
                castBegin = (double)begin;
                castEnd = (double)end;
            }

            var distribution = SequenceHelper.GetDistribution(newNotNormalizedSeq, castBegin, castEnd, totalIntervals)
                .Select(i => i / (double)totalValues).ToArray();

            var distResults = dist.EstimateDistribution(distribution, distributionEpsilon);
            var statResults = dist.EstimateStatistics(expVal, variance, statEpsilon);
            Assert.IsTrue(distResults && statResults);
        }

        [TestCase(-513, 499, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        [TestCase(511, 1477, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        [TestCase(-1538, -497, 0.005, 0.5, USUAL_TEST_LENGTH, USUAL_INTERVALS)]
        public void UniformDistributionGeneratorTest(double begin, double end,
            double distributionEpsilon, double statEpsilon, int totalValues, int totalIntervals)
        {
            var dist = new UniformDistribution(begin, end);
            var generator = new UniformDistributionGenerator(dist);
            CheckAssert(
                generator, dist,
                begin, end,
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
               null, null,
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
                null, null,
                distributionEpsilon, statEpsilon,
                totalValues, totalIntervals);
        }

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
