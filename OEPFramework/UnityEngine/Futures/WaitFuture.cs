using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Behaviour;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class WaitFuture : Future, IPlayable
    {
        private float _sec;
        private Timer _waitTimer;
        private int _engineTimerLoop;

        public WaitFuture()
        {
            //no op
        }
        
        public WaitFuture(float sec, int engineTimerLoop = -1)
        {
            Initialize(sec, engineTimerLoop);
        }

        public IFuture Initialize(float sec, int engineTimerLoop = -1)
        {
            _sec = sec;
            _engineTimerLoop = engineTimerLoop;
            return this;
        }

        protected override void OnRun()
        {
            _waitTimer = Timer.Create(_sec, () => Complete(), null, true, _engineTimerLoop);
        }

        protected override void OnComplete()
        {
            _waitTimer?.Drop();
            _waitTimer = null;
        }

        public void Pause()
        {
            _waitTimer?.Pause();
        }

        public void Play()
        {
            _waitTimer?.Resume();
        }
    }
}
