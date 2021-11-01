using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class CompositeFuture : FutureBase, IFutureCollection
    {
        public int futuresCount => _futures.Count;

        private readonly List<IFuture> _futures = new List<IFuture>();

        public CompositeFuture(IEnumerable<IFuture> futures)
        {
            foreach (var future in futures)
            {
                if (future == null) continue;
                AddFuture(future);
            }
        }

        public CompositeFuture(params IFuture[] futures)
        {
            foreach (var future in futures)
            {
                if (future == null) continue;
                AddFuture(future);
            }
        }
        
        public override bool Reuse()
        {
            if (!isDone && !isCancelled) return false;

            isCancelled = false;
            isDone = false;
            wasRun = false;
            return true;
        }
        
        public override void Cancel()
        {
            if (promise || isCancelled || isDone) return;
            
            isCancelled = true;
            wasRun = false;
            var copy = GetFuturesCopyList();
            _futures.Clear();

            foreach (var future in copy)
            {
                if (future.isCancelled) continue;
                
                future.RemoveListener(OnFutureComplete);
                future.Cancel();
            }

            CallHandlers();
            CallFinalizeHandlers();
        }

        public void AddFuture(IFuture future)
        {
            if (wasRun || isDone || isCancelled || future.isDone || future.isCancelled) return;

            _futures.Add(future);
        }

        private void OnFutureComplete(IFuture future)
        {
            _futures.Remove(future);
            future.RemoveListener(OnFutureComplete);

            if (_futures.Count > 0) return;
            
            isDone = true;
            wasRun = false;

            CallHandlers();
            CallFinalizeHandlers();
        }

        public override IFuture Run()
        {
            if (wasRun) return this;
            
            wasRun = true;
            isDone = _futures.Count == 0;

            CallRunHandlers();

            if (isDone)
            {
                wasRun = false;
                CallHandlers();
                CallFinalizeHandlers();
            }
            else
            {
                foreach (var future in GetFuturesCopyList())
                {
                    future.Run();
                    future.AddListener(FutureCompletionState.Both, OnFutureComplete);
                }
            }
            return this;
        }
        
        private List<IFuture> GetFuturesCopyList()
        {
            return new List<IFuture>(_futures);
        }
    }
}
