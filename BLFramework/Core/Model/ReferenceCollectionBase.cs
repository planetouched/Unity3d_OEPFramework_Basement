using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Basement.BLFramework.Core.Model
{
    public abstract class ReferenceCollectionBase<TModel, TCategories, TDescription> : CollectionBase<TModel>, IReferenceModel, IEnumerable<KeyValuePair<string, TModel>>, IReferenceCollection
        where TDescription : IDescription
        where TCategories : class
        where TModel : IReferenceModel
    {
        public TCategories categories { get; }
        public IDescription dataSource { get; }
        private readonly RawNode _initNode;
        public bool selectable { get; }

        protected ReferenceCollectionBase(RawNode initNode, TCategories categories, IContext context, IDescription dataSource) : base(dataSource.key, context, null)
        {
            _initNode = initNode;
            this.dataSource = dataSource;
            this.categories = categories;
            selectable = true;
        }

        public override TModel this[string collectionKey]
        {
            get
            {
                TModel model = base[collectionKey];

                if (model != null)
                {
                    return model;
                }

                var isExist = dataSource.Exist(collectionKey); 
                var description = (TDescription)dataSource.Build(collectionKey);
                model = Factory(_initNode.GetNode(collectionKey), description);
                AddChild(model);

                if (!isExist)
                {
                    description.Initialization();
                }
                
                model.Initialization();

                return model;
            }
        }

        public override IModel GetChild(string collectionKey)
        {
            return this[collectionKey];
        }

        public new IEnumerator<KeyValuePair<string, TModel>> GetEnumerator()
        {
            foreach (var sortedKey in dataSource.GetNode().GetSortedKeys())
            {
                yield return new KeyValuePair<string, TModel>(sortedKey, this[sortedKey]);
            }
        }

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict();

            foreach (var unsortedKey in dataSource.GetNode().GetUnsortedKeys())
            {
                if (Exist(unsortedKey))
                {
                    var serialized = this[unsortedKey].Serialize();

                    if (serialized != null)
                    {
                        dict.Add(unsortedKey, serialized);
                    }
                }
                else
                {
                    var node = _initNode.GetNode(unsortedKey);
                    
                    if (node.IsInit())
                    {
                        dict.Add(unsortedKey, _initNode.GetNode(unsortedKey).GetRawData());
                    }
                }
            }

            return dict;
        }

        protected abstract TModel Factory(RawNode initNode, TDescription description);
    }
}
