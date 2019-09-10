using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.EssentialRandom;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public class RandomReward : WrappedReward
    {
        public double probability { get; }
        private readonly Random _random;

        public RandomReward(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            if (rawNode.CheckKey("random"))
            {
                _random = PathUtil.GetModelPath(GetContext(), rawNode.GetString("random"), null).GetSelf<Random>();
            }

            probability = rawNode.GetDouble("probability");
        }

        public override IRewardResult Calculate()
        {
            var check = _random.NextDouble() <= probability;
            var result = check ? new WrappedRewardResult(type, innerReward.Calculate()) : new RewardResult();
            return result;
        }

        public override IRewardResult Award(IRewardResult rewardResult)
        {
            innerReward.Award(((WrappedRewardResult)rewardResult).rewardResult);
            return rewardResult;
        }
    }
}
