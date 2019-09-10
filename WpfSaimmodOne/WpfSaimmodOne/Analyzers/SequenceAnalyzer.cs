using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodOne.Analyzers
{
    internal static class SequenceAnalyzer
    {
        public static void ThrowNotUnique(IEnumerable<uint> data)
        {        
            if (data.Count() != data.Distinct().Count())
            {
                throw new ApplicationException();
            }                                 
        }
    }
}
