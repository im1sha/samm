using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodThree.Models
{
    public class ProbabilityItem
    {
        public string State { get; }
        public double Probability { get; }

        public ProbabilityItem(string state, double probability)
        {
            State = state;
            Probability = probability;
        }
    }
}
