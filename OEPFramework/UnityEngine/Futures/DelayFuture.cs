using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Behaviour;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class DelayFuture : Future, IPlayable
    {
        private float _delay;
        private IFuture _delayedFuture;
        private Timer _timer;

        public DelayFuture()
        {
        }
        
        public DelayFuture(float delay, IFuture delayedFuture)
        {
            Initialize(delay, delayedFuture);
        }

        public IFuture Initialize(float delay, IFuture delayedFuture)
        {
            _delay = delay;
            _delayedFuture = delayedFuture;
            return this;
        }
        
        protected override void OnRun()
        {
            _delayedFuture.AddListener(FutureCompletionState.Done, f => 
            {
                Complete();
            });

            _timer = Timer.Create(_delay, () =>
            {
                _delayedFuture.Run();
            }, null, true);
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                _delayedFuture.Cancel();
            }

            _timer?.Drop();

            _timer = null;
            _delayedFuture = null;
            _delay = 0;
        }

        public void Pause()
        {
            _timer?.Pause();
        }

        public void Play()
        {
            _timer?.Resume();
        }
    }
}
