using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Distributions
{
    public interface IDistribution
    {
        double RightExpectedValue { get; }

        double RightVariance { get; }

        (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values);

        double MinValue { get; }

        double MaxValue { get; }
    }
}
