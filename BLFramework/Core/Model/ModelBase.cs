using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.Common;

#if REFVIEW
using Basement.Common.Util;
#endif

namespace Basement.BLFramework.Core.Model
{
    public abstract class ModelBase : 
    #if REFVIEW
            ReferenceCounter,
    #endif
        
    IModel, IEnumerable<KeyValuePair<string, IModel>>
    {
        public string key { get; set; }
        private readonly WeakRef<IContext> _weakContext;
        private WeakRef<IModel> _weakParent;
        private Event _modelEvent;
        private IOrderedDictionary _children;
        public event Action<IModel> onDestroy;

        protected ModelBase(string key, IContext context, IModel parent = null)
        {
            _weakContext = new WeakRef<IContext>(context);
            this.key = key;

            if (parent != null)
            {
                SetParent(parent);
            }
        }

        protected IDictionary GetChildren()
        {
            return _children ?? (_children = new OrderedDictionary());
        }
        
        public IContext GetContext()
        {
            return _weakContext.obj;
        }

        public Event GetEvent()
        {
            return _modelEvent ?? (_modelEvent = new Event());
        }

        public void Attach(EventCategory category, Event.EventHandler func)
        {
            GetEvent().Attach(category, func);
        }

        public void Detach(EventCategory category, Event.EventHandler func)
        {
            GetEvent().Detach(category, func);
        }

        public virtual void Initialization()
        {
        }

        public void SetParent(IModel parent)
        {
            if (parent == null)
            {
                _weakParent = null;
                return;
            }
            
            _weakParent = new WeakRef<IModel>(parent);
        }

        public IModel GetParent()
        {
            return _weakParent?.obj;
        }

        public virtual IModel GetChild(string collectionKey)
        {
            return (IModel)GetChildren()[collectionKey];
        }

        public virtual void AddChild(IModel child)
        {
            GetChildren().Add(child.key, child);
            child.SetParent(this);
            child.onDestroy += OnDestroy;
        }

        private void OnDestroy(IModel model)
        {
            RemoveChild(model.key);
        }

        public void RemoveChild(string collectionKey)
        {
            if (_children == null) return;

            var child = GetChild(collectionKey);
            child.SetParent(null);
            child.onDestroy -= OnDestroy;
            _children.Remove(collectionKey);
        }

        public bool Exist(string collectionKey)
        {
            return GetChildren().Contains(collectionKey);
        }
        
        public virtual bool CheckAvailable()
        {
            return true;
        }

        public IList<IModel> GetModelPath(bool check)
        {
            var models = new List<IModel>();
            IModel current = this;

            while (current != null)
            {
                if (check && !current.CheckAvailable())
                    return null;

                models.Add(current);
                current = current.GetParent();
            }

            return models;
        }

        public IEnumerator<KeyValuePair<string, IModel>> GetEnumerator()
        {
            foreach (DictionaryEntry pair in GetChildren())
            {
                yield return new KeyValuePair<string, IModel>((string)pair.Key, (IModel)pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void Call(EventCategory category, object args = null)
        {
            var models = GetModelPath(true);
            Event.Call(category, models, args, GetContext());
        }

        public virtual object Serialize()
        {
            return null;
        }

        public virtual void Destroy()
        {
            _modelEvent?.Clear();

            SetParent(null);

            if (_children != null)
            {
                IModel[] copyArray = new IModel[_children.Count];
                _children.Values.CopyTo(copyArray, 0);
                
                foreach (var model in copyArray)
                {
                    model.Destroy();
                }
            }

            onDestroy?.Invoke(this);
            onDestroy = null;
        }
    }
}
