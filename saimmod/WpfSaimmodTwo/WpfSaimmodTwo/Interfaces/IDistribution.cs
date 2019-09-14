using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces
{
    internal interface IDistribution
    {
        double Begin { get; }
        double End { get; }

        (double expectedValue, double variance, double standardDeviation) GetStatistics(
          IEnumerable<double> values);

        IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals);
    }
}
