using System;
using System.Threading;

namespace Basement.OEPFramework.Futures
{
    public abstract class ThreadSafeFuture : IFuture
    {
        static int _globalHashCode;
        private readonly int _hashCode;

        public object syncRoot { get; }
        public bool isCancelled { get; private set; }
        public bool isDone { get; private set; }
        public bool wasRun { get; private set; }

        private event Action<IFuture> _onComplete;
        private event Action<IFuture> _onRun;
        private bool _promise;

        protected ThreadSafeFuture ()
        {
            syncRoot = new object();
            _hashCode = Interlocked.Increment(ref _globalHashCode);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        void CallRunHandlers()
        {
            if (_onRun != null)
                _onRun(this);
            _onRun = null;
        }

        void CallHandlers()
        {
            if (_onComplete != null)
                _onComplete(this);
            _onComplete = null;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            bool call = false;
            lock (syncRoot)
            {
                if (!wasRun)
                    _onRun += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }

        public void RemoveListenerOnRun(Action<IFuture> method)
        {
            lock (syncRoot)
                _onRun -= method;
        }

        public IFuture AddListener(Action<IFuture> method)
        {
            bool call = false;
            lock (syncRoot)
            {
                if (!isDone && !isCancelled)
                    _onComplete += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }


        public void RemoveListener(Action<IFuture> method)
        {
            lock (method)
                _onComplete -= method;
        }

        public void Cancel()
        {
            lock (syncRoot)
            {
                if (_promise || isCancelled || isDone)
                    return;
                isCancelled = true;
            }

            OnComplete();
            CallHandlers();
        }

        public void Complete()
        {
            lock (syncRoot)
            {
                if (isCancelled || isDone)
                    return;
                isDone = true;
            }

            OnComplete();
            CallHandlers();
        }

        public IFuture Run()
        {
            lock (syncRoot)
            {
                if (wasRun || isCancelled || isDone) return this;
                wasRun = true;
            }

            OnRun();
            CallRunHandlers();
            return this;
        }

        protected abstract void OnRun();
        protected abstract void OnComplete();

        public static T StaticCast<T>(IFuture future)
        {
            return (T)future;
        }

        public T Cast<T>() where T : IFuture
        {
            return StaticCast<T>(this);
        }

        public IFuture SetAsPromise()
        {
            _promise = true;
            return this;
        }
    }
}
