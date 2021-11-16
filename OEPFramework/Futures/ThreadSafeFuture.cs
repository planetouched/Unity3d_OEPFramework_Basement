using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures
{
    public abstract class ThreadSafeFuture : IFuture
    {
        private readonly object _syncRoot;
        public bool isCancelled { get; private set; }
        public bool isDone { get; private set; }
        public bool isRun { get; private set; }
        public bool isExternal { get; private set;}
        public bool isPromise { get; private set; }

        private event Action<IFuture> onFinalize;
        private event Action<IFuture> onRun;
        
        private readonly List<(FutureCompletionState state, Action<IFuture> action)> _onComplete = new List<(FutureCompletionState state, Action<IFuture> action)>(2); 

        protected ThreadSafeFuture()
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
            var copy = new List<(FutureCompletionState state, Action<IFuture> action)>(_onComplete);
            
            for (int i = 0; i < copy.Count; i++)
            {
                var state = copy[i].state;
                if (CallCheck(state))
                {
                    copy[i].action(this);
                }
            }

            // it is ok. because isDone or isCancelled have been already set
            _onComplete.Clear();
        }

        private void CallFinalizeHandlers()
        {
            onFinalize?.Invoke(this);
            onFinalize = null;
        }

        private bool CallCheck(FutureCompletionState state)
        {
            return state == FutureCompletionState.Both ||
                   state == FutureCompletionState.Done && isDone ||
                   state == FutureCompletionState.Cancelled && isCancelled;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            bool call = false;
            lock (_syncRoot)
            {
                if (!isRun)
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

        public IFuture AddListener(FutureCompletionState state, Action<IFuture> method)
        {
            bool call = false;
            lock (_syncRoot)
            {
                if (!isDone && !isCancelled)
                {
                    _onComplete.Add((state, method));
                }
                else
                    call = true;
            }

            if (call)
            {
                if (CallCheck(state))
                {
                    method(this);
                }
            }

            return this;
        }

        public void RemoveListener(Action<IFuture> method)
        {
            lock (_syncRoot)
            {
                for (int i = 0; i < _onComplete.Count; i++)
                {
                    if (_onComplete[i].action == method)
                    {
                        _onComplete.RemoveAt(i);
                        break;
                    }
                }
            }
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
                if (isPromise || isCancelled || isDone) return;
                isCancelled = true;
            }

            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public void Complete(bool external = false)
        {
            lock (_syncRoot)
            {
                if (isCancelled || isDone) return;
                isDone = true;
                isExternal = external;
            }

            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public IFuture Run()
        {
            lock (_syncRoot)
            {
                if (isRun || isCancelled || isDone) return this;
                isRun = true;
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
                isRun = false;
                return true;
            }
        }

        protected void SetAsPromise()
        {
            lock (_syncRoot)
            {
                isPromise = true;
            }
        }
    }
}
