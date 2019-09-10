using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Essential.EssentialRandom
{
    public class RandomDescription : DescriptionBase
    {
        public string type { get; }

        public RandomDescription(RawNode node, IContext context = null) : base(node, context)
        {
            type = node.GetString("type");
        }
    }
}
