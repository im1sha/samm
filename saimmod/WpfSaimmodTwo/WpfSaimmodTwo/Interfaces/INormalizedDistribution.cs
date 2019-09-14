using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces
{
    internal interface INormalizedDistribution : IDistribution
    {      
        double CalculateIndirectEstimation(IEnumerable<double> values);

        bool CheckIndirectEstimation(double estimation, double epsilon);

        IEnumerable<int> GetNormalizedDistribution(IEnumerable<double> values, int totalIntervals);
    }
}
