using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Model;

namespace Basement.BLFramework.Core.ThroughEvent
{
    public class ModelsPath
    {
        private Dictionary<Type, IModel> _stack;
        private List<IModel> _models;

        public void Set(IEnumerable<IModel> collection, bool reverse)
        {
            _models = new List<IModel>(collection);

            if (reverse)
            {
                _models.Reverse();
            }
        }

        public T GetSelf<T>() where T : IModel
        {
            return (T)GetSelf();
        }

        public IModel GetSelf()
        {
            return _models[_models.Count - 1];
        }

        public T Get<T>() where T : class
        {
            if (_stack == null)
            {
                _stack = new Dictionary<Type, IModel>();
                for (int i = 0; i < _models.Count - 1; i++)
                {
                    _stack.Add(_models[i].GetType(), _models[i]);
                }
            }
            return (T)_stack[typeof(T)];
        }

        public override string ToString()
        {
            if (_models.Count == 0)
            {
                return string.Empty;
            }

            string path = _models[0].key + ".";

            for (int i = 1; i < _models.Count; i++)
            {
                path += _models[i].key + (i < _models.Count - 1 ? "." : string.Empty);
            }

            return path;
        }
    }
}
