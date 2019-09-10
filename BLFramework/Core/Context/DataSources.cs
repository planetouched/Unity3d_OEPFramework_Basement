using System.Collections.Generic;
using Basement.BLFramework.Core.Common;
using Basement.BLFramework.Core.Reference.Description;

namespace Basement.BLFramework.Core.Context
{
    public class DataSources : IChildren<IDescription>
    {
        private readonly IDictionary<string, IDescription> _children = new Dictionary<string, IDescription>();
        
       
        public IDescription GetChild(string collectionKey)
        {
            return _children[collectionKey];
        }

        public void AddChild(IDescription child)
        {
            _children.Add(child.key, child);
        }

        public void RemoveChild(string collectionKey)
        {
            throw new System.NotImplementedException();
        }

        public bool Exist(string collectionKey)
        {
            return _children.ContainsKey(collectionKey);
        }
        
        public T GetChild<T>(string collectionKey) where T : class
        {
            return (T)_children[collectionKey];
        }
    }
}