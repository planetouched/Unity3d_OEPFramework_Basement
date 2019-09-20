using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Collection;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public class DynamicCompositeReward : DynamicReward
    {
        public LazyArray<DynamicReward> rewards { get; }
        
        public DynamicCompositeReward(RawNode node, IContext context) : base(node, context)
        {
            rewards = new LazyArray<DynamicReward>(node.GetNode("rewards"), context);
        }
        
        public  override IRewardResult Calculate(int amount)
        {
            var rewardResults = new IRewardResult[rewards.Count()];

            for (int i = 0; i < rewards.Count(); i++)
            {
                rewardResults[i] = rewards[i].Calculate(amount);
            }

            return new CompositeRewardResult(type, rewardResults);
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            var results = ((CompositeRewardResult)rewardResult).results;

            for (int i = 0; i < results.Length; i++)
            {
                rewards[i].Award(results[i]);
            }

            return rewardResult;
        }
    }
}