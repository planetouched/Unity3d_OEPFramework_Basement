using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Choices;
using Basement.BLFramework.Essential.Path;
using Basement.BLFramework.Essential.Rewards.Result;
using Basement.Common;

namespace Basement.BLFramework.Essential.Rewards
{
    public class Reward : DescriptionBase, IReward
    {
        public string type { get; }
        public IPathChoice choice { get; }
        public ModelsPath path { get; }

        public Reward(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");

            if (node.CheckKey("choice"))
            {
                choice = FactoryManager.Build<PathChoice>(node.GetNode("choice"), context);
            }
            else
            {
                if (node.CheckKey("path"))
                {
                    path = PathUtil.GetModelPath(GetContext(), node.GetNode("path"));
                }
            }
        }

        public IRewardResult Award()
        {
            return Award(Calculate());
        }

        public virtual IRewardResult Award(IRewardResult rewardResult)
        {
            return rewardResult;
        }

        public virtual IRewardResult Calculate()
        {
            return new RewardResult();
        }

        protected ModelsPath RewardPath()
        {
            return choice != null ? choice.GetModelPath() : path;
        }
    }
}
