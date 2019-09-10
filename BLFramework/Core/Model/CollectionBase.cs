using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Util;

namespace Basement.BLFramework.Core.Model
{
    public abstract class CollectionBase<TModel> : ModelBase where TModel : IModel
    {
        protected CollectionBase(string key, IContext context, IModel parent) : base(key, context, parent)
        {
        }

        public virtual TModel this[string collectionKey] => (TModel)GetChildren()[collectionKey];

        public override object Serialize()
        {
            var result = SerializeUtil.Dict();

            foreach (var pair in this)
            {
                var serialized = pair.Value.Serialize();

                if (serialized != null)
                {
                    result.Add(pair.Key, serialized);
                }
            }

            return result;
        }
    }
}
