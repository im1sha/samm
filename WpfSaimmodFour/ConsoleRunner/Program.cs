using System;
using System.Collections.Generic;
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

            model.Run();

            var expected = new Dictionary<(bool? Queue, bool? Channel), double>
            {
                { (null, null), 0.369003690036900369 },
                { (null, false), 0.2240015942865621788 },
                { (null, true), 0.1081017267466481533 },
                { (false, false), 0.13290956214522520922 },
                { (false, true), 0.093431474379316731237 },
                { (true, true), 0.072551952405347358434 }
            };

            var res = model.StatesProbabilities.OrderBy(i => i.Key.Queue).ThenBy(i => i.Key.Channel);
            foreach (var item in res)
            {
                Console.WriteLine(
                    $"{((item.Key.Queue == null) ? 0.ToString() : (item.Key.Queue == true ? 2.ToString() : 1.ToString()))}" +
                    $"{((item.Key.Channel == null) ? 0.ToString() : (item.Key.Channel == true ? 2.ToString() : 1.ToString()))}: " +
                    $"%  {((expected[item.Key] - item.Value) / item.Value)*100 }  \n"
                );
            }
        }
    }
}


