namespace Basement.BLFramework.Essential.EssentialRandom.Implementation
{
    public class FastRandom : IRandomImplementation
    {
        const double RealUnitInt = 1.0 / (int.MaxValue + 1.0);
        const int W = 273326509;

        public double NextDouble(ref int seed, bool incSeed)
        {
            int x = ((seed * 1431655781) & 0x7fffffff)
                     + ((seed * 1183186591) & 0x7fffffff)
                     + ((seed * 622729787) & 0x7fffffff)
                     + ((seed * 338294347) & 0x7fffffff) & 0x7fffffff;

            if (incSeed)
                seed++;

            int t = (x ^ (x << 11)) & 0x7fffffff;
            return RealUnitInt * (0x7FFFFFFF & (W ^ (W >> 19) ^ t ^ (t >> 8)));
        }

        public int NextInt(ref int seed, int exclusiveMax, bool incSeed)
        {
            return (int)(NextDouble(ref seed, incSeed) * exclusiveMax);
        }

        public int Range(int inclusiveMin, int exclusiveMax, ref int seed, bool incSeed)
        {
            var result = NextDouble(ref seed, incSeed);
            return (int)((exclusiveMax - inclusiveMin) * result + inclusiveMin);
        }
    }
}
