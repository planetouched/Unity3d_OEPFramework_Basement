using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public abstract class WrappedRequirement : Requirement
    {
        public IRequirement innerRequirement;

        protected WrappedRequirement(RawNode node, IContext context) : base(node, context)
        {
            innerRequirement = FactoryManager.Build<Requirement>(node.GetNode("requirement"), context);
        }
    }
}
