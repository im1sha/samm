using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces
{
    internal interface INormalizedDistribution
    {
        (double expectedValue, double variance, double standardDeviation) GetStatistics(
           IEnumerable<double> values);

        double CalculateIndirectEstimation(IEnumerable<double> values);

        bool CheckIndirectEstimation(double estimation, double epsilon);

        IEnumerable<int> GetDistribution(IEnumerable<double> values, int totalIntervals);
    }
}
