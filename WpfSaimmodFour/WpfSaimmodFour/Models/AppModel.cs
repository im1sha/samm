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

        #region calculated data

        public Dictionary<(bool? Queue, bool? Channel), double> StatesProbabilities { get; private set; }

        public double RelativeProbabilityOfPriorityItems { get; private set; }
        public double RelativeProbabilityOfUsualItems { get; private set; }

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

        public void Run()
        {
            Dictionary<(bool? Queue, bool? Channel), double> statesTimeCounter
                = new Dictionary<(bool? Queue, bool? Channel), double>();

            // generatorData[i].Time & channelTimeWhenActiveList store moments of time (NOT deltas)
            var (generatorDataList, channelTimeWhenActiveList, maxTimeBound) = GetReady(
                GeneratorIntensivity, ChannelIntesivity,
                HighPriorityItemPorbability, TimeApproximation);

            #region debug
#if DEBUG
            StringBuilder deb_output = new StringBuilder(string.Empty);
            deb_output.Append($"priority = {generatorDataList.Count(i => i.IsPriorityItem) / (double)generatorDataList.Count()} " +
                $"usual = {generatorDataList.Count(i => !i.IsPriorityItem) / (double)generatorDataList.Count()}" +
                $"\n");
#endif

            #endregion

            (double Total, double ActiveChannel) time = (0.0, 0.0);
            (int Priority, int Usual) dropItemCount = (0, 0);

            // null is free
            // true is high priority item
            // false is low priority item
            (bool? Queue, bool? Channel) systemState = (null, null);
            (bool IsGeneratorEvent, bool IsPriority) currentEvent;

            // it should count time for channel relatively to its active state
            (int Index, double Time) storedChannelWhenActiveEvent = (-1, 0);
            (int Index, double Time, bool IsPriority) storedGeneratorEvent = (-1, 0, false);

            (double Total, double Generator, double? Channel) nextTime;
            (int Generator, int Channel) nextIndex;

#if DEBUG
            int deb_generated = 0;
#endif
            // handles empty generatorDataList
            while (time.Total < maxTimeBound)
            {
                nextIndex.Generator = storedGeneratorEvent.Index + 1;
                nextIndex.Channel = storedChannelWhenActiveEvent.Index + 1;

                nextTime.Generator = generatorDataList.ElementAt(nextIndex.Generator).Time;
                if (channelTimeWhenActiveList == null || !channelTimeWhenActiveList.Any())
                {
                    nextTime.Channel = null;
                }
                else
                {
                    nextTime.Channel = (channelTimeWhenActiveList.ElementAt(nextIndex.Channel) - time.ActiveChannel)
                        + time.Total;
                }

                // store event
                // 
                // ignore next case: nextTime.Channel == nextTime.Generator
                // due to its porbability -> 0               
                if ((systemState.Channel != null) && (nextTime.Channel < nextTime.Generator))
                {
                    storedChannelWhenActiveEvent = (
                        Index: nextIndex.Channel,
                        Time: channelTimeWhenActiveList.ElementAt(nextIndex.Channel));
                    currentEvent = (
                        IsGeneratorEvent: false,
                        IsPriority: false);
                    nextTime.Total = (double)nextTime.Channel;
                }
                else
                {
                    storedGeneratorEvent = (
                        Index: nextIndex.Generator,
                        Time: generatorDataList.ElementAt(nextIndex.Generator).Time,
                        IsPriority: generatorDataList.ElementAt(nextIndex.Generator).IsPriorityItem);
                    currentEvent = (
                        IsGeneratorEvent: true,
                        IsPriority: generatorDataList.ElementAt(nextIndex.Generator).IsPriorityItem);
                    nextTime.Total = nextTime.Generator;
                }

                #region states time counter

                if (!statesTimeCounter.ContainsKey(systemState))
                {
                    statesTimeCounter.Add(systemState, 0.0);
                }
                if (nextTime.Total < maxTimeBound)
                {
                    statesTimeCounter[systemState] += (nextTime.Total - time.Total);
                }
                else
                {
                    statesTimeCounter[systemState] += (maxTimeBound - time.Total);
                }

                #endregion

                #region debug information colletion
                //#if DEBUG
                //                deb_output.Append(
                //                    $"{nameof(time.Total)}={time.Total.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                //                    $"{nameof(time.ActiveChannel)}={time.ActiveChannel.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                //                    $"{nameof(nextTime.Generator)}={nextTime.Generator.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                //                    $"{nameof(nextTime.Channel)}={nextTime.Channel.ToString().PadRight(20, '0').Substring(0, 9)}  " +
                //                    $"{nameof(channelTimeWhenActiveList)}[]={channelTimeWhenActiveList.ElementAt(nextIndex.Channel).ToString().PadRight(20, '0').Substring(0, 9)}  " +
                //                    $"{((systemState.Queue == null) ? 0.ToString() : (systemState.Queue == true ? 2.ToString() : 1.ToString()))}" +
                //                    $"{((systemState.Channel == null) ? 0.ToString() : (systemState.Channel == true ? 2.ToString() : 1.ToString()))}: " +
                //                    $"{(currentEvent.IsGeneratorEvent ? "L" : "M")}" +
                //                    $"{((!currentEvent.IsGeneratorEvent) ? "" : (currentEvent.IsPriority ? "(P)" : "(1-P)"))}  " +
                //                    $"\n"
                //                );
                //#endif
                #endregion

                if (systemState.Channel != null)
                {
                    time.ActiveChannel += (nextTime.Total - time.Total);
                }
                // go to event moment
                time.Total = nextTime.Total;

#if DEBUG
                if (currentEvent.IsGeneratorEvent)
                {
                    deb_generated++;
                }
#endif
                // drop item if it should 
                switch (ShouldDropPriorityItem(systemState, currentEvent))
                {
                    case true:
                        dropItemCount.Priority++;
                        break;
                    case false:
                        dropItemCount.Usual++;
                        break;
                }

                // go to new state
                systemState = ChangeState(systemState, currentEvent);
            }

            # region states probabilities
            StatesProbabilities.Clear();
            foreach (var item in statesTimeCounter)
            {
                StatesProbabilities.Add(item.Key, item.Value / statesTimeCounter.Sum(i => i.Value));
            }
            if (!StatesProbabilities.Any())
            {
                StatesProbabilities.Add((null, null), 1);
            }
            #endregion

            #region debug output
#if DEBUG
            var deb_fileName = DateTime.Now.ToBinary().ToString();
            deb_output.Append($"\nTOTAL GENERATED: {deb_generated}");
            deb_output.Append($"\nTIME: {maxTimeBound}");
            deb_output.Append($"\nTIME SUM: {statesTimeCounter.Sum(i => i.Value)}");
            File.WriteAllText(deb_fileName, deb_output.ToString());
#endif
            #endregion

            int totalEmitted = storedGeneratorEvent.Index + 1;

            RelativeProbabilityOfPriorityItems = 1 - ((dropItemCount.Priority + CountItemsInsideSystem(true, systemState))
                / (double)generatorDataList.Take(storedGeneratorEvent.Index + 1).Count(i => i.IsPriorityItem));

            RelativeProbabilityOfUsualItems = 1 - ((dropItemCount.Usual + CountItemsInsideSystem(false, systemState))
                / (double)generatorDataList.Take(storedGeneratorEvent.Index + 1).Count(i => !i.IsPriorityItem)); 
        }

        #endregion

        #region private methods

        private static int CountItemsInsideSystem(bool isPriority, (bool? Queue, bool? Channel) state) 
        {
            var result = 0;
            if (isPriority && (state.Queue == true))
            {
                result++;
            }
            if (isPriority && (state.Channel == true))
            {
                result++;
            }
            if (!isPriority && (state.Queue == false))
            {
                result++;
            }
            if (!isPriority && (state.Queue == false))
            {
                result++;
            }
            return result;
        }

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
            IEnumerable<double> generatorAccumulatedDistribution = ExponentialGeneratorWrapper
                .AccumulateDistribution(generatorForSource.GenerateDistribution());
            IEnumerable<double> channelAccumulatedDistribution = ExponentialGeneratorWrapper
                .AccumulateDistribution(generatorForChannel.GenerateDistribution());

            var boolRandom = new Random();

            double time;
            if (!generatorAccumulatedDistribution.Any())
            {
                time = 0;
            }
            else if (generatorAccumulatedDistribution.Any() && !channelAccumulatedDistribution.Any())
            {
                time = generatorAccumulatedDistribution.Last();
            }
            else
            {
                time = Math.Min(generatorAccumulatedDistribution.Last(), channelAccumulatedDistribution.Last());
            }
            
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
                return systemState.Channel != null
                    && systemState.Queue != null
                    && lastEvent.IsGeneratorEvent;              
            }

            // use if ShouldDropItem returns true =>
            // systemStates in {(true, true), (false, false), (false, true)}
            static bool IsPriorityDroppedItem(
                (bool? Queue, bool? Channel) systemState,
                bool IsPriorityItem)
            {
                return systemState.Queue == true
                    && systemState.Channel == true
                    && IsPriorityItem;
            }

            #endregion

            if (ShouldDropItem(systemState, lastEvent))
            {
                return IsPriorityDroppedItem(systemState, lastEvent.IsPriority);
            }

            return null;
        }
        #endregion

    }
}
