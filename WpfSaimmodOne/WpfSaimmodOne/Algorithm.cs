using System.Collections.Generic;

namespace WpfSaimmodOne
{
    internal class Algorithm
    {
        private readonly int _multiplier;
        private readonly int _inintialValue;
        private readonly int _divider;
        private readonly IEnumerable<int> _seq;

        /// <summary>
        /// R[n+1] = (R[n] *a) %m. m > a
        /// </summary>
        /// <param name="multiplier">a</param>
        /// <param name="initialValue">R[0]</param>
        /// <param name="divider">m</param>
        public Algorithm(int multiplier, int initialValue, int divider)
        {
            _multiplier = multiplier;
            _inintialValue = initialValue;
            _divider = divider;
            _seq = CreateSequence(multiplier, initialValue, divider);
        }

        public int Divider => _divider;

        // Gets sequnce of generated members 
        private IEnumerable<int> CreateSequence(int multiplier, int initialValue, int divider)
        {
            int currMember = initialValue;
            int multiplication;

            while (true)
            {           
                // a * R[n] 
                multiplication = multiplier * currMember;
                currMember = multiplication % divider;
                yield return currMember;
            }
        }

        public IEnumerable<int> GetSequence()
        {
            foreach (var item in _seq)
            {
                yield return item;
            }
        }
    }
}
