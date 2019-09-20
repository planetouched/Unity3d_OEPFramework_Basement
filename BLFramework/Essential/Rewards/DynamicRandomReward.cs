using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.EssentialRandom;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public class DynamicRandomReward : DynamicWrappedReward
    {
        public double probability { get; }
        private readonly Random _random;
        
        public DynamicRandomReward(RawNode node, IContext context) : base(node, context)
        {
            if (node.CheckKey("random"))
            {
                _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
            }

            probability = node.GetDouble("probability");
        }
        
        public override IRewardResult Calculate(int amount)
        {
            var check = _random.NextDouble() <= probability;
            var result = check ? new WrappedRewardResult(type, innerReward.Calculate(amount)) : new RewardResult();
            return result;
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            innerReward.Award(((WrappedRewardResult)rewardResult).rewardResult);
            return rewardResult;
        }
    }
}