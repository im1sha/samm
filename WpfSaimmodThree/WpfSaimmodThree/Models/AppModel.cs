using System;
using System.Collections.Generic;
using System.Linq;

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

        public int TotalGenerated { get; private set; }
        public int TotalProcessed { get; private set; }
        public int TotalFailures { get; private set; }

        public List<State> SystemStates { get; } = new List<State>();

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

        private readonly Random _random1 = new Random(123456);
        private readonly Random _random2 = new Random(42);

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
            return TotalFailures / (double)TotalGenerated;
        }

        #endregion

        public void Run()
        {
            int GetNextTacts(int currentTacts)
            {
                if (currentTacts == 2)
                {
                    return 1;
                }
                else if (currentTacts == 1)
                {
                    return 2;
                }
                else
                {
                    throw new ArgumentException(nameof(currentTacts));
                }
            }

            //currentQueueLength={0, 1, 2}
            //currentTacts={1,2}
            (int Length, bool Failed) GetNextQueueLength(
                bool generatedIsBusyChannel1, 
                bool previousIsBusyChannel1,
                int currentQueueLength,
                int currentTacts,
                int maxQueueLength = 2)
            {
                if (maxQueueLength < currentQueueLength)
                {
                    throw new ArgumentException();
                }

                throw new Exception();                
            }

            //
            bool channelIsBusy1 = false;
            bool channelIsBusy2 = false;
            int currentQueueLength = 0;
            int tactsToNewItem = 2;
            //

            for (int i = 0; i < TotalTacts; i++)
            {
                SystemStates.Add(new State(
                    tactsToNewItem, 
                    currentQueueLength, 
                    channelIsBusy1, 
                    channelIsBusy2));

                bool isFailed = false;

                #region unused
                //currentState = new State(channelIsFree1, channelIsFree2, 
                //    currentQueueLength, tactsToNewItem);
                //_systemStates.Add(currentState);
                //++TotalGenerated;
                #endregion

                // save current channels state
                (bool, bool) savedChannelIsBusy = (channelIsBusy1, channelIsBusy2);

                (bool, bool) generatedIsBusy = (GetChannelIsBusy1(), GetChannelIsBusy2());

                // update channelIsBusy1, channelIsBusy2
                switch ((tactsToNewItem, currentQueueLength, channelIsBusy1, channelIsBusy2))
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
                            if (generatedIsBusy.Item1)
                            {
                                // no channels changed
                                // -> (x, x, true, false)
                            }
                            else
                            {
                                channelIsBusy2 = true;
                                if (currentQueueLength == 0 && tactsToNewItem == 2)
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
                            if (generatedIsBusy.Item1)
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
                            if (generatedIsBusy.Item2)
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
                            if (!generatedIsBusy.Item1)
                            {
                                channelIsBusy1 = false;
                                // -> (x, x, false, true)
                            }
                            else if (generatedIsBusy.Item1 && generatedIsBusy.Item2)
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
                            if ((!generatedIsBusy.Item1)
                                || (generatedIsBusy.Item1 && generatedIsBusy.Item2))
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
                            if ((generatedIsBusy.Item1 && generatedIsBusy.Item2)
                                || (!generatedIsBusy.Item1))
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
                            if (generatedIsBusy.Item1 && !generatedIsBusy.Item2)
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
                        break;
                }

                // update currentQueueLength
                (currentQueueLength, isFailed) = GetNextQueueLength(
                    generatedIsBusy.Item1, savedChannelIsBusy.Item1, 
                    currentQueueLength, tactsToNewItem);

                //if (isFailed)
                //{
                //    ++TotalFailures;
                //}

                // update tactsToNewItem
                tactsToNewItem = GetNextTacts(tactsToNewItem);
            }
        }
    }
}
