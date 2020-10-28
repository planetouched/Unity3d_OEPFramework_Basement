using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Behaviour;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class WaitFuture : Future, IPlayable
    {
        private readonly float _sec;
        private Timer _waitTimer;
        private int _engineTimerLoop;
        
        public WaitFuture(float sec, int engineTimerLoop = -1)
        {
            _sec = sec;
            _engineTimerLoop = engineTimerLoop;
        }

        protected override void OnRun()
        {
            _waitTimer = Timer.Create(_sec, Complete, null, true, _engineTimerLoop);
        }

        protected override void OnComplete()
        {
            if (_waitTimer != null)
                _waitTimer.Drop();
        }

        public void Pause()
        {
            if (_waitTimer != null)
                _waitTimer.Pause();
        }

        public void Play()
        {
            if (_waitTimer != null)
                _waitTimer.Resume();
        }
    }
}
