using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfSaimmodThree.Models
{
    public class AppModel
    {
        #region given data

        public double BusyProbability1 { get; }
        public double BusyProbability2 { get; }
        public int TotalTacts { get; }

        #endregion

        #region stored statistics

        public int TotalProcessed { get; private set; }

        public IList<State> SystemStates { get; } = new List<State>();

        public int DroppedDueToQueueOverflow { get; private set; }

        public int DroppedInChannel { get; private set; }

        public List<ProbabilityItem> StatesProbabilities { get; private set; }

        #endregion

        #region ctor
        public AppModel(double probability1, double probability2, int totalTacts = 1_000_000)
        {
            BusyProbability1 = probability1;
            BusyProbability2 = probability2;
            TotalTacts = totalTacts;
        }
        #endregion

        #region current channel state genetation

        private readonly Random _random1 = new Random();
        private readonly Random _random2 = new Random(78541137);

        private bool GetChannelIsBusy1()
        {
            return _random1.NextDouble() <= BusyProbability1;
        }

        private bool GetChannelIsBusy2()
        {
            return _random2.NextDouble() <= BusyProbability2;
        }

        #endregion

        #region calculated statistics

        // A in [0.0, 1.0]
        public double GetBandwidth()
        {
            return TotalProcessed / (double)TotalTacts;
        }


        // Lqueue in [0.0, 2.0]
        public double GetAverageQueueLength()
        {
            return SystemStates.Average(s => (double)s.CurrentQueueLength);
        }

        // Pfail
        public double GetFailureProbability()
        {
            return GetTotalDropped() / (double)(GetTotalDropped() + TotalProcessed);
        }

        public int GetTotalDropped()
        {
            return DroppedDueToQueueOverflow + DroppedInChannel;
        }

        #endregion

        public void Run()
        {
#if DEBUG
            StringBuilder deb_output = new StringBuilder(string.Empty);
#endif
            static int GetNextTacts(int previousTacts)
            {
                if (previousTacts == 2)
                {
                    return 1;
                }
                else if (previousTacts == 1)
                {
                    return 2;
                }
                else
                {
                    throw new ArgumentException(nameof(previousTacts));
                }
            }

            //currentQueueLength={0, 1, 2}
            //currentTacts={1,2}
            static (int Length, bool Dropped) GetNextQueueLength(
                bool generatedSignalChannelIsBusy1,
                bool previousChannelIsBusy1,
                int previousQueueLength,
                int previousTacts,
                int maxQueueLength = 2)
            {
                if (maxQueueLength < previousQueueLength)
                {
                    throw new ArgumentException();
                }
                int delta = 0;

                // channel has finished processing 
                // or
                // channel was empty
                if (!generatedSignalChannelIsBusy1 || !previousChannelIsBusy1)
                {
                    delta--;
                }
                if (previousTacts == 1)
                {
                    delta++;
                }

                int resultLength = previousQueueLength + delta;

                if (resultLength < 0)
                {
                    return (0, false);
                }
                if (resultLength > maxQueueLength)
                {
                    return (maxQueueLength, true);
                }
                return (resultLength, false);
            }

            static bool IsItemDroppedInChannel(
                bool previousChannelIsBusy1,
                bool previousChannelIsBusy2,
                bool generatedSignalChannelIsBusy1,
                bool generatedSignalChannelIsBusy2)
            {
                if (previousChannelIsBusy1 && previousChannelIsBusy2
                    && !generatedSignalChannelIsBusy1 && generatedSignalChannelIsBusy2)
                {
                    return true;
                }
                return false;
            }

            //
            bool channelIsBusy1 = false;
            bool channelIsBusy2 = false;
            int queueLength = 0;
            int tactsToNewItem = 2;
            //

            for (int i = 0; i < TotalTacts; i++)
            {
                SystemStates.Add(new State(
                    tactsToNewItem, queueLength,
                    channelIsBusy1, channelIsBusy2));

                // save current channels state
                (bool, bool) previousChannelIsBusy = (channelIsBusy1, channelIsBusy2);

                // generate random signals for state changing
                // true == channel is in process with previous item
                // false == channel processed item
                (bool, bool) generatedSignals = (GetChannelIsBusy1(), GetChannelIsBusy2());

                // update channelIsBusy1, channelIsBusy2
                switch ((tactsToNewItem, queueLength, channelIsBusy1, channelIsBusy2))
                {
                    case (2, 0, false, false):
                        // no channels changed
                        // -> (x, x, false, false)
                        break;
                    case (1, 0, false, false):
                        {
                            channelIsBusy1 = true;
                            // -> (x, x, true, false)
                            break;
                        }
                    case (2, 0, true, false):
                    case (1, 0, true, false):
                    case (2, 1, true, false):
                    case (1, 1, true, false):
                    case (2, 2, true, false):
                        {
                            // IsBusy: true == 1
                            if (generatedSignals.Item1)
                            {
                                // no channels changed
                                // -> (x, x, true, false)
                            }
                            else
                            {
                                channelIsBusy2 = true;
                                if (queueLength == 0 && tactsToNewItem == 2)
                                {
                                    channelIsBusy1 = false;
                                    // -> (x, x, false, true)
                                }
                                else
                                {
                                    channelIsBusy1 = true;
                                    // -> (x, x, true, true)
                                }
                            }
                            break;
                        }
                    case (1, 2, true, false):
                        {
                            if (generatedSignals.Item1)
                            {
                                // no channels changed
                                // -> (x, x, true, false)
                            }
                            else
                            {
                                channelIsBusy2 = true;
                                // -> (x, x, true, true)
                            }
                            break;
                        }
                    case (1, 0, false, true):
                        {
                            channelIsBusy1 = true;
                            if (generatedSignals.Item2)
                            {
                                channelIsBusy2 = true;
                                // -> (x, x, true, true)
                            }
                            else
                            {
                                channelIsBusy2 = false;
                                // -> (x, x, true, false)
                            }
                            break;
                        }
                    case (2, 0, true, true):
                        {
                            if (!generatedSignals.Item1)
                            {
                                channelIsBusy1 = false;
                                // -> (x, x, false, true)
                            }
                            else if (generatedSignals.Item1 && generatedSignals.Item2)
                            {
                                // no channels changed
                                // -> (x, x, true, true)
                            }
                            else
                            {
                                channelIsBusy2 = false;
                                // -> (x, x, true, false)
                            }
                            break;
                        }
                    case (1, 0, true, true):
                    case (2, 1, true, true):
                    case (1, 1, true, true):
                        {
                            if ((!generatedSignals.Item1)
                                || (generatedSignals.Item1 && generatedSignals.Item2))
                            {
                                // no channels changed
                                // -> (x, x, true, true)
                            }
                            else
                            {
                                channelIsBusy2 = false;
                                // -> (x, x, true, true)
                            }
                            break;
                        }
                    case (2, 2, true, true):
                        {
                            if ((generatedSignals.Item1 && generatedSignals.Item2)
                                || (!generatedSignals.Item1))
                            {
                                // no channels changed
                                // -> (x, x, true, true)
                            }
                            else
                            {
                                channelIsBusy2 = false;
                                // -> (x, x, true, false)
                            }
                            break;
                        }
                    case (1, 2, true, true):
                        {
                            if (generatedSignals.Item1 && !generatedSignals.Item2)
                            {
                                channelIsBusy2 = false;
                                // -> (x, x, true, false)
                            }
                            else
                            {
                                // no channels changed
                                // -> (x, x, true, true)
                            }
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }

                // save queue length & tactsToNewItem
                int previousQueueLength = queueLength;
                int previousTactsToNewItem = tactsToNewItem;

                #region drop counter update

                bool isDroppedNow;

                #region queue length update

                // update currentQueueLength
                // and
                // check whether new generated queue item 've been dropped due to queue overflow
                (queueLength, isDroppedNow) = GetNextQueueLength(
                    generatedSignals.Item1, previousChannelIsBusy.Item1,
                    previousQueueLength, previousTactsToNewItem);

                #endregion

                if (isDroppedNow)
                {
                    ++DroppedDueToQueueOverflow;
                }
                // check whether item in 1st channel 've been dropped due to 2nd channel is busy
                isDroppedNow = IsItemDroppedInChannel(previousChannelIsBusy.Item1,
                    previousChannelIsBusy.Item2, generatedSignals.Item1,
                    generatedSignals.Item2);
                if (isDroppedNow)
                {
                    ++DroppedInChannel;
                }

                #endregion

                #region processed counter update
                if (!generatedSignals.Item2 && previousChannelIsBusy.Item2)
                {
                    ++TotalProcessed;
                }
                #endregion

                // update tactsToNewItem
                tactsToNewItem = GetNextTacts(previousTactsToNewItem);
#if DEBUG
                deb_output.Append($"({previousTactsToNewItem}{previousQueueLength}{Convert.ToInt32(previousChannelIsBusy.Item1)}{Convert.ToInt32(previousChannelIsBusy.Item2)})" +
                    $"({tactsToNewItem}{queueLength}{Convert.ToInt32(channelIsBusy1)}{Convert.ToInt32(channelIsBusy2)})" +
                    $"<{Convert.ToInt32(generatedSignals.Item1)}{Convert.ToInt32(generatedSignals.Item2)}>\n");
#endif
            }

#if DEBUG
            File.WriteAllText(DateTime.Now.ToBinary().ToString(), deb_output.ToString());
#endif

            #region update states probabilites

            State[] orderedStates = SystemStates.OrderBy(i => i.AsInt).ToArray();
            int totalTacts = orderedStates.Length;
            int[] statesNames = orderedStates.Select(i => i.AsInt).Distinct().ToArray();

            var probabilitiesDictionary = new List<ProbabilityItem>();
            int stateCount;
            foreach (int stateName in statesNames)
            {
                stateCount = orderedStates.Count(i => stateName == i.AsInt);
                probabilitiesDictionary.Add(new ProbabilityItem(stateName.ToString().PadLeft(4, '0'), stateCount / (double)totalTacts));
            }

            StatesProbabilities = probabilitiesDictionary;

            #endregion
        }
    }
}
