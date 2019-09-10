using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Collection;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public class CompositeReward : Reward
    {
        public LazyArray<Reward> rewards { get; }

        public CompositeReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            rewards = new LazyArray<Reward>(rawNode.GetNode("rewards"), context);
        }

        public  override IRewardResult Calculate()
        {
            var rewardResults = new IRewardResult[rewards.Count()];

            for (int i = 0; i < rewards.Count(); i++)
            {
                rewardResults[i] = rewards[i].Calculate();
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
