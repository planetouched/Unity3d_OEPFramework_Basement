using System;
using System.Collections.Generic;

namespace Basement.Common.Pool
{
    public class ObjectPool<T> : IObjectPool where T : class
    {
        public enum Items
        {
            All, Used, Unused
        }
        
        private readonly Dictionary<T, bool> _checker = new Dictionary<T, bool>();
        private readonly Queue<T> _queue = new Queue<T>();

        public int unusedObjectsCount => _queue.Count;

        public int usedObjectsCount => _checker.Count - _queue.Count;

        public int objectsCount => _checker.Count;

        private Func<T> _createFunc;

        public ObjectPool(Func<T> createFunc = null)
        {
            _createFunc = createFunc;
        }

        public void SetFactory(Func<T> factory)
        {
            _createFunc = factory;
        }

        public void UnsetFactory()
        {
            _createFunc = null;
        }

        public void Add(T obj)
        {
            if (_checker.ContainsKey(obj))
            {
                throw new Exception("Add(T obj)");
            }

            _checker.Add(obj, true);
            _queue.Enqueue(obj);
        }

        public void ReturnObj(object obj)
        {
            Return((T)obj);
        }
        
        public void Return(T obj)
        {
            if (_checker[obj])
            {
                throw new Exception("Return(T obj)");
            }

            _checker[obj] = true;
            _queue.Enqueue(obj);
        }

        public T Take()
        {
            T obj;

            if (_queue.Count == 0)
            {
                if (_createFunc == null) return null;

                obj = _createFunc();
                _checker.Add(obj, false);
            }
            else
            {
                obj = _queue.Dequeue();
                _checker[obj] = false;
            }

            return obj;
        }

        public void ForEach(Action<T> iterationAction, Items items)
        {
            if (items == Items.All || items == Items.Used)
            {
                foreach (var pair in _checker)
                {
                    if (!pair.Value || items == Items.All)
                    {
                        iterationAction(pair.Key);
                    }
                }
            }
            else
            {
                foreach (var item in _queue)
                {
                    iterationAction(item);
                }
            }
        }

        public void RemoveAll(Action<T> destroyAction = null)
        {
            if (destroyAction != null)
            {
                foreach (var item in _checker)
                {
                    destroyAction(item.Key);
                }
            }

            _queue.Clear();
            _checker.Clear();
        }
    }
}