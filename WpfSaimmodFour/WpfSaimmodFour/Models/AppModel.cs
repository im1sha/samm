using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        #region calculated properties

        public Dictionary<(bool? Queue, bool? Channel), double> StatesProbabilities { get; private set; }

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
            StatesProbabilities = new Dictionary<(bool? Queue, bool? Channel), double>();
        }

        #endregion

        #region public methods

        public (double RelativeProbabilityOfPriorityItems, double RelativeProbabilityOfUsualItems) Run()
        {
#if DEBUG
            StringBuilder deb_output = new StringBuilder(string.Empty);
#endif

            Dictionary<(bool? Queue, bool? Channel), double> statesTimeCounter
                = new Dictionary<(bool? Queue, bool? Channel), double>();

            // generatorData[i].Time & channelTime store moments of time (NOT deltas)
            var (generatorData, channelTime, maxTimeBound) = GetReady(
                GeneratorIntensivity, ChannelIntesivity,
                HighPriorityItemPorbability, TimeApproximation);

            double time = 0.0;
            (int PriorityItem, int UsualItem) dropCount = (0, 0);

            (int Index, double EventTime, bool IsPriority) lastGeneratorEventDesription
                = (-1, 0, false);
            (int Index, double EventTime) lastChannelEventDesription = (-1, 0);
            // null is free
            // true is high priority item
            // false is low priority item
            (bool? Queue, bool? Channel) systemState = (null, null);
            (bool IsGeneratorEvent, bool IsPriority) currentEvent;

            double nextGeneratorTime;
            double nextChannelTime;
            int nextChannelIndex;
            int nextGeneratorIndex;

            while (time < maxTimeBound)
            {
                nextGeneratorIndex = lastGeneratorEventDesription.Index + 1;
                nextChannelIndex = lastChannelEventDesription.Index + 1;
                nextGeneratorTime = generatorData.ElementAt(nextGeneratorIndex).Time;
                nextChannelTime = channelTime.ElementAt(nextChannelIndex);

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

                #region statistics

                if (!statesTimeCounter.ContainsKey(systemState))
                {
                    statesTimeCounter.Add(systemState, 0);
                }
                statesTimeCounter[systemState] += (Math.Min(nextGeneratorTime, nextChannelTime) - time);

                #endregion 


#if DEBUG
                deb_output.Append($"{time.ToString().PadRight(20, '0')}" +
                    $"{((systemState.Queue == null) ? 0.ToString() : (systemState.Queue == true ? 2.ToString() : 1.ToString()))}"+
                    $"{((systemState.Channel == null) ? 0.ToString() : (systemState.Channel == true ? 2.ToString() : 1.ToString()))}: " +
                    $"{(currentEvent.IsGeneratorEvent ? "L" : "M")}" +
                    $"{((!currentEvent.IsGeneratorEvent) ? "" : (currentEvent.IsPriority ? "(P)" : "(1-P)"))}\n");
#endif
                
                time = Math.Min(nextGeneratorTime, nextChannelTime);     
                
                if (ShouldDropItem(systemState, currentEvent))
                {
                    if (IsPriorityDroppedItem(systemState, currentEvent.IsPriority))
                    {
                        dropCount.PriorityItem++;
                    }
                    else
                    {
                        dropCount.UsualItem++;
                    }
                }

                systemState = ChangeState(systemState, currentEvent);
            }

            # region states probabilities
            StatesProbabilities.Clear();
            foreach (var item in statesTimeCounter)
            {
                StatesProbabilities.Add(item.Key, item.Value / statesTimeCounter.Sum(i => i.Value));
            }
            #endregion

            int totalEmitted = lastGeneratorEventDesription.Index;
            int totalPriorityItemsEmitted = generatorData.Take(totalEmitted).Count(i => i.IsPriorityItem);
            int totalUsualItemsEmitted = totalEmitted - totalPriorityItemsEmitted;

#if DEBUG
            File.WriteAllText(DateTime.Now.ToBinary().ToString(), deb_output.ToString());
#endif

            return ((totalPriorityItemsEmitted - dropCount.PriorityItem) / (double)totalPriorityItemsEmitted,
                (totalUsualItemsEmitted - dropCount.UsualItem) / (double)totalUsualItemsEmitted);
        }

        #endregion

        #region private methods

        private static (bool? Queue, bool? Channel) ChangeState(
            (bool? Queue, bool? Channel) systemState,
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
                default:
                    throw new ArgumentException(nameof(systemState));
            }
            return systemState;
        }

        private static (IEnumerable<(double Time, bool IsPriorityItem)> GeneratorData,
            IEnumerable<double> ChannelTimes, double Time) GetReady(
            double generatorIntensivity,
            double channelIntesivity,
            double highPriorityItemPorbability,
            double timeApproximation)
        {
            var initilizationRandom = new Random();

            var generatorForSource = new ExponentialGeneratorWrapper(
                generatorIntensivity, new Random(initilizationRandom.Next()), timeApproximation);
            var generatorForChannel = new ExponentialGeneratorWrapper(
                channelIntesivity, new Random(initilizationRandom.Next()), timeApproximation);
            IEnumerable<double> generatorAccumulatedDistribution = generatorForSource
                .AccumulateDistribution(generatorForSource.GenerateDistribution());
            IEnumerable<double> channelAccumulatedDistribution = generatorForChannel
                .AccumulateDistribution(generatorForChannel.GenerateDistribution());

            var boolRandom = new Random();

            double time = Math.Min(generatorAccumulatedDistribution.Last(), channelAccumulatedDistribution.Last());

            IEnumerable<(double Time, bool IsPriorityItem)> generatorData
                = Enumerable.Range(0, generatorAccumulatedDistribution.Count())
                .Select((item, index) =>
                    (generatorAccumulatedDistribution.ElementAt(index),
                    boolRandom.NextDouble() < highPriorityItemPorbability)).ToArray();

            return (generatorData, channelAccumulatedDistribution, time);
        }

        private static bool ShouldDropItem(
            (bool? Queue, bool? Channel) systemState,
            (bool IsGeneratorEvent, bool IsPriority) lastEvent)
        {
            if (systemState.Channel != null
                && systemState.Queue != null
                && lastEvent.IsGeneratorEvent)
            {
                return true;
            }
            return false;
        }

        // use if ShouldDropItem returns true =>
        // systemStates in {(true, true), (false, false), (false, true)}
        private static bool IsPriorityDroppedItem(
            (bool? Queue, bool? Channel) systemState,
            bool IsPriorityItem)
        {
            if (systemState.Queue == true
                && systemState.Channel == true
                && IsPriorityItem)
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
