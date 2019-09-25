using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Collection;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public class CompositePrice : Price
    {
        public LazyArray<Price> prices { get; }

        public CompositePrice(RawNode rawNode, IContext context)
            : base(rawNode, context)
        {
            prices = new LazyArray<Price>(rawNode.GetNode("prices"), context);
        }

        public override bool Check()
        {
            foreach (var price in prices)
            {
                if (!price.Value.Check())
                    return false;
            }
            return true;
        }

        public override void Pay()
        {
            if (!Check()) return;
            
            foreach (var price in prices)
                price.Value.Pay();
        }
    }
}
