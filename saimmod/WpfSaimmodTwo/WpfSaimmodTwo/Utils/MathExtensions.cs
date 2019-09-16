using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSaimmodTwo.Utils
{
    public static class MathUtils
    {
        public static int Factorial(int n)
        {
            int res = 1;
            while (n > 1)
            {
                res *= n;
                n -= 1;
            }
            return res;
        }
    }
}
