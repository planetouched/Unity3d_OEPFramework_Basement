using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Essential.EssentialRandom;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Amounts
{
    public class SetAmount : Amount
    {
        private readonly int[] _elements;
        private readonly Random _random;

        public SetAmount(RawNode node, IContext context)
            : base(node, context)
        {
            _elements = node.GetIntArray("elements");
            _random = PathUtil.GetModelPath(GetContext(), node.GetString("random"), null).GetSelf<Random>();
        }

        public override int Number()
        {
            return _elements[_random.Range(0, _elements.Length)];
        }
    }
}