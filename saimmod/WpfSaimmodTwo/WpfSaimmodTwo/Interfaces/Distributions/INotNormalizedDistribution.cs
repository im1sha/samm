namespace WpfSaimmodTwo.Interfaces.Distributions
{
    public interface INotNormalizedDistribution : IDistribution
    {
        double[] AdditionalParameters { get; }
    }
}
