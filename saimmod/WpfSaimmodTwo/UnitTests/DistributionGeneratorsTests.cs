using NUnit.Framework;
using WpfSaimmodTwo.Models;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;
using WpfSaimmodTwo.Utils;

namespace UnitTests
{
    [TestFixture]
    public class DistributionGeneratorsTests
    {
        private static readonly uint _initialValue = 3479695279;
        private static readonly uint _divider = 4290123121;
        private static readonly uint _multiplier = 2172724891;

        [TestCase(5, 105, 0.1)]
        [TestCase(-49, 51, 0.1)]
        [TestCase(-101, -1, 0.1)]
        public void UniformDistributionGeneratorTest(double min, double max, double epsilon)
        {
            var generator = new UniformDistributionGenerator(new UniformDistribution(min, max));
            var seq = new LehmerGenerator(_multiplier, _initialValue, _divider).GenerateSequence(500_000);
            var normalized = SequenceHelper.Normalize(seq, _divider);
            var newNotNormalized = generator.GenerateSequence(normalized);

            (double exp, _, _) = StatisticsGenerator.GetStatistics(newNotNormalized);

            Assert.IsTrue(exp < (min + max) / 2 + epsilon && exp > (min + max) / 2 - epsilon);
        }
    }
}
