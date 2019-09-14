using System;
using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;
using WpfSaimmodTwo.Utils;

namespace WpfSaimmodTwo.Models
{
    public abstract class NotNormalizedDistribution : INotNormalizedDistribution
    {
        public double RightExpectedValue { get; }

        public double RightVariance { get; }

        public double MinValue { get; }

        public double MaxValue { get; }

        public double[] AdditionalParameters { get; }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)
        {
            return StatisticsGenerator.GetStatistics(values);
        }

        public NotNormalizedDistribution(double minValue, double maxValue, double rightExpectedValue, 
            double rightVariance, double[] additionalParameter = null)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentException();
            }
            MinValue = minValue;
            MaxValue = maxValue;
            RightExpectedValue = rightExpectedValue;
            RightVariance = rightVariance;
            AdditionalParameters = additionalParameter;
        }
    }
}
