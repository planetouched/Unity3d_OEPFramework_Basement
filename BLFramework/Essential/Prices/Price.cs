using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public class Price : DescriptionBase, IPrice
    {
        public string type { get; }
        public int amount { get; }
        private ModelsPath _cache;

        public Price(RawNode node, IContext context) : base(node, context)
        {
            type = node.GetString("type");
            amount = node.GetInt("amount");
        }

        public ModelsPath GetPath()
        {
            return _cache ?? (_cache = PathUtil.GetModelPath(GetContext(), node.GetString("path"), null));
        }

        public virtual bool Check()
        {
            return true;
        }

        public virtual void Pay()
        {
        }
    }
}
