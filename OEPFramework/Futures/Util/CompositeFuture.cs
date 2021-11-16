using System;
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
            isRun = false;
            return true;
        }

        public override void Complete(bool external = false)
        {
            if (isPromise || isCancelled || isDone) return;
            
            isExternal = external;
            
            isDone = true;
            isRun = false;
            var copy = GetFuturesCopyList();
            _futures.Clear();

            foreach (var future in copy)
            {
                if (future.isPromise)
                {
                    throw new Exception("future.isPromise == true");
                }
                
                if (future.isCancelled || future.isDone) continue;
                
                future.RemoveListener(OnFutureComplete);
                future.Complete(external);
            }

            CallHandlers();
            CallFinalizeHandlers();
        }

        public override void Cancel()
        {
            if (isPromise || isCancelled || isDone) return;
            
            isCancelled = true;
            isRun = false;
            var copy = GetFuturesCopyList();
            _futures.Clear();

            foreach (var future in copy)
            {
                if (future.isPromise)
                {
                    throw new Exception("future.isPromise == true");
                }
                
                if (future.isCancelled || future.isDone) continue;
                
                future.RemoveListener(OnFutureComplete);
                future.Cancel();
            }

            CallHandlers();
            CallFinalizeHandlers();
        }

        public void AddFuture(IFuture future)
        {
            if (isRun || isDone || isCancelled || future.isDone || future.isCancelled) return;

            _futures.Add(future);
        }

        private void OnFutureComplete(IFuture future)
        {
            _futures.Remove(future);
            future.RemoveListener(OnFutureComplete);

            if (_futures.Count > 0) return;
            
            isDone = true;
            isRun = false;

            CallHandlers();
            CallFinalizeHandlers();
        }

        public override IFuture Run()
        {
            if (isRun) return this;
            
            isRun = true;
            isDone = _futures.Count == 0;

            CallRunHandlers();

            if (isDone)
            {
                isRun = false;
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
