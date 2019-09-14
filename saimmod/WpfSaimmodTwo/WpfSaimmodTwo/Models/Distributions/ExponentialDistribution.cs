namespace WpfSaimmodTwo.Models.Distributions
{
    internal class ExponentialDistribution : NotNormalizedDistribution
    {
        public ExponentialDistribution(double begin, double end)
           : base(begin, end, 0, 0)
        { 
        }   
    }
}
