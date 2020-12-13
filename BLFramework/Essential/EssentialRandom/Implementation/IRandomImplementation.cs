namespace Basement.BLFramework.Essential.EssentialRandom.Implementation
{
    public interface IRandomImplementation
    {
        double NextDouble(ref int seed, bool incSeed);
        int NextInt(ref int seed, int exclusiveMax, bool incSeed);
        int Range(int inclusiveMin, int exclusiveMax, ref int seed, bool incSeed);
    }
}