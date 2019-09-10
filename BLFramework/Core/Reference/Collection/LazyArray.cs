using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Core.Reference.Collection
{
    public class LazyArray<TValue> : LazyCollectionBase<int, TValue> where TValue : IDescription
    {
        public LazyArray(RawNode descriptionNode, IContext context = null) : base(descriptionNode, context)
        {
        }

        protected override TValue GetDescription(int collectionKey)
        {
            TValue value;

            if (items.TryGetValue(collectionKey, out value))
                return value;

            value = Factory(descriptionNode.GetNode(collectionKey));
            items.Add(collectionKey, value);

            return value;
        }

        public int Count()
        {
            return descriptionNode.nodesCount;
        }

        public override IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            for (int i = 0; i < descriptionNode.nodesCount; i++)
            {
                yield return new KeyValuePair<int, TValue>(i, GetDescription(i));
            }
        }
    }
}
