using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class FutureQueue : IFutureContainer
    {
        private readonly Queue<IFuture> _queueFutures = new Queue<IFuture>();
        private IFuture _current;
        public int futuresCount => _queueFutures.Count;
        public event Action<IFuture> onFutureComplete;
        public event Action onAllFutureComplete;
        
        public void AddFuture(IFuture future)
        {
            if (future.isDone || future.isCancelled || future.isRun)
                throw new Exception("future already run or completed");

            _queueFutures.Enqueue(future);
            future.AddListener(FutureCompletionState.Both, FutureComplete);
            
            if (_queueFutures.Count == 1)
            {
                _current = future;
                future.Run();
            }
        }

        private void FutureComplete(IFuture f)
        {
            _queueFutures.Dequeue();
            _current = null;
            
            if (_queueFutures.Count > 0)
            {
                _current = _queueFutures.Peek();
            }
            else
            {
                onAllFutureComplete?.Invoke();
            }
            onFutureComplete?.Invoke(f);
            _current?.Run();
        }

        public void CancelCurrent()
        {
            _current?.Cancel();
        }

        public void Cancel()
        {
            foreach (var future in new List<IFuture>(_queueFutures))
            {
                future.RemoveListener(FutureComplete);
                future.Cancel();
            }
            
            _queueFutures.Clear();
        }
    }
}
