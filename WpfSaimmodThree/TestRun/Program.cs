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

            AppModel model = new AppModel(0.5, 0.5);

            model.Run();
            var states = model.SystemStates;
            states = states.OrderBy(i => i.AsInt).ToArray();
            var len = states.Count();
            Console.WriteLine(model.TotalTacts);


            //2000     1E-06            //0
            //1000     1E-06            //0   
            //2010  0.041723            //0.039823180928429758494
            //1010  0.051954            //0.049778976160537198118
            //2110  0.054641            //0.052267924968564058024
            //1110  0.055294            //0.052890162170570773
            //2210  0.092315            //0.053045721471072451744
            //1210  0.064697            //0.05308461129619787143
            //1001  0.083919            //0.079646361856859516988
            //2011  0.125441            //0.11946954278528927548
            //1011  0.114253            //0.10951374755318183586
            //2111  0.111473            //0.10702479874515497595
            //1111  0.111240            //0.10640256154314826098
            //2211  0.074406            //0.10624700224264658223
            //1211  0.018642            //0.070805408278347441698

            var counted = 0;
            foreach (var k in dict.Keys)
            {
                var newCount = states.Count(i => k == i.AsInt);
                counted += newCount;
                Console.WriteLine($"{k}  {newCount / (double)len}");
            }

            Console.WriteLine($"counted {counted}");
        }

        static void CheckErrors(IEnumerable<State> states, Dictionary<int, int[]> dict) {
          
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
