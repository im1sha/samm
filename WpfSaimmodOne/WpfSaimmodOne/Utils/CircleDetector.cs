using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne.Utils
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
        public static (int clength, int cstart) FindCycle(T mult, T x0, T div, Func<T, T, T, T> yielder)
        {
            int power, lambda;
            T tortoise, hare;
            power = lambda = 1;
            tortoise = x0;
            hare = yielder(mult, x0, div);

            // Find lambda, the cycle length
            while (!tortoise.Equals(hare))
            {
                if (power == lambda)
                {
                    tortoise = hare;
                    power *= 2;
                    lambda = 0;
                }
                hare = yielder(mult, hare, div);
                lambda += 1;
            }

            // Find mu, the zero-based index of the start of the cycle
            var mu = 0;
            tortoise = hare = x0;
            for (var times = 0; times < lambda; times++)
            {
                hare = yielder(mult, hare, div);
            }

            while (!tortoise.Equals(hare))
            {
                tortoise = yielder(mult, tortoise, div);
                hare = yielder(mult, hare, div);
                mu += 1;
            }

            return (lambda, mu);
        }
    }
}
