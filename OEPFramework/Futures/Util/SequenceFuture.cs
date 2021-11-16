using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class SequenceFuture : FutureBase, IFutureCollection
    {
        public int futuresCount => _futures.Count;
        
        private readonly List<IFuture> _futures = new List<IFuture>();
        
        public List<IFuture> GetFuturesCopy()
        {
            return new List<IFuture>(_futures);
        }
        
        public SequenceFuture(IEnumerable<IFuture> futures)
        {
            foreach (var future in futures)
            {
                if (future == null) continue;
                AddFuture(future);
            }
        }

        public SequenceFuture(params IFuture[] futures)
        {
            foreach (var future in futures)
            {
                if (future == null) continue;
                AddFuture(future);
            }
        }

        public void AddFuture(IFuture future)
        {
            if (isRun || isDone || isCancelled || future.isDone || future.isCancelled)
                return;
            
            _futures.Add(future);
        }

        private void OnFutureComplete(IFuture future)
        {
            _futures.Remove(future);
            future.RemoveListener(OnFutureComplete);

            if (_futures.Count > 0)
            {
                _futures[0].Run();
                _futures[0].AddListener(FutureCompletionState.Both, OnFutureComplete);
                return;
            }
            
            isDone = true;
            isRun = false;

            CallHandlers();
            CallFinalizeHandlers();
        }

        public override IFuture Run()
        {
            if (isRun) return this;
            isRun = true;
            CallRunHandlers();
            isDone = _futures.Count == 0;
            
            if (isDone)
            {
                isRun = false;
                CallHandlers();
                CallFinalizeHandlers();
            }
            else
            {
                _futures[0].Run();
                _futures[0].AddListener(FutureCompletionState.Both, OnFutureComplete);

            }
            
            return this;
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

            var copy = GetFuturesCopy();
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

            var copy = GetFuturesCopy();
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
    }
}