namespace Basement.OEPFramework.Futures
{
    public abstract class Future : FutureBase
    {
        public override void Cancel()
        {
            if (isPromise || isCancelled || isDone)
                return;

            FuturesRegistry.Complete(this);
            
            isCancelled = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public override void Complete(bool external = false)
        {
            if (isCancelled || isDone)
                return;
            
            FuturesRegistry.Complete(this);

            isExternal = external;
            isDone = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        // private static ThreadLocal<Stack<Future>> _futures = new ThreadLocal<Stack<Future>>(() => new Stack<Future>());

        public override IFuture Run()
        {
            if (isRun || isCancelled || isDone) return this;
            
            FuturesRegistry.Run(this);

            isRun = true;
            CallRunHandlers();
            OnRun();
            return this;
        }

        protected abstract void OnRun();
        protected abstract void OnComplete();
        
        public override bool Reuse()
        {
            if (!isDone && !isCancelled) return false;
            
            isCancelled = false;
            isDone = false;
            isRun = false;
            return true;
        }
    }
}
