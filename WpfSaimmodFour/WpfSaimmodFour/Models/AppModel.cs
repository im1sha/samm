using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSaimmodFour.Models
{
    public class AppModel
    {
        #region const data

        public double GeneratorIntensivity { get; }
        public double ChannelIntesivity { get; }
        public double HighPriorityItemPorbability { get; }
        public double TimeApproximation { get; }

        #endregion

        #region ctor

        public AppModel(double generatorIntensivity,
            double channelIntesivity,
            double highPriorityItemPorbability,
            double timeApproximation)
        {
            GeneratorIntensivity = generatorIntensivity;
            ChannelIntesivity = channelIntesivity;
            HighPriorityItemPorbability = highPriorityItemPorbability;
            TimeApproximation = timeApproximation;
        }

        #endregion

        #region public methods

        public void Run()
        {
             (IEnumerable<(double Interval, bool IsPriorityItem)> generatorData,
                IEnumerable<double> channelIntervals,
                double time)  
                = GetReady(GeneratorIntensivity, ChannelIntesivity, HighPriorityItemPorbability, TimeApproximation);
        }

        #endregion

        #region private methods

        private (IEnumerable<(double Interval, bool IsPriorityItem)> generatorData,
            IEnumerable<double> channelIntervals, double time) GetReady(
            double generatorIntensivity,
            double channelIntesivity,
            double highPriorityItemPorbability,
            double timeApproximation)
        {
            IEnumerable<double> generatorDistribution = new ExponentialGeneratorWrapper(
                generatorIntensivity, new Random(1), timeApproximation).Generate();
            IEnumerable<double> channelDistribution = new ExponentialGeneratorWrapper(
                channelIntesivity, new Random(2), timeApproximation).Generate();

            var boolRandom = new Random(3);

            double time = Math.Min(generatorDistribution.Sum(), channelDistribution.Sum());

            IEnumerable<(double Interval, bool IsPriorityItem)> generatorData
                = Enumerable.Range(0, generatorDistribution.Count())
                .Select((item, index) => 
                    (generatorDistribution.ElementAt(index),
                    boolRandom.NextDouble() < highPriorityItemPorbability)).ToArray();
         
            return (generatorData, channelDistribution, time);
        }

        private void Generator()
        {

        }

        private void Channel()
        {

        }


        #endregion

    }
}
