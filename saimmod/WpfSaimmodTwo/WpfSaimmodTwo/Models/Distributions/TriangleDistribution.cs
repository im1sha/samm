namespace WpfSaimmodTwo.Models.Distributions
{
    internal class TriangleDistribution : NotNormalizedDistribution
    {
        public TriangleDistribution(double begin, double end)
           : base(begin, end, 0, 0)
        {
        }
    }
}
