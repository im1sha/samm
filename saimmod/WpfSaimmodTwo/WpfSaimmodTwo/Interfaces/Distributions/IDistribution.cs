using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Distributions
{
    internal interface IDistribution
    {
        double RightExpectedValue { get; }

        double RightVariance { get; }

        double Begin { get; }

        double End { get; }

        (double expectedValue, double variance, double standardDeviation) GetStatistics(
            IEnumerable<double> values);

    }
}
