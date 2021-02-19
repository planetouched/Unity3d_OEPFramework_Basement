using System;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Transit;
using Basement.OEPFramework.UnityEngine._Base;

namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public abstract class FutureBehaviour : Future, IControllable, IDroppableItem, ILoopable
    {
        private readonly ControlLoopTransit _controlLoopTransit;
        public event Action<IDroppableItem> onDrop;

        public bool dropped => _controlLoopTransit.dropped;
        public bool initialized => _controlLoopTransit.initialized;

        protected FutureBehaviour()
        {
            _controlLoopTransit = new ControlLoopTransit
            {
                onPlay = OnPlay, 
                onInitialize = OnInitialize, 
                onUninitialize = OnUninitialize, 
                onPause = OnPause
            };

            AddListenerOnFinalize(f => { Drop(); });
        }

        protected virtual void OnUninitialize() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnPause() { }
        protected virtual void OnPlay() { }

        public void LoopOn(int loopType, Action action, bool callNow = false)
        {
            _controlLoopTransit.LoopOn(loopType, action, callNow);
        }

        public void LoopOff(int loopType)
        {
            _controlLoopTransit.LoopOff(loopType);
        }

        public void Initialize()
        {
            _controlLoopTransit.Initialize();
        }

        public void Uninitialize()
        {
            _controlLoopTransit.Uninitialize();
        }

        public void Pause()
        {
            _controlLoopTransit.Pause();
        }

        public void Play()
        {
            _controlLoopTransit.Play();
        }

        public virtual void Drop()
        {
            if (dropped) return;
            _controlLoopTransit.Drop();
            
            onDrop?.Invoke(this);
            onDrop = null;
        }

        public override bool Reuse()
        {
            if (!dropped) return false;

            if (_controlLoopTransit.Reuse() && base.Reuse())
            {
                AddListenerOnFinalize(f => { Drop(); });
                return true;
            }

            return false;
        }
    }
}
