using System;

// GetDigits and NumberOfDigits from:
// https://stackoverflow.com/questions/829174/is-there-an-easy-way-to-turn-an-int-into-an-array-of-ints-of-each-digit

namespace ship_battle.Data
{
    public static class RandArr
    {
        public static int[] GetArrSeed(int seed, int items)
        {
            int[] returnArr = new int[items];
            int[] digitArr = GetDigits(seed);
            for (int i = 0; i < digitArr.Length; i++)
            {
                returnArr[i] = digitArr[i];
            }
            return returnArr;
        }
        public static int[] GetArrAuto(int items)
        {
            Random autoRand = new();
            int[] returnArr = new int[items];
            int[] digitArr = GetDigits(autoRand.Next());
            for (int i = 0; i < digitArr.Length; i++)
            {
                returnArr[i] = digitArr[i];
            }
            return returnArr;
        }
        private static int[] GetDigits(int n)
        {
            if (n == 0)
                return new[] { 0 };

            var x = Math.Abs(n);

            var numDigits = NumberOfDigits(x);

            var res = new int[numDigits];
            var count = 0;

            while (x > 0)
            {
                res[count++] = x % 10;

                x /= 10;
            }

            Array.Reverse(res);

            return res;
        }

        private static int NumberOfDigits(int n)
        {
            if (n >= 0)
            {
                if (n < 10) return 1;
                if (n < 100) return 2;
                if (n < 1000) return 3;
                if (n < 10000) return 4;
                if (n < 100000) return 5;
                if (n < 1000000) return 6;
                if (n < 10000000) return 7;
                if (n < 100000000) return 8;
                if (n < 1000000000) return 9;
                return 10;
            }
            else
            {
                if (n > -10) return 2;
                if (n > -100) return 3;
                if (n > -1000) return 4;
                if (n > -10000) return 5;
                if (n > -100000) return 6;
                if (n > -1000000) return 7;
                if (n > -10000000) return 8;
                if (n > -100000000) return 9;
                if (n > -1000000000) return 10;
                return 11;
            }
        }

    }
}
