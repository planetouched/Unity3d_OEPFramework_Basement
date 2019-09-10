using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Factories;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public abstract class WrappedPrice : Price
    {
        protected IPrice innerPrice;

        protected WrappedPrice(RawNode node, IContext context) : base(node, context)
        {
            innerPrice = FactoryManager.Build<Price>(node.GetNode("price"), context);
        }
    }
}
