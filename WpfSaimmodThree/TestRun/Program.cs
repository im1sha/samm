using System;
using System.Collections.Generic;
using System.Linq;
using WpfSaimmodThree.Models;

namespace TestRun
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dict = new Dictionary<int, int[]>
            {
                { 2000, new[] { 1000 } },
                { 1000, new[] { 2010 } },
                { 2010, new[] { 1010, 1001 } },
                { 1010, new[] { 2110, 2011 } },
                { 2110, new[] { 1110, 1011 } },
                { 1110, new[] { 2210, 2111 } },
                { 2210, new[] { 1210, 1111 } },
                { 1210, new[] { 2210, 2211 } },
                { 1001, new[] { 2010, 2011 } },
                { 2011, new[] { 1010, 1001, 1011 } },
                { 1011, new[] { 2011, 2110, 2111 } },
                { 2111, new[] { 1011, 1110, 1111 } },
                { 1111, new[] { 2111, 2210, 2211 } },
                { 2211, new[] { 1111, 1211, 1210 } },
                { 1211, new[] { 2210, 2211 } }
            };

            AppModel model = new AppModel(0.2, 0.9);

            model.Run();
            Console.WriteLine("run() finished");
            var states = model.SystemStates;
            states = states.OrderBy(i => i.AsInt).ToArray();
            var len = states.Count();
            Console.WriteLine(model.TotalTacts);

            //expected probabilities from analytical model
            //0
            //0
            //0.041666666666666666667
            //0.052083333333333333333
            //0.0546875
            //0.055338541666666666667
            //0.092534722222222222222
            //0.064800347222222222222
            //0.083333333333333333333
            //0.125
            //0.11458333333333333333
            //0.11197916666666666667
            //0.111328125
            //0.074131944444444444444
            //0.018532986111111111111

            var counted = 0;
            foreach (var k in dict.Keys)
            {
                var newCount = states.Count(i => k == i.AsInt);
                counted += newCount;
                Console.WriteLine($"{k}  {newCount / (double)len}");
            }

            Console.WriteLine($"counted {counted}");

            Console.WriteLine($"A {model.GetBandwidth()}");
            Console.WriteLine($"Lqueue {model.GetAverageQueueLength()}");
            Console.WriteLine($"Drop in Channel {model.DroppedInChannel / (double)(model.TotalProcessed + model.GetTotalDropped())}");
            Console.WriteLine($"Drop in Queue {model.DroppedDueToQueueOverflow / (double) (model.TotalProcessed + model.GetTotalDropped())}");
            Console.WriteLine($"Processed {model.TotalProcessed }");
            Console.WriteLine($"Pfail {model.GetFailureProbability()}");
        }

        private static void CheckErrorStates(IEnumerable<State> states, Dictionary<int, int[]> dict)
        {

            var errors = 0;
            var processed = 0;
            var stringErrors = new List<string>();

            for (int i = 1; i < 100000; i++)
            {
                if (dict.TryGetValue(states.ElementAt(i - 1).AsInt, out int[] values))
                {
                    processed++;
                    if (!values.Contains(states.ElementAt(i - 1).AsInt))
                    {
                        errors++;
                        var errorStr = $"{(states.ElementAt(i - 1)).AsInt} {(states.ElementAt(i - 1)).AsInt}";

                        if (!stringErrors.Contains(errorStr))
                        {
                            stringErrors.Add(errorStr);
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            Console.WriteLine("errors" + errors.ToString());
            Console.WriteLine("total" + processed.ToString());

            for (int i = 0; i < stringErrors.Count; i++)
            {
                Console.WriteLine(stringErrors[i]);
            }
        }

    }
}
