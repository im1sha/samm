using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Models.Distributions;
using WpfSaimmodTwo.Models.Generators;

namespace WpfSaimmodFour.Models
{
    public class ExponentialGeneratorWrapper
    {
        public double Lambda { get; }

        public double LengthApproximation { get; }

        private readonly Random _random;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lambda">Exponential distribution parameter</param>
        /// <param name="random">Unique random instance</param>
        /// <param name="lengthApproximation">Expected resulting sequence sum</param>
        public ExponentialGeneratorWrapper(double lambda,
            Random random,
            double lengthApproximation)
        {
            Lambda = lambda;
            LengthApproximation = lengthApproximation;
            _random = random;
        }

        /// <summary>
        /// Generates sequence that has exponential distribution
        /// </summary>
        /// <returns>Sequence that has exponential distribution</returns>
        public IEnumerable<double> GenerateDistribution()
        {
            var generator = new ExponentialDistributionGenerator(new ExponentialDistribution(Lambda));

            var uniformNormalizedSequence = Enumerable.Range(
                0, (int)(LengthApproximation / generator.Distribution.RightExpectedValue)
                ).Select(i => _random.NextDouble()).ToArray();

            return generator.GenerateSequence(uniformNormalizedSequence).ToArray();
        }

        public static IEnumerable<double> AccumulateDistribution(IEnumerable<double> distribution)
        {
            if (distribution == null)
            {
                throw new ArgumentNullException(nameof(distribution));
            }

            List<double> result = new List<double>();

            var sum = 0.0;

            foreach (var item in distribution)
            {
                sum += item;
                result.Add(sum);
            }

            return result;
        }
    }
}
