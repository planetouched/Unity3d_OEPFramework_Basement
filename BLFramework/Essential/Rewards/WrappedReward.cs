using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public abstract class WrappedReward : Reward
    {
        public IReward innerReward { get; protected set; }

        protected WrappedReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            innerReward = FactoryManager.Build<Reward>(rawNode.GetNode("reward"), context);
        }
    }
}
