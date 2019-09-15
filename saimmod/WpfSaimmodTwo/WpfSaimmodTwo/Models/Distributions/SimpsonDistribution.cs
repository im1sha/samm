﻿using System;
using System.Collections.Generic;

namespace WpfSaimmodTwo.Models.Distributions
{
    public class SimpsonDistribution : NotNormalizedDistribution
    {
        public SimpsonDistribution(double begin, double end)
            : base(begin, end, (begin + end) / 2.0, Math.Pow(begin - end, 2.0) / 12.0)
        {

        }

        public override bool EstimateDistribution(IEnumerable<double> values, double epsilon)
        {
            throw new NotImplementedException();
        }
    }
}
