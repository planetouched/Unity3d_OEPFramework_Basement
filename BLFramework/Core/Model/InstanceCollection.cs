using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.Util;
using Basement.Common;

namespace Basement.BLFramework.Core.Model
{
    public abstract class InstanceCollection<TModel, TCategories, TDescription> : ReferenceModelBase<TCategories, TDescription>
        where TModel : IModel
        where TDescription : IDescription
        where TCategories : class
    {
        private int _lastId;

        protected InstanceCollection(RawNode initNode, TCategories categories, TDescription description,
            IContext context) : base(initNode, categories, description, context)
        {
        }

        public TModel this[string collectionKey] => (TModel) GetChild(collectionKey);

        public override void Initialization()
        {
            var collectionNode = initNode.GetNode("collection");

            foreach (var pair in collectionNode.GetUnsortedCollection())
            {
                _lastId = Math.Max(_lastId, int.Parse(pair.Key));
                var model = Factory(pair.Key, collectionNode.GetNode(pair.Key));
                model.key = pair.Key;
                base.AddChild(model);
                model.Initialization();
            }
        }

        public TModel AddChild(RawNode modelNode)
        {
            var id = IncrementAndGetLastCollectionId();
            var model = Factory(id, modelNode);
            model.key = id;
            base.AddChild(model);
            model.Initialization();
            return model;
        }

        public IEnumerable<KeyValuePair<string, TModel>> GetCollection()
        {
            foreach (var pair in this)    
            {
                yield return new KeyValuePair<string, TModel>(pair.Key, (TModel)pair.Value);
            }
        }
        
        protected abstract TModel Factory(string collectionKey, RawNode modelInitNode);

        protected string IncrementAndGetLastCollectionId()
        {
            return (++_lastId).ToString();
        }

        public override object Serialize()
        {
            var dict = SerializeUtil.Dict();

            foreach (var pair in this)
            {
                dict.Add(pair.Key, pair.Value.Serialize());
            }

            if (dict.Count == 0)
            {
                return null;
            }

            return SerializeUtil.Dict().SetArgs("collection", dict);
        }
    }
}