namespace WpfSaimmodOne
{
    internal static class PrimeTests
    {
        public static bool IsPrime(uint n)
        {
            if (n < 2)
            {
                return false;
            }

            if (n == 2 || n == 3 || n == 5 || n == 7)
            {
                return true;
            }

            if (n % 2 == 0)
            {
                return false;
            }

            var n1 = n - 1;
            var r = 1;
            var d = n1;
            while (d % 2 == 0)
            {
                r++;
                d >>= 1;
            }
            if (!Witness(2, r, d, n, n1))
            {
                return false;
            }

            if (n < 2047)
            {
                return true;
            }

            return Witness(7, r, d, n, n1)
                   && Witness(61, r, d, n, n1);
        }

        // a single instance of the Miller-Rabin Witness loop, optimized for odd numbers < 2e32
        private static bool Witness(int a, int r, uint d, uint n, uint n1)
        {
            var x = ModPow((ulong)a, d, n);
            if (x == 1 || x == n1)
            {
                return true;
            }

            while (r > 1)
            {
                x = ModPow(x, 2, n);
                if (x == 1)
                {
                    return false;
                }

                if (x == n1)
                {
                    return true;
                }

                r--;
            }
            return false;
        }

        private static uint ModPow(ulong value, uint exponent, uint modulus)
        {
            //value %= modulus; // unnecessary here because we know this is true every time already
            ulong result = 1;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                {
                    result = result * value % modulus;
                }

                value = value * value % modulus;
                exponent >>= 1;
            }
            return (uint)result;
        }
    }

}
