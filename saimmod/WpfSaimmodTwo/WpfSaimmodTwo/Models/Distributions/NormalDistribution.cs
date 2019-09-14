namespace WpfSaimmodTwo.Models.Distributions
{
    internal class NormalDistribution : NotNormalizedDistribution
    {
        public NormalDistribution(double begin, double end)
           : base(begin, end, 0, 0)
        {

        }
    }
}
