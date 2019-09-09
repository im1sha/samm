using System.Collections.Generic;

namespace WpfSaimmodOne
{
    internal interface IDistribution
    {
        IEnumerable<uint> GetChartData(IEnumerable<uint> values);

        (double expectedValue, double variance, double standardDeviation) GetNormalizedStatistics(
            IEnumerable<uint> values);

        (bool isCorrect, double value) EstimateDistribution(IEnumerable<uint> values);
    }
}
