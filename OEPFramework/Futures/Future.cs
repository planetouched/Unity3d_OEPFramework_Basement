namespace Basement.OEPFramework.Futures
{
    public abstract class Future : FutureBase
    {
        public override void Cancel()
        {
            if (promise || isCancelled || isDone)
                return;
            isCancelled = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public void Complete()
        {
            if (isCancelled || isDone)
                return;
            isDone = true;
            OnComplete();
            CallHandlers();
            CallFinalizeHandlers();
        }

        public override IFuture Run()
        {
            if (wasRun || isCancelled || isDone) return this;
            wasRun = true;
            OnRun();
            CallRunHandlers();
            return this;
        }

        protected abstract void OnRun();
        protected abstract void OnComplete();
        
        public override bool Reuse()
        {
            if (!isDone && !isCancelled) return false;
            
            isCancelled = false;
            isDone = false;
            wasRun = false;
            return true;
        }
    }
}
