using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Essential.Amounts
{
    public class SimpleAmount : Amount
    {
        public int amount { get; }

        public SimpleAmount(RawNode node, IContext context)
            : base(node, context)
        {
            amount = node.GetInt("amount");
        }
        public override int Number()
        {
            return amount;
        }
    }
}