using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class SequenceFuture : FutureBase
    {
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
            if (wasRun || isDone || isCancelled || future.isDone || future.isCancelled)
                return;
            
            _futures.Add(future);
            future.AddListener(OnFutureComplete);
        }

        private void OnFutureComplete(IFuture future)
        {
            _futures.Remove(future);
            future.RemoveListener(OnFutureComplete);

            if (_futures.Count > 0)
            {
                _futures[0].Run();
                return;
            }
            
            isDone = true;
            wasRun = false;

            CallHandlers();
        }

        public override IFuture Run()
        {
            if (wasRun) return this;
            wasRun = true;
            CallRunHandlers();
            isDone = _futures.Count == 0;
            
            if (isDone)
            {
                wasRun = false;
                CallHandlers();
            }
            else
            {
                _futures[0].Run();
            }
            
            return this;
        }
        
        public override void Cancel()
        {
            if (promise || isCancelled || isDone)
                return;
            isCancelled = true;
            wasRun = false;

            var copy = GetFuturesCopy();
            _futures.Clear();

            foreach (var future in copy)
            {
                if (future.isCancelled) continue;
                future.RemoveListener(OnFutureComplete);
                future.Cancel();
            }

            CallHandlers();
        }
    }
}