using System;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class GammaDistribution : NotNormalizedDistribution
    {
        public GammaDistribution(double begin, double end, double eta, double lambda)
            : base(begin, end, eta/lambda, eta/lambda/lambda, new[] { eta, lambda })
        {
        }
    }
}
