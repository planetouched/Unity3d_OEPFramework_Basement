using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Requirements
{
    public class Requirement : DescriptionBase, IRequirement
    {
        public string type { get; }
        private ModelsPath _cache;

        public Requirement(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public ModelsPath GetPath()
        {
            return _cache ?? (_cache = PathUtil.GetModelPath(GetContext(), node.GetString("path"), null));
        }

        public virtual bool Check()
        {
            return true;
        }
    }
}