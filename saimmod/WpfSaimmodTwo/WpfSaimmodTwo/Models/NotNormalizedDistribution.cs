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

        public double MinValue { get; private set; }

        public double MaxValue { get; private set; }

        public bool ShouldOverrideMinMax { get; }

        public double[] AdditionalParameters { get; }

        public NotNormalizedDistribution(double minValue, double maxValue, double rightExpectedValue,
            double rightVariance, double[] additionalParameter = null, bool shouldOverrideMinMax = false)
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
            ShouldOverrideMinMax = shouldOverrideMinMax;
        }

        public (double expectedValue, double variance, double standardDeviation) GetStatistics(IEnumerable<double> values)
        {
            return StatisticsGenerator.GetStatistics(values);
        }

        /// <summary>
        /// Estimates whether distribution is correct
        /// </summary>
        /// <param name="values">List of {points in segments} / {total points}. Sum of values == 1.0</param>
        /// <param name="epsilon">Max error</param>
        /// <returns>Estimation</returns>
        public abstract bool EstimateDistribution(IEnumerable<double> values, double epsilon);

        public bool EstimateStatistics(double expectedValue, double variance, double epsilon)
        {
            if (double.IsNaN(RightExpectedValue) || double.IsNaN(RightVariance))
            {
                return true;
            }
            return (RightExpectedValue < expectedValue + epsilon)
                && (RightExpectedValue > expectedValue - epsilon)
                && (Math.Sqrt(variance) + epsilon > Math.Sqrt(RightVariance))
                && (Math.Sqrt(variance) - epsilon < Math.Sqrt(RightVariance));
        }

        public void OverrideMinMax(double min, double max)
        {
            if (!ShouldOverrideMinMax)
            {
                throw new ApplicationException();
            }

            MinValue = min;
            MaxValue = max;
        }
    }
}
