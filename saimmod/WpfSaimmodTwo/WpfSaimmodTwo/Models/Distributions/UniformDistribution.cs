using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces;

namespace WpfSaimmodTwo.Models.Distributions
{
    internal class UniformDistribution : IDistribution
    {
        public double Begin { get; }

        public double End { get; }

        public UniformDistribution(double begin, double end)
        {
            Begin = begin;
            End = end;
        }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetDistribution(IEnumerable<double> values, double begin, double end, int totalIntervals)
        {
            throw new NotImplementedException();
        }
    }
}
