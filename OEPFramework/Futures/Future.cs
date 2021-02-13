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
        }

        public virtual void Complete()
        {
            if (isCancelled || isDone)
                return;
            isDone = true;
            OnComplete();
            CallHandlers();
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
    }
}
