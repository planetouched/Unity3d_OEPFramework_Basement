using System.Collections.Generic;
using Basement.BLFramework.Essential.Rewards.Result;

namespace Basement.BLFramework.Essential.Rewards
{
    public static class RewardUtil
    {
        public static IReward[] Decomposite(IReward reward, List<IReward> rewards = null)
        {
            if (rewards == null)
                rewards = new List<IReward>();

            var wr = reward as WrappedReward;

            if (wr != null)
                Decomposite(wr.innerReward, rewards);
            else
            {
                var cr = reward as CompositeReward;

                if (cr != null)
                {
                    foreach (var r in cr.rewards)
                        Decomposite(r.Value, rewards);
                }
                else
                    rewards.Add(reward);
            }

            return rewards.ToArray();
        }

        public static IRewardResult[] DecompositeResult(IRewardResult reward, List<IRewardResult> rewards = null)
        {
            if (rewards == null)
                rewards = new List<IRewardResult>();

            var wr = reward as WrappedRewardResult;

            if (wr != null)
                DecompositeResult(wr.rewardResult, rewards);
            else
            {
                var cr = reward as CompositeRewardResult;

                if (cr != null)
                {
                    foreach (var r in cr.results)
                        DecompositeResult(r, rewards);
                }
                else
                    rewards.Add(reward);
            }

            return rewards.ToArray();
        }
    }
}
