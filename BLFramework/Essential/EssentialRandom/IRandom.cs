
namespace Basement.BLFramework.Essential.EssentialRandom
{
    public interface IRandom
    {
        int GetSeed();
        double NextDouble(bool incSeed = true);
        int NextInt(int exclusiveMax = int.MaxValue, bool incSeed = true);
        int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true);
    }
}