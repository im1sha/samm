using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodTwo.Models.Generators;

namespace WpfSaimmodFour.Models
{
    public class AppModel
    {
        public double GeneratorIntensivity { get; }
        public double ChannelIntesivity { get; }
        public double HighPriorityItemPorbability { get; }


        public IEnumerable<double> Meth(
            ExponentialDistributionGenerator generator,
            IEnumerable<double> uniformNormalizedSequence)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }
            if (uniformNormalizedSequence == null)
            {
                throw new ArgumentNullException(nameof(uniformNormalizedSequence));
            }

            return generator.GenerateSequence(uniformNormalizedSequence).ToArray();
        }

        public AppModel(double generatorIntensivity,
            double channelIntesivity,
            double highPriorityItemPorbability)
        {
            GeneratorIntensivity = generatorIntensivity;
            ChannelIntesivity = channelIntesivity;
            HighPriorityItemPorbability = highPriorityItemPorbability;
        }


    }
}
