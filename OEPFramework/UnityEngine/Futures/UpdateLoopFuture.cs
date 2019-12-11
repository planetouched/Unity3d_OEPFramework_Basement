using System;
using Basement.OEPFramework.UnityEngine.Behaviour;
using Basement.OEPFramework.UnityEngine.Loop;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class UpdateLoopFuture : FutureBehaviour
    {
        private Action<UpdateLoopFuture> _updateAction;
        
        public UpdateLoopFuture(Action<UpdateLoopFuture> updateAction)
        {
            _updateAction = updateAction;
        }
        
        protected override void OnRun()
        {
            LoopOn(Loops.UPDATE, Update);
            Play();
        }

        private void Update()
        {
            _updateAction(this);
        }

        protected override void OnComplete()
        {
            Drop();
        }

        public override void Drop()
        {
            if (dropped) return;
            _updateAction = null;
            base.Drop();
        }
    }
}
