using System;
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

        private event Action<IFuture> onComplete;
        private event Action<IFuture> onFinalize;
        private event Action<IFuture> onRun;
        protected bool promise;

        protected void CallRunHandlers()
        {
            onRun?.Invoke(this);
            onRun = null;
        }

        protected void CallHandlers()
        {
            onComplete?.Invoke(this);
            onComplete = null;
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

        public IFuture AddListener(Action<IFuture> method)
        {
            if (!isDone && !isCancelled)
                onComplete += method;
            else
                method(this);

            return this;
        }


        public void RemoveListener(Action<IFuture> method)
        {
            onComplete -= method;
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
