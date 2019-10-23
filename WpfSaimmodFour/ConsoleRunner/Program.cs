using System;
using System.Linq;
using WpfSaimmodFour.Models;

namespace ConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const double lambda = 0.45;
            const double mu = 0.5;
            const double p = 0.4;
            const double timeApprox = 1_000_000;

            var model = new AppModel(lambda, mu, p, timeApprox);

            Console.WriteLine(model.Run());

            foreach (var item in model.StatesProbabilities)
            {
                Console.WriteLine(item.Key+ ": " +item.Value);
                Console.WriteLine();
            }

        }
    }
}
