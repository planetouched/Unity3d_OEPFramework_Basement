using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Basement.OEPFramework.Futures
{
    public class FutureTask<T> : ThreadSafeFuture
    {
        public T result { get; private set; }
        private readonly Func<T> _func;
        public FutureTask(Func<T> func)
        {
            _func = func;
        }

        protected override void OnRun()
        {
            try
            {
                result = _func();
            }
            catch (Exception e)
            {
#if UNITY_5_3_OR_NEWER
                Debug.LogException(e);
#endif
                throw;
            }
            
            Complete();
        }

        protected override void OnComplete()
        {
        }
    }
    
    public class FutureTask : ThreadSafeFuture
    {
        private readonly Action _action;
        public FutureTask(Action action)
        {
            _action = action;
        }

        protected override void OnRun()
        {
            try
            {
                _action();
            }
            catch (Exception e)
            {
#if UNITY_5_3_OR_NEWER                
                Debug.LogException(e);
#endif
                throw;
            }
            
            Complete();
        }

        protected override void OnComplete()
        {
        }
    }
}