using System.Collections;

namespace Basement.OEPFramework.Futures.Coroutine
{
    public abstract class CoroutineFutureBase : Future
    {
        protected IEnumerator enumerator;
        protected IFuture currentFuture;

        protected void Next(IFuture obj)
        {
            if (isCancelled) return;
            
            if (enumerator.MoveNext())
            {
                if (enumerator != null && enumerator.Current != null)
                {
                    currentFuture = (IFuture)enumerator.Current;
                    currentFuture.AddListener(Next);
                }
            }
            else
                Complete();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                if (currentFuture != null)
                {
                    currentFuture.RemoveListener(Next);
                    currentFuture.Cancel();
                }
            }

            currentFuture = null;
            enumerator = null;
        }
    }
}
