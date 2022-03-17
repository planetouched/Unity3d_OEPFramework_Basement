using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public class SimpleRequirement : Requirement
    {
        private bool _checked;
        
        public SimpleRequirement(RawNode node, IContext context) : base(node, context)
        {
            _checked = node.GetBool("checked");
        }

        public override bool Check()
        {
            return _checked;
        }
    }
}