using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public class OrRequirement : CompositeRequirement
    {
        public OrRequirement(RawNode node, IContext context)
            : base(node, context)
        {
        }

        public override bool Check()
        {
            foreach (var requirement in requirements)
            {
                if (requirement.Value.Check()) return true;
            }

            return false;
        }
    }
}