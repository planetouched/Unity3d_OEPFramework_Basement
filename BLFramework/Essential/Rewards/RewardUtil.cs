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

            if (reward is WrappedReward wr)
                Decomposite(wr.innerReward, rewards);
            else
            {
                if (reward is CompositeReward cr)
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

            if (reward is WrappedRewardResult wr)
                DecompositeResult(wr.rewardResult, rewards);
            else
            {
                if (reward is CompositeRewardResult cr)
                {
                    foreach (var r in cr.results)
                        DecompositeResult(r, rewards);
                }
                else
                    rewards.Add(reward);
            }

            return rewards.ToArray();
        }

        public static IList<T> Filter<T>(IRewardResult reward, List<T> rewards = null) where T : IRewardResult
        {
            if (rewards == null)
            {
                rewards = new List<T>();
            }

            if (reward is WrappedRewardResult wr)
            {
                if (reward is T targetReward)
                {
                    rewards.Add(targetReward);
                }
                Filter(wr.rewardResult, rewards);
            }
            else
            {
                if (reward is CompositeRewardResult cr)
                {
                    if (reward is T targetReward)
                    {
                        rewards.Add(targetReward);
                    }
                    foreach (var r in cr.results)
                    {
                        Filter(r, rewards);
                    }
                }
                else if (reward is T targetReward)
                {
                    rewards.Add(targetReward);
                }
            }

            return rewards;
        }
    }
}
