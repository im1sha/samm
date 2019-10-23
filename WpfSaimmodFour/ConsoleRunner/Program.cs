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

            Console.WriteLine(new AppModel(lambda, mu, p, timeApprox).Run());
        }
    }
}
