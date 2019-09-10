using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Collection;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public abstract class CompositeRequirement : Requirement
    {
        public LazyArray<Requirement> requirements { get; }

        protected CompositeRequirement(RawNode node, IContext context)
            : base(node, context)
        {
            requirements = new LazyArray<Requirement>(node.GetNode("requirements"), context);
        }
    }
}