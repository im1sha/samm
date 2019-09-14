using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    internal abstract class NotNormalizedDistribution : INotNormalizedDistribution
    {
        public double RightExpectedValue { get; }

        public double RightVariance { get; }

        public double Begin { get; }

        public double End { get; }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)
        {
            return StatisticsGenerator.GetStatistics(values);
        }

        public NotNormalizedDistribution(double begin, double end, double rightExpectedValue, double rightVariance)
        {
            if (begin >= end)
            {
                throw new ArgumentException();
            }
            RightExpectedValue = rightExpectedValue;
            RightVariance = rightVariance;
            Begin = begin;
            End = end;
        }
    }
}
