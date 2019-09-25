using System;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Collection;
using Basement.Common;

namespace Basement.BLFramework.Essential.Prices
{
    public class DynamicCompositePrice : DynamicPrice
    {
        public LazyArray<DynamicPrice> prices { get; }

        public DynamicCompositePrice(RawNode node, IContext context) : base(node, context)
        {
            prices = new LazyArray<DynamicPrice>(node.GetNode("prices"), context);
        }
        
        public override bool Check(int amount)
        {
            if (amount < 0)
            {
                throw new Exception("amount < 0");
            }
            
            foreach (var price in prices)
            {
                if (!price.Value.Check(amount))
                    return false;
            }
            return true;
        }

        public override void Pay(int amount)
        {
            if (!Check(amount)) return;
            
            foreach (var price in prices)
            {
                price.Value.Pay(amount);
            }
        }
    }
}