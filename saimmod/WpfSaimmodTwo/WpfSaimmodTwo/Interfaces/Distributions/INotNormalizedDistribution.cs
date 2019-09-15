using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Distributions
{
    public interface INotNormalizedDistribution : IDistribution
    {
        double[] AdditionalParameters { get; }

        bool EstimateDistribution(IEnumerable<double> values, double epsilon);

        bool EstimateStatistics(double expectedValue, double variance, double epsilon);

        void OverrideMinMax(double min, double max);
    }
}
