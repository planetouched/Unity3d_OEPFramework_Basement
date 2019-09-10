using System;

namespace Basement.OEPFramework.Futures
{
    public class FutureTask<T> : ThreadSafeFuture
    {
        public T result { get; private set; }
        private Func<T> _func;
        public FutureTask(Func<T> func)
        {
            _func = func;
        }

        protected override void OnRun()
        {
            result = _func();
            Complete();
        }

        protected override void OnComplete()
        {
            _func = null;
        }
    }
    
    public class FutureTask : ThreadSafeFuture
    {
        private Action _action;
        public FutureTask(Action action)
        {
            _action = action;
        }

        protected override void OnRun()
        {
            _action();
            Complete();
        }

        protected override void OnComplete()
        {
            _action = null;
        }
    }
}