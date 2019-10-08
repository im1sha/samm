using System;
using System.Linq;

namespace WpfSaimmodThree.Models
{
    public class State 
    {
        public bool ChannelIsBusy1 { get; }
        public bool ChannelIsBusy2 { get; }

        // {0, 1, 2}
        public int CurrentQueueLength { get; }
        private static readonly int[] POSSIBLE_QUEUE_LENGTH = new[] { 0, 1, 2 };

        // {1, 2}
        public int TactsToNewItem { get; }
        private static readonly int[] POSSIBLE_TACTS = new[] { 2, 1 };

        public State(int tactsToNewItem, int currentQueueLength, bool channelIsBusy1, bool channelIsBusy2)
        {
            if (!POSSIBLE_QUEUE_LENGTH.Contains(currentQueueLength))
            {
                throw new ArgumentException(nameof(currentQueueLength));
            }
            if (!POSSIBLE_TACTS.Contains(tactsToNewItem))
            {
                throw new ArgumentException(nameof(tactsToNewItem));
            }

            ChannelIsBusy1 = channelIsBusy1;
            ChannelIsBusy2 = channelIsBusy2;
            CurrentQueueLength = currentQueueLength;
            TactsToNewItem = tactsToNewItem;
        }

        public State((int TactsToNewItem, int CurrentQueueLength, bool ChannelIsBusy1, bool ChannelIsBusy2) state) 
            : this(state.TactsToNewItem, state.CurrentQueueLength, state.ChannelIsBusy1, state.ChannelIsBusy2)
        {             
        }

        public State(int TactsToNewItem, int CurrentQueueLength, int ChannelIsBusy1, int ChannelIsBusy2)
            : this(TactsToNewItem, CurrentQueueLength, Convert.ToBoolean(ChannelIsBusy1), Convert.ToBoolean(ChannelIsBusy2))
        { 
        }

        public State(int state) 
            : this((int)Char.GetNumericValue(state.ToString().PadLeft(4, '0')[0]),
                  (int)Char.GetNumericValue(state.ToString().PadLeft(4, '0')[1]),
                  (int)Char.GetNumericValue(state.ToString().PadLeft(4, '0')[2]),
                  (int)Char.GetNumericValue(state.ToString().PadLeft(4, '0')[3]))
        { 
        }       
    }
}
