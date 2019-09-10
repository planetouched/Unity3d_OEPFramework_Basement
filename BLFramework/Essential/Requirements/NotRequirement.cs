using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public class NotRequirement : WrappedRequirement
    {
        public NotRequirement(RawNode node, IContext context) : base(node, context)
        {
        }

        public override bool Check()
        {
            return !innerRequirement.Check();
        }
    }
}
