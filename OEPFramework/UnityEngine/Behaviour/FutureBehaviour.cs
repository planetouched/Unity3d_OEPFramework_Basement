using System;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Transit;
using Basement.OEPFramework.UnityEngine._Base;

namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public abstract class FutureBehaviour : Future, IControllable, IDroppableItem, ILoopable
    {
        private readonly int _hashCode;

        private readonly ControlLoopTransit _controlLoopTransit;
        public event Action<IDroppableItem> onDrop;

        public bool dropped => _controlLoopTransit.dropped;
        public bool initialized => _controlLoopTransit.initialized;

        protected FutureBehaviour()
        {
            _hashCode = DroppableItemBase.globalHashCode++;
            _controlLoopTransit = new ControlLoopTransit();
            _controlLoopTransit.onDrop += onDrop;
            _controlLoopTransit.onPlay = OnPlay;
            _controlLoopTransit.onInitialize = OnInitialize;
            _controlLoopTransit.onUninitialize = OnUninitialize;
            _controlLoopTransit.onPause = OnPause;

            AddListener(f => { Drop(); });
        }

        protected virtual void OnUninitialize() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnPause() { }
        protected virtual void OnPlay() { }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public void LoopOn(int loopType, Action action)
        {
            _controlLoopTransit.LoopOn(loopType, action);
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

            if (onDrop != null)
                onDrop(this);
            onDrop = null;
        }
    }
}
