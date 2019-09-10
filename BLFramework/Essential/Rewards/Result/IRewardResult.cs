using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Basement.BLFramework.Essential.Rewards.Result
{
    public interface IRewardResult : ISerialize, IHasContext
    {
        string type { get; }
    }
}