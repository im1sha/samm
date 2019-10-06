using System;

namespace WpfSaimmodTwo.Utils
{
    /// <summary>
    /// Find the length and start of a cycle in a series of objects of any IEquatable type using Brent's cycle algorithm.
    /// </summary>
    public class CycleDetector<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Find the cycle length and start position of a series using Brent's cycle algorithm.
        /// 
        ///  Given a recurrence relation X[n+1] = f(X[n]) where f() has
        ///  a finite range, you will eventually repeat a value that you have seen before.
        ///  Once this happens, all subsequent values will form a cycle that begins
        ///  with the first repeated value. The period of that cycle may be of any length.
        /// </summary>
        /// <returns>A tuple where:
        ///    Item1 is lambda (the length of the cycle) 
        ///    Item2 is mu, the zero-based index of the item that started the first cycle.</returns>
        /// <param name="yielder">Function delegate that generates the series by iterated execution.</param>
        public static (int length, int start) FindCycle(T multiplier, T initialValue, T divider, Func<T, T, T, T> yielder)
        {
            int power, lambda;
            T tortoise, hare;
            power = lambda = 1;
            tortoise = initialValue;
            hare = yielder(multiplier, initialValue, divider);

            // Find lambda, the cycle length
            while (!tortoise.Equals(hare))
            {
                if (power == lambda)
                {
                    tortoise = hare;
                    power *= 2;
                    lambda = 0;
                }
                hare = yielder(multiplier, hare, divider);
                lambda += 1;
            }

            // Find mu, the zero-based index of the start of the cycle
            var mu = 0;
            tortoise = hare = initialValue;
            for (var times = 0; times < lambda; times++)
            {
                hare = yielder(multiplier, hare, divider);
            }

            while (!tortoise.Equals(hare))
            {
                tortoise = yielder(multiplier, tortoise, divider);
                hare = yielder(multiplier, hare, divider);
                mu += 1;
            }

            return (lambda, mu);
        }
    }
}
