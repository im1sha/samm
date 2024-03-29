﻿using System;
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

            Console.WriteLine($"{nameof(lambda)} = {lambda}\n" +
                $"{nameof(mu)}     = {mu}\n" +
                $"{nameof(p)}      = {p}\n\n");

            var model = new AppModel(lambda, mu, p, timeApprox);

            model.Run();

            var res = model.StatesProbabilities.OrderBy(i => i.Key.Queue).ThenBy(i => i.Key.Channel);
            foreach (var item in res)
            {
                Console.WriteLine(
                    $"state {((item.Key.Queue == null) ? 0.ToString() : (item.Key.Queue == true ? 2.ToString() : 1.ToString()))}" +
                    $"{((item.Key.Channel == null) ? 0.ToString() : (item.Key.Channel == true ? 2.ToString() : 1.ToString()))}: " +
                    // $"%  {((expected[item.Key] - item.Value) / item.Value)*100 }  \n"
                    $" { item.Value }  \n"
                );
            }

            Console.WriteLine("Q1 (priority items): " + model.RelativeProbabilityOfPriorityItems);
            Console.WriteLine("Q2 (usual items)   : " + model.RelativeProbabilityOfUsualItems);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}


