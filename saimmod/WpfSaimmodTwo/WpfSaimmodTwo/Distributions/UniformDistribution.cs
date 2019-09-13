using WpfSaimmodTwo.Interfaces;

namespace WpfSaimmodTwo.Distributions
{
    internal class UniformDistribution : IDistribution
    {
        public double Min { get; }
        public double Max { get; }
        public UniformDistribution(double min, double max)
        {
            Max = max;
            Min = min;
        }
    }
}
