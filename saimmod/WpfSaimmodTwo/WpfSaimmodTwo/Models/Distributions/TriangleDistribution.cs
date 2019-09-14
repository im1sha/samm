using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSaimmodTwo.Interfaces.Distributions;
namespace WpfSaimmodTwo.Models.Distributions
{
    class TriangleDistribution : INotNormalizedDistribution
    {
        public double RightExpectedValue => throw new NotImplementedException();

        public double RightVariance => throw new NotImplementedException();

        public double Begin => throw new NotImplementedException();

        public double End => throw new NotImplementedException();

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
    }
}
