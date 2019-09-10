using Basement.BLFramework.Essential.Rewards.Result;

namespace Basement.BLFramework.Essential.Rewards
{
    public interface IReward
    {
        string type { get; }
        IRewardResult Award();
        IRewardResult Award(IRewardResult rewardResult);
        IRewardResult Calculate();
    }
}
