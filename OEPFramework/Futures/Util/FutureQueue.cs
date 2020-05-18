using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class FutureQueue : IFutureContainer
    {
        private readonly List<IFuture> _queueFutures = new List<IFuture>();
        private IFuture _current;
        public int futuresCount => _queueFutures.Count;
        public event Action<IFuture> onFutureComplete;

        public void AddFuture(IFuture future)
        {
            if (future.isDone || future.isCancelled || future.wasRun)
                throw new Exception("future already run or completed");

            _queueFutures.Add(future);
            future.AddListener(FutureComplete);
            
            if (_queueFutures.Count == 1)
            {
                _current = future;
                future.Run();
            }
        }

        private void FutureComplete(IFuture f)
        {
            _queueFutures.Remove(f);
            
            if (_queueFutures.Count > 0)
            {
                if (_current == f)
                    _current = _queueFutures[0];
            }
            else
                _current = null;

            if (onFutureComplete != null)
                onFutureComplete(f);
            
            if (_current != null)
                _current.Run();
        }

        public void CancelCurrent()
        {
            if (_current != null)
                _current.Cancel();
        }

        public void Cancel()
        {
            var copy = new List<IFuture>(_queueFutures);
            _queueFutures.Clear();

            foreach (var future in copy)
            {
                future.RemoveListener(FutureComplete);
                future.Cancel();
            }

            onFutureComplete = null;
        }
    }
}
