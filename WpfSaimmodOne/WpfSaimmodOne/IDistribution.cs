using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodOne
{
    interface IDistribution
    {
        IEnumerable<uint> GetChartData(IEnumerable<uint> values);
        (double expectedValue, double variance, double standardDeviation) GetNormalizedStatistics(IEnumerable<uint> values);
    }
}
