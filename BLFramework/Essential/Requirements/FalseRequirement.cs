using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public class FalseRequirement : Requirement
    {
        public FalseRequirement(RawNode node, IContext context) 
            : base(node, context)
        {
        }

        public override bool Check()
        {
            return false;
        }
    }
}