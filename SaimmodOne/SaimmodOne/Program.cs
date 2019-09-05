using System;

namespace SaimmodOne
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int mult, 
                init,
                divider;

            Console.WriteLine($"{nameof(mult)}");
            mult = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"{nameof(init)}");
            init = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"{nameof(divider)}");
            divider = Convert.ToInt32(Console.ReadLine());

            var alg = new Algorithm(mult, init, divider);
            var sq = alg.GetSequence();
            var count = 0;
            Console.WriteLine("\nVALUES\n");
            foreach (int item in sq)
            {
                count++;
                Console.WriteLine(item);
                if (count == 20)
                {
                    break;
                }
            }
        }
    }
}
