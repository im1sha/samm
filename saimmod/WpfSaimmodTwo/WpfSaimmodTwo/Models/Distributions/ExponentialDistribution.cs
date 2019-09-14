﻿namespace WpfSaimmodTwo.Models.Distributions
{
    public class ExponentialDistribution : NotNormalizedDistribution
    {
        public ExponentialDistribution(double begin, double end, double lambda)
            : base(begin, end, 1 / lambda, 1 / lambda / lambda)
        {
        }
    }
}
