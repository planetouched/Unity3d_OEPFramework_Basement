using System.Collections.Concurrent;
using System.Linq;

namespace Basement.OEPFramework.Futures.Util.ThreadSafe
{
    public class FutureWatcher
    {
        private readonly ConcurrentDictionary<IFuture, bool> _futures = new ConcurrentDictionary<IFuture, bool>(); 
        public IFuture[] futures => _futures.Keys.ToArray();

        public int futuresCount => _futures.Count;

        public void AddFuture(IFuture future)
        {
            if (future == null) return;

            if (_futures.TryAdd(future, true))
            {
                future.AddListener(InnerRemoveFuture);
            }
        }

        private void InnerRemoveFuture(IFuture future)
        {
            if (_futures.TryRemove(future, out _))
            {
                future.RemoveListener(InnerRemoveFuture);
            }
        }

        public void CancelFutures()
        {
            foreach (var pair in _futures)
            {
                pair.Key.RemoveListener(InnerRemoveFuture);
                pair.Key.Cancel();
            }
        }    
    }
}
