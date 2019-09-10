using System.Collections.Generic;

namespace Basement.BLFramework.Essential.Prices
{
    public static class PriceUtil
    {
        public static IPrice[] Decomposite(IPrice price, List<IPrice> prices = null)
        {
            if (prices == null)
                prices = new List<IPrice>();

            var cp = price as CompositePrice;

            if (cp != null)
            {
                foreach (var pr in cp.prices)
                    Decomposite(pr.Value, prices);
            }
            else
                prices.Add(price);
            
            return prices.ToArray();
        }
    }
}
