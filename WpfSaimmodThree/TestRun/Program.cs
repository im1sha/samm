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
            var dict = new Dictionary<int, int[]>();

            dict.Add(2000, new[] { 1000 });
            dict.Add(1000, new[] { 2010 });
            dict.Add(2010, new[] { 1010, 1001 });
            dict.Add(1010, new[] { 2110, 2011 });
            dict.Add(2110, new[] { 1110, 1011 });
            dict.Add(1110, new[] { 2210, 2111 });
            dict.Add(2210, new[] { 1210, 1111 });
            dict.Add(1210, new[] { 2210, 2211 });
            dict.Add(1001, new[] { 2010, 2011 });
            dict.Add(2011, new[] { 1010, 1001, 1011});
            dict.Add(1011, new[] { 2011, 2110, 2111});
            dict.Add(2111, new[] { 1011, 1110, 1111});
            dict.Add(1111, new[] { 2111, 2210, 2211});
            dict.Add(2211, new[] { 1111, 1211, 1210});
            dict.Add(1211, new[] { 2210, 2211});


            AppModel model = new AppModel(0.5, 0.5);

            model.Run();


            var states = model.SystemStates;
            var errors = 0;
            var processed = 0;
            var stringErrors = new List<string>();

            for (int i = 1; i < 100000; i++)
            {
                if (dict.TryGetValue(GetIntItem(states[i-1]), out int[] values)) 
                {
                    processed++;
                    if (!values.Contains(GetIntItem(states[i]))) 
                    {
                        errors++;                      
                        var errorStr = $"{GetStringItem(states[i-1])} {GetStringItem(states[i])}";

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
        static string GetStringItem(State state) 
        {
            return $"{state.TactsToNewItem}{state.CurrentQueueLength}" +
                $"{Convert.ToInt32(state.ChannelIsBusy1)}" +
                $"{Convert.ToInt32(state.ChannelIsBusy2)}";
        }

        static int GetIntItem(State state)
        {
            return int.Parse(GetStringItem(state));
        }

    }
}
