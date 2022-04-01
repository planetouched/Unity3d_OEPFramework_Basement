using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Core.Reference.Description
{
    public abstract class DescriptionBase : IDescription
    {
        public bool selectable { get; }
        public string key { get; }
        
        protected readonly RawNode node;
        
        private readonly WeakRef<IContext> _weakContext;
        private WeakRef<IDescription> _weakParent;
        private IDictionary<string, IDescription> _items;
        

        protected DescriptionBase(RawNode node, IContext context = null)
        {
            this.node = node;
            key = node.nodeKey;
            selectable = node.GetBool("selectable", true);

            if (context != null)
            {
                _weakContext = new WeakRef<IContext>(context);
            }
        }

        public string GetDescriptionPath()
        {
            var path = new List<string>();
            IDescription current = this;
            
            while (current != null)
            {
                path.Add(current.key);
                current = current.GetParent();
            }
            
            path.Reverse();
            return string.Join(".", path.ToArray());
        }

        public virtual void Initialization()
        {
        }

        public virtual IDescription Build(string collectionKey)
        {
            return null;
        }

        protected IDictionary<string, IDescription> GetChildren()
        {
            return _items ?? (_items = new Dictionary<string, IDescription>());
        }
        
        public virtual IDescription GetChild(string collectionKey)
        {
            return GetChildren()[collectionKey];
        }

        public virtual T GetChild<T>(string collectionKey) where T : IDescription
        {
            return (T)GetChildren()[collectionKey];
        }

        public void AddChild(IDescription child)
        {
            GetChildren().Add(child.key, child);
            child.SetParent(this);
        }

        public virtual void RemoveChild(string collectionKey)
        {
            var description = GetChildren()[collectionKey];
            description.SetParent(null);
            GetChildren().Remove(collectionKey);
        }

        public bool Exist(string collectionKey)
        {
            return GetChildren().ContainsKey(collectionKey);
        }

        public RawNode GetNode()
        {
            return node;
        }

        public IContext GetContext()
        {
            return _weakContext?.obj;
        }

        public void SetParent(IDescription parent)
        {
            if (parent == null)
            {
                _weakParent = null;
                return;
            }
            
            _weakParent = new WeakRef<IDescription>(parent);
        }

        public IDescription GetParent()
        {
            return _weakParent == null ? null : _weakParent.obj;
        }
    }
}
