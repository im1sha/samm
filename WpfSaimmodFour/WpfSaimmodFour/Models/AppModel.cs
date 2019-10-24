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
            Dictionary<(bool? Queue, bool? Channel), double> statesTimeCounter
                = new Dictionary<(bool? Queue, bool? Channel), double>();

            // generatorData[i].Time & channelTime store moments of time (NOT deltas)
            var (generatorData, channelTimeWhenActiveList, maxTimeBound) = GetReady(
                GeneratorIntensivity, ChannelIntesivity,
                HighPriorityItemPorbability, TimeApproximation);

#if DEBUG
            StringBuilder deb_output = new StringBuilder(string.Empty);
            deb_output.Append($"priority = {generatorData.Count(i => i.IsPriorityItem) / (double)generatorData.Count()} " +
                $"usual = {generatorData.Count(i => !i.IsPriorityItem) / (double)generatorData.Count()}" +
                $"\n");
#endif

            (double Total, double ActiveChannel) time = (0.0, 0.0);
            (int PriorityItem, int UsualItem) dropCount = (0, 0);

            (int Index, double EventTime, bool IsPriority) lastGeneratorEventDesription
                = (-1, 0, false);
            // it should count time for channel relatively to its active state
            (int Index, double EventTime) lastChannelEventWhenActiveDesription = (-1, 0);

            // null is free
            // true is high priority item
            // false is low priority item
            (bool? Queue, bool? Channel) systemState = (null, null);

            (bool IsGeneratorEvent, bool IsPriority) currentEvent;

            double nextGeneratorTime;
            double nextChannelTime;
            int nextChannelIndex;
            int nextGeneratorIndex;
            double nextTime;

            while (time.Total < maxTimeBound)
            {
                nextGeneratorIndex = lastGeneratorEventDesription.Index + 1;
                nextChannelIndex = lastChannelEventWhenActiveDesription.Index + 1;

                nextGeneratorTime = generatorData.ElementAt(nextGeneratorIndex).Time;
                nextChannelTime = (channelTimeWhenActiveList.ElementAt(nextChannelIndex) - time.ActiveChannel)
                    + time.Total;

                // ignore nextGeneratorTime == nextChannelTime due to its porbability -> 0               
                if ((systemState.Channel != null) && (nextChannelTime < nextGeneratorTime))
                {
                    lastChannelEventWhenActiveDesription = (nextChannelIndex, channelTimeWhenActiveList.ElementAt(nextChannelIndex));
                    currentEvent = (false, false);
                    nextTime = nextChannelTime;
                }
                else
                {
                    lastGeneratorEventDesription = (nextGeneratorIndex,
                        nextGeneratorTime,
                        generatorData.ElementAt(nextGeneratorIndex).IsPriorityItem);
                    currentEvent = (true, generatorData.ElementAt(nextGeneratorIndex).IsPriorityItem);
                    nextTime = nextGeneratorTime;
                }

                #region states time counter

                if (!statesTimeCounter.ContainsKey(systemState))
                {
                    statesTimeCounter.Add(systemState, 0.0);
                }
                statesTimeCounter[systemState] += (nextTime - time.Total);

                #endregion

                #region debug information colletion
#if DEBUG
                deb_output.Append(
                    $"{nameof(time.Total)}={time.Total.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                    $"{nameof(time.ActiveChannel)}={time.ActiveChannel.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                    $"{nameof(nextGeneratorTime)}={nextGeneratorTime.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                    $"{nameof(nextChannelTime)}={nextChannelTime.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                    $"{nameof(channelTimeWhenActiveList)}[]={channelTimeWhenActiveList.ElementAt(nextChannelIndex).ToString().PadRight(20, '0').Substring(0, 9)}  " +
                    $"{((systemState.Queue == null) ? 0.ToString() : (systemState.Queue == true ? 2.ToString() : 1.ToString()))}" +
                    $"{((systemState.Channel == null) ? 0.ToString() : (systemState.Channel == true ? 2.ToString() : 1.ToString()))}: " +
                    $"{(currentEvent.IsGeneratorEvent ? "L" : "M")}" +
                    $"{((!currentEvent.IsGeneratorEvent) ? "" : (currentEvent.IsPriority ? "(P)" : "(1-P)"))}  " +
                    $"\n"
                );
#endif
                #endregion

                if (systemState.Channel != null)
                {
                    time.ActiveChannel += (nextTime - time.Total);
                }
                time.Total = nextTime;

                switch (ShouldDropPriorityItem(systemState, currentEvent))
                {
                    case true:
                        dropCount.PriorityItem++;
                        break;
                    case false:
                        dropCount.UsualItem++;
                        break;
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

            #region debug output
#if DEBUG
            var deb_fileName = DateTime.Now.ToBinary().ToString();
            File.WriteAllText(deb_fileName, deb_output.ToString());
#endif
            #endregion

            int totalEmitted = lastGeneratorEventDesription.Index;
            int totalPriorityItemsEmitted = generatorData.Take(totalEmitted).Count(i => i.IsPriorityItem);
            int totalUsualItemsEmitted = totalEmitted - totalPriorityItemsEmitted;

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

        private static bool? ShouldDropPriorityItem(
            (bool? Queue, bool? Channel) systemState,
            (bool IsGeneratorEvent, bool IsPriority) lastEvent)
        {
            #region inner functions

            static bool ShouldDropItem(
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
            static bool IsPriorityDroppedItem(
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

            if (ShouldDropItem(systemState, lastEvent))
            {
                if (IsPriorityDroppedItem(systemState, lastEvent.IsPriority))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return null;
        }



        #endregion

    }
}
