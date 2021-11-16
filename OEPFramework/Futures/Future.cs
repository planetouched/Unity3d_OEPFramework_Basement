namespace Basement.OEPFramework.Futures
{
    public abstract class Future : FutureBase
    {
        public override void Cancel()
        {
            if (isPromise || isCancelled || isDone)
                return;
            isCancelled = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public override void Complete(bool external = false)
        {
            if (isCancelled || isDone)
                return;

            isExternal = external;
            isDone = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public override IFuture Run()
        {
            if (isRun || isCancelled || isDone) return this;
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
