namespace WpfSaimmodTwo.Models.Distributions
{
    internal class GammaDistribution : NotNormalizedDistribution
    {
        public GammaDistribution(double begin, double end)
           : base(begin, end, 0, 0)
        {

        }
    }
}
