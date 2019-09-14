using System.Collections.Generic;

namespace WpfSaimmodTwo.Interfaces.Generators
{
    internal interface IUniformNormalizedBasedGenerator : IGenerator
    {
        /// <summary>
        /// Generates values from normalized sequence 
        /// </summary>
        /// <param name="values">Normalized sequence (in range [0,1])</param>
        /// <returns>New sequence (can be not normilized)</returns>
        IEnumerable<double> GenerateSequence(IEnumerable<double> values);
    }
}
