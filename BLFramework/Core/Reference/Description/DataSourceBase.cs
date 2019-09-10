using System;
using System.Collections;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Core.Reference.Description
{
    public abstract class DataSourceBase<TDescription> : DescriptionBase, IEnumerable<KeyValuePair<string, TDescription>> where TDescription : IDescription
    {
        protected abstract TDescription Factory(RawNode partialNode);
        
        public TDescription this[string collectionKey] => GetDescription(collectionKey);

        protected DataSourceBase(RawNode node, IContext context = null) : base(node, context)
        {
        }

        private TDescription GetDescription(string collectionKey)
        {
            IDescription value;

            if (GetChildren().TryGetValue(collectionKey, out value))
                return (TDescription)value;

            if (!node.CheckKey(collectionKey))
            {
                throw new Exception("Key not found");
            }
            
            value = Factory(node.GetNode(collectionKey));
            AddChild(value);
            value.Initialization();
            return (TDescription)value;
        }

        public IEnumerator<KeyValuePair<string, TDescription>> GetEnumerator()
        {
            foreach (var pair in node.GetSortedCollection())
            {
                yield return new KeyValuePair<string, TDescription>(pair.Key, GetDescription(pair.Key));
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override IDescription GetChild(string collectionKey)
        {
            return GetDescription(collectionKey);
        }
    }
}