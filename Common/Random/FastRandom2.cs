namespace Basement.Common.Random
{
    public class FastRandom2
    {
        const double RealUnitInt = 1.0 / (int.MaxValue + 1.0);
        const int W = 273326509;

        public int seed { get; private set; }

        public FastRandom2(int seed)
        {
            this.seed = seed;
        }

        public double NextDouble(bool incSeed = true)
        {
            int x = ((seed * 1431655781) & 0x7fffffff)
                     + ((seed * 1183186591) & 0x7fffffff)
                     + ((seed * 622729787) & 0x7fffffff)
                     + ((seed * 338294347) & 0x7fffffff) & 0x7fffffff;

            if (incSeed)
                seed++;

            int t = (x ^ (x << 11)) & 0x7fffffff;
            return RealUnitInt * (0x7FFFFFFF & ((W ^ (W >> 19)) ^ (t ^ (t >> 8))));
        }

        public int NextInt(int exclusiveMax = int.MaxValue, bool incSeed = true)
        {
            return (int)(NextDouble(incSeed) * exclusiveMax);
        }

        public int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true)
        {
            var result = NextDouble(incSeed);
            return (int)((exclusiveMax - inclusiveMin) * result + inclusiveMin);
        }
    }
}
