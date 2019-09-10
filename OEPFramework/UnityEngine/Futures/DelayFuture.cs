using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Behaviour;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class DelayFuture : Future, IPlayable
    {
        private readonly float _delay;
        private readonly IFuture _delayFuture;
        private Timer _timer;
        public DelayFuture(float delay, IFuture delayFuture)
        {
            _delay = delay;
            _delayFuture = delayFuture;
        }
        protected override void OnRun()
        {
            _delayFuture.AddListener(f =>
            {
                if (f.isDone)
                    Complete();
            });

            _timer = Timer.Create(_delay, () =>
            {
                _delayFuture.Run();
            }, null, true);
        }

        protected override void OnComplete()
        {
            if (isCancelled)
            {
                _delayFuture.Cancel();
            }

            if (_timer != null)
                _timer.Drop();
        }

        public void Pause()
        {
            if (_timer != null)
                _timer.Pause();
        }

        public void Play()
        {
            if (_timer != null)
                _timer.Resume();
        }
    }
}
