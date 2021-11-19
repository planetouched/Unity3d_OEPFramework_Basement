using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures.Util.ThreadSafe;

namespace Basement.OEPFramework.Futures.Util
{
    public class SemaphoreFuture : Future
    {
        private int _simultaneously;
        private Queue<IFuture> _futures;
        private FutureWatcher _futureWatcher;
        
        public SemaphoreFuture(int simultaneously, params IFuture[] futures)
        {
            Init(simultaneously, futures);
        }

        public SemaphoreFuture(int simultaneously, IEnumerable<IFuture> futures)
        {
            Init(simultaneously, futures);
        }

        private void Init(int simultaneously, IEnumerable<IFuture> futures) 
        {
            _simultaneously = simultaneously;
            _futures = new Queue<IFuture>(futures);
        }

        private void RunNextFuture()
        {
            var f = _futures.Dequeue();
            _futureWatcher.AddFuture(f);
            f.AddListener(FutureCompletionState.Both, FutureComplete);
            f.Run();
        }
        
        protected override void OnRun()
        {
            _futureWatcher = new FutureWatcher();
            
            for (int i = 0; i < Math.Min(_simultaneously, _futures.Count) ; i++)
            {
                RunNextFuture();
            }
        }

        private void FutureComplete(IFuture obj)
        {
            if (isCancelled) return;

            if (_futures.Count == 0)
            {
                if (_futureWatcher.futuresCount == 0)
                {
                    Complete();
                }
                return;
            }
            
            RunNextFuture();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                _futureWatcher.CancelFutures();
            }
        }
    }
}