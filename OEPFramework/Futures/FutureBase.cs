using System;
using System.Collections.Generic;

#if REFVIEW
using Basement.Common.Util;
#endif

namespace Basement.OEPFramework.Futures
{
    public abstract class FutureBase :
#if REFVIEW
    ReferenceCounter,
#endif
        IFuture
    {
        public bool isCancelled { get; protected set; }
        public bool isDone { get; protected set; }
        public bool wasRun { get; protected set; }

        private event Action<IFuture> onFinalize;
        private event Action<IFuture> onRun;
        protected bool promise;

        private List<(FutureCompletionState state, Action<IFuture> action)> _onComplete = new List<(FutureCompletionState state, Action<IFuture> action)>(); 

        protected void CallRunHandlers()
        {
            onRun?.Invoke(this);
            onRun = null;
        }

        protected void CallHandlers()
        {
            for (int i = 0; i < _onComplete.Count; i++)
            {
                var state = _onComplete[i].state;
                if (state == FutureCompletionState.Both || state == FutureCompletionState.Done && isDone || state == FutureCompletionState.Cancelled && isCancelled)
                {
                    _onComplete[i].action(this);
                }
            }
            
            _onComplete.Clear();
        }
        
        protected void CallFinalizeHandlers()
        {
            onFinalize?.Invoke(this);
            onFinalize = null;
        }

        public IFuture AddListenerOnRun(Action<IFuture> method)
        {
            if (!wasRun)
                onRun += method;
            else
                method(this);

            return this;
        }

        public void RemoveListenerOnRun(Action<IFuture> method)
        {
            onRun -= method;
        }

        public IFuture AddListener(FutureCompletionState state, Action<IFuture> method)
        {
            if (!isDone && !isCancelled)
            {
                _onComplete.Add((state, method));
            }
            else
            {
                if (state == FutureCompletionState.Both || state == FutureCompletionState.Done && isDone || state == FutureCompletionState.Cancelled && isCancelled)
                {
                    method(this);
                }
            }

            return this;
        }


        public void RemoveListener(Action<IFuture> method)
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
        
        public IFuture AddListenerOnFinalize(Action<IFuture> method)
        {
            if (!isDone && !isCancelled)
                onFinalize += method;
            else
                method(this);

            return this;
        }

        public void RemoveListenerOnFinalize(Action<IFuture> method)
        {
            onFinalize -= method;
        }

        public abstract void Cancel();
        public abstract IFuture Run();

        public static T StaticCast<T>(IFuture future)
        {
            return (T)future;
        }

        public T Cast<T>() where T : IFuture
        {
            return StaticCast<T>(this);
        }

        public abstract bool Reuse();

        protected void SetAsPromise()
        {
            promise = true;
        }
    }
}
