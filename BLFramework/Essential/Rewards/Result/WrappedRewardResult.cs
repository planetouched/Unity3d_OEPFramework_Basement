using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards.Result
{
    public class WrappedRewardResult : RewardResult
    {
        public IRewardResult rewardResult { get; private set; }

        public WrappedRewardResult(string type, IRewardResult rewardResult, IContext context = null)
            : base(type, context)
        {
            this.rewardResult = rewardResult;
        }

        public WrappedRewardResult(RawNode node, IContext context) : base(node, context)
        {
            rewardResult = FactoryManager.Build<RewardResult>(node.GetNode("result"), context);
        }

        public override object Serialize()
        {
            return SerializeUtil.Dict().SetArgs("type", type, "result", rewardResult.Serialize());
        }
    }
}
