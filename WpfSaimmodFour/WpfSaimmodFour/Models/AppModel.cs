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
            var (generatorData, channelIntervals, time) = GetReady(
                GeneratorIntensivity, ChannelIntesivity,
                HighPriorityItemPorbability, TimeApproximation);

            double currentTime = 0.0;
            (int Index, double EventTime, bool IsPriority) lastGeneratorEventDesription 
                = (-1, 0, false);
            (int Index, double EventTime) lastChannelEventDesription = (-1, 0);
            // null is free
            // true is high priority item
            // false is low priority item
            (bool? Generator, bool? Channel) systemState = (null, null);

            (bool IsGeneratorEvent, bool IsPriority) currentEvent;
            double nextGeneratorTime;
            double nextChannelTime;
            int nextChannelIndex;
            int nextGeneratorIndex;
            while (currentTime < time)
            {
                nextGeneratorIndex = lastGeneratorEventDesription.Index + 1;
                nextChannelIndex = lastChannelEventDesription.Index + 1;

                nextGeneratorTime = generatorData.ElementAt(nextGeneratorIndex).Interval
                    + lastGeneratorEventDesription.EventTime;
                nextChannelTime = channelIntervals.ElementAt(nextChannelIndex)
                    + lastChannelEventDesription.EventTime;

                // ignore nextGeneratorTime == nextChannelTime due to its porbability -> 0
                if (nextGeneratorTime < nextChannelTime)
                {
                    lastGeneratorEventDesription = (nextGeneratorIndex,
                        nextGeneratorTime,
                        generatorData.ElementAt(nextGeneratorIndex).IsPriorityItem);
                    currentEvent = (true, generatorData.ElementAt(nextGeneratorIndex).IsPriorityItem);
                }
                else
                {
                    lastChannelEventDesription = (nextChannelIndex, nextChannelTime);
                    currentEvent = (false, false);
                }
                time = Math.Min(nextGeneratorTime, nextChannelTime);

                systemState = ChangeState(systemState, currentEvent);
            }
        }

        #endregion

        #region private methods

        private static (bool? Generator, bool? Channel) ChangeState(
            (bool? Generator, bool? Channel) systemState, 
            (bool IsGeneratorEvent, bool IsPriority) lastEvent)
        {
            switch (systemState)
            {
                case (null, null):
                    {
                        if (lastEvent.IsGeneratorEvent && !lastEvent.IsPriority)
                        {
                            return (null, false);
                        }
                        else if (lastEvent.IsGeneratorEvent && lastEvent.IsPriority)
                        {
                            return (null, true);
                        }
                        break;
                    }
                case (null, false):
                    {
                        if (!lastEvent.IsGeneratorEvent)
                        {
                            return (null, null);
                        }
                        else if (lastEvent.IsGeneratorEvent && lastEvent.IsPriority)
                        {
                            return (false, true);
                        }
                        else if (lastEvent.IsGeneratorEvent && !lastEvent.IsPriority)
                        {
                            return (false, false);
                        }
                        break;
                    }
                case (false, false):
                    {
                        if (!lastEvent.IsGeneratorEvent)
                        {
                            return (null, false);
                        }
                        else if (lastEvent.IsGeneratorEvent && lastEvent.IsPriority)
                        {
                            return (false, true);
                        }

                        break;
                    }
                case (null, true):
                    {
                        if (!lastEvent.IsGeneratorEvent)
                        {
                            return (null, null);
                        }
                        else if (lastEvent.IsGeneratorEvent && lastEvent.IsPriority)
                        {
                            return (true, true);
                        }
                        else if (lastEvent.IsGeneratorEvent && !lastEvent.IsPriority)
                        {
                            return (false, true);
                        }
                        break;
                    }
                case (false, true):
                    {
                        if (!lastEvent.IsGeneratorEvent)
                        {
                            return (null, false);
                        }
                        else if (lastEvent.IsGeneratorEvent && lastEvent.IsPriority)
                        {
                            return (true, true);
                        }
                        break;
                    }
                case (true, true):
                    {
                        if (!lastEvent.IsGeneratorEvent)
                        {
                            return (null, true);
                        }
                        break;
                    }
            }         
            return systemState;
        }

        private static (IEnumerable<(double Interval, bool IsPriorityItem)> GeneratorData,
            IEnumerable<double> ChannelIntervals, double Time) GetReady(
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

    
        #endregion

    }
}
