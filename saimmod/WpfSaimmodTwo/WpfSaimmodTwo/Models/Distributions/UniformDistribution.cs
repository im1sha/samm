using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models.Distributions
{
    internal class UniformDistribution : INotNormalizedDistribution
    {
        public double Begin { get; }

        public double End { get; }

        public double RightExpectedValue => (Begin + End) / 2.0;

        public double RightVariance => Math.Pow(Begin - End, 2.0) / 12.0;

        public UniformDistribution(double begin, double end)
        {
            Begin = begin;
            End = end;
        }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)       
            => StatisticsGenerator.GetStatistics(values);
        
    }
}
