using Basement.BLFramework.Essential.Rewards.Result;

namespace Basement.BLFramework.Essential.Rewards
{
    public interface IDynamicReward
    {
        string type { get; }
        IRewardResult Award(int amount);
        IRewardResult Award(IRewardResult rewardResult);
        IRewardResult Calculate(int amount);
    }
}