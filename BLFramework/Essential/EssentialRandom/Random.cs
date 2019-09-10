using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Essential.EssentialRandom.Implementation;
using Basement.Common;

namespace Basement.BLFramework.Essential.EssentialRandom
{
    public class Random : ReferenceModelBase<RandomCategories, RandomDescription>, IRandom
    {
        private int _seed;
        private readonly IRandomImplementation _random;

        public Random(RawNode initNode, RandomCategories categories, RandomDescription description, IContext context) : base(initNode, categories, description, context)
        {
            _seed = initNode.GetInt("seed");
            _random = (IRandomImplementation)FactoryManager.Build(typeof(IRandomImplementation), description.type);
        }

        public int GetSeed()
        {
            return _seed;
        }

        public double NextDouble(bool incSeed = true)
        {
            return _random.NextDouble(ref _seed, incSeed);
        }

        public int NextInt(int exclusiveMax = int.MaxValue, bool incSeed = true)
        {
            return _random.NextInt(ref _seed, exclusiveMax, incSeed);
        }

        public int Range(int inclusiveMin, int exclusiveMax, bool incSeed = true)
        {
            return _random.Range(inclusiveMin, exclusiveMax, ref _seed, incSeed);
        }

        public override object Serialize()
        {
            return new Dictionary<string, object> { { "seed", _seed } };
        }
    }
}
