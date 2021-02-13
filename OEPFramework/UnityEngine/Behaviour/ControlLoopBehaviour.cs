namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public abstract class ControlLoopBehaviour : LoopBehaviour, IControllable
    {
        public bool initialized { get; private set; }

        protected virtual void OnUninitialize() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnPause() { }
        protected virtual void OnPlay() {}

        protected ControlLoopBehaviour()
        {
            callActions = false;
        }
        
        public void Initialize()
        {
            if (initialized || dropped) return;
            initialized = true;
            OnInitialize();
        }
        
        public void Uninitialize()
        {
            if (!initialized || dropped) return;
            initialized = false;
            Pause();
            OnUninitialize();
        }

        public void Pause()
        {
            if (!callActions || dropped) return;
            callActions = false;
            OnPause();
        }

        public void Play()
        {
            if (callActions || dropped) return;
            callActions = true;
            OnPlay();
        }

        public override void Drop()
        {
            if (dropped) return;
            Uninitialize();
            base.Drop();
        }

        public override void Reset()
        {
            base.Reset();
            initialized = false;
            callActions = false;
        }
    }
}