using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public class DynamicPrice : DescriptionBase, IDynamicPrice
    {
        public string type { get; }
        private ModelsPath _cache;

        public DynamicPrice(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
        }

        public ModelsPath GetPath()
        {
            return _cache ?? (_cache = PathUtil.GetModelPath(GetContext(), node.GetString("path"), null));
        }

        public virtual bool Check(int amount)
        {
            return true;
        }

        public virtual void Pay(int amount)
        {
        }
    }
}