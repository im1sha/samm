using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Distributions
{
    public interface INormalizedDistribution : IDistribution
    {      
        double CalculateIndirectEstimation(IEnumerable<double> values);

        bool CheckIndirectEstimation(double estimation, double epsilon);
    }
}
