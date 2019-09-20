using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public abstract class DynamicWrappedPrice : DynamicPrice
    {
        protected IDynamicPrice innerPrice;
        
        protected DynamicWrappedPrice(RawNode node, IContext context) : base(node, context)
        {
            innerPrice = FactoryManager.Build<DynamicPrice>(node.GetNode("price"), context);
        }
    }
}