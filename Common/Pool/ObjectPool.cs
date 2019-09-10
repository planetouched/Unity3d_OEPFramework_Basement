using System;
using System.Collections.Generic;

namespace Basement.Common.Pool
{
    public class ObjectPool<T> : IObjectPool where T : class
    {
        private readonly Dictionary<T, bool> _checker = new Dictionary<T, bool>();
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _locker = new object();

        public int unusedObjectsCount
        {
            get
            {
                lock (_locker)
                    return _queue.Count;
            }
        }

        public int usedObjectsCount
        {
            get
            {
                lock (_locker)
                    return _checker.Count - _queue.Count;
            }
        }

        public int objectsCount
        {
            get
            {
                lock (_locker)
                    return _checker.Count;
            }
        }

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
            lock (_locker)
            {
                if (_checker.ContainsKey(obj))
                    throw new Exception("Add(T obj)");
                
                _checker.Add(obj, true);
                _queue.Enqueue(obj);
            }
        }

        public void ReturnObj(object obj)
        {
            Return((T)obj);
        }
        
        public void Return(T obj)
        {
            lock (_locker)
            {
                if (_checker[obj])
                    throw new Exception("Return(T obj)");

                _checker[obj] = true;
                _queue.Enqueue(obj);
            }
        }

        public T Take()
        {
            lock (_locker)
            {
                T obj;

                if (_queue.Count == 0)
                {
                    if (_createFunc == null)
                        return null;

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
        }

        public bool ForEach(Action<T> iterationAction)
        {
            if (usedObjectsCount > 0)
                return false;

            foreach (var item in _queue)
                iterationAction(item);

            return true;
        }

        public bool RemoveAll(Action<T> destroyAction = null)
        {
            if (usedObjectsCount > 0)
                return false;

            if (destroyAction != null)
            {
                foreach (var item in _queue)
                    destroyAction(item);
            }

            _queue.Clear();
            _checker.Clear();
            return true;
        }
    }
}