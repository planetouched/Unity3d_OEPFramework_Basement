using System;

namespace Basement.OEPFramework.Futures
{
    public abstract class ThreadSafeFuture : IFuture
    {
        private readonly object _syncRoot;
        public bool isCancelled { get; private set; }
        public bool isDone { get; private set; }
        public bool wasRun { get; private set; }

        private event Action<IFuture> onComplete;
        private event Action<IFuture> onFinalize;
        private event Action<IFuture> onRun;
        private volatile bool _promise;

        protected ThreadSafeFuture ()
        {
            _syncRoot = new object();
        }

        private void CallRunHandlers()
        {
            onRun?.Invoke(this);
            onRun = null;
        }

        private void CallHandlers()
        {
            onComplete?.Invoke(this);
            onComplete = null;
        }

        private void CallFinalizeHandlers()
        {
            onFinalize?.Invoke(this);
            onFinalize = null;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            bool call = false;
            lock (_syncRoot)
            {
                if (!wasRun)
                    onRun += method;
                else
                    call = true;
            }

            if (call)
            {
                method(this);
            }

            return this;
        }

        public void RemoveListenerOnRun(Action<IFuture> method)
        {
            lock (_syncRoot)
            {
                onRun -= method;
            }
        }

        public IFuture AddListener(Action<IFuture> method)
        {
            bool call = false;
            lock (_syncRoot)
            {
                if (!isDone && !isCancelled)
                    onComplete += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }

        public void RemoveListener(Action<IFuture> method)
        {
            lock (_syncRoot)
                onComplete -= method;
        }
        
        public IFuture AddListenerOnFinalize(Action<IFuture> method)
        {
            bool call = false;
            lock (_syncRoot)
            {
                if (!isDone && !isCancelled)
                    onFinalize += method;
                else
                    call = true;
            }

            if (call)
                method(this);

            return this;
        }

        public void RemoveListenerOnFinalize(Action<IFuture> method)
        {
            lock (_syncRoot)
                onFinalize -= method;
        }

        public void Cancel()
        {
            lock (_syncRoot)
            {
                if (_promise || isCancelled || isDone) return;
                isCancelled = true;
            }

            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public void Complete()
        {
            lock (_syncRoot)
            {
                if (isCancelled || isDone) return;
                isDone = true;
            }

            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public IFuture Run()
        {
            lock (_syncRoot)
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

        public virtual bool Reuse()
        {
            lock (_syncRoot)
            {
                if (!isDone && !isCancelled) return false;
            
                isCancelled = false;
                isDone = false;
                wasRun = false;
                return true;
            }
        }

        protected void SetAsPromise()
        {
            _promise = true;
        }
    }
}
