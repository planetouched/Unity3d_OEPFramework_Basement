using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Choices
{
    public class SimplePathChoice : PathChoice
    {
        public SimplePathChoice(RawNode node, IContext context)
            : base(node, context)
        {
        }

        public override ModelsPath GetModelPath()
        {
            return PathUtil.GetModelPath(GetContext(), node.GetString("path"), random);
        }

        public override T GetDescription<T>()
        {
            return PathUtil.GetDescription<T>(GetContext(), node.GetString("path"), random);
        }
    }
}