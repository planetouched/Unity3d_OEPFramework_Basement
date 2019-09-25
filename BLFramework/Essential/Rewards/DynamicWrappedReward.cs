using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public abstract class DynamicWrappedReward : DynamicReward
    {
        public IDynamicReward innerReward { get; }
        
        protected DynamicWrappedReward(RawNode node, IContext context) : base(node, context)
        {
            innerReward = FactoryManager.Build<DynamicReward>(node.GetNode("reward"), context);
        }
    }
}