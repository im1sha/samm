﻿using System.Collections.Generic;
using WpfSaimmodTwo.Interfaces.Distributions;

namespace WpfSaimmodTwo.Interfaces.Generators
{
    public interface IUniformNormalizedBasedGenerator : IGenerator
    {
        /// <summary>
        /// Generates values from normalized sequence 
        /// </summary>
        /// <param name="values">Normalized sequence (in range [0,1])</param>
        /// <returns>New sequence (can be not normilized)</returns>
        IEnumerable<double> GenerateSequence(IEnumerable<double> values);
    }
}
