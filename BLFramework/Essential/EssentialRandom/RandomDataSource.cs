using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Essential.EssentialRandom
{
    public class RandomDataSource : DataSourceBase<RandomDescription>
    {
        public RandomDataSource(RawNode node, IContext context = null) : base(node, context)
        {
        }

        protected override RandomDescription Factory(RawNode partialNode)
        {
            return new RandomDescription(partialNode);
        }
    }
}
