using System;
using Basement.OEPFramework.UnityEngine.Behaviour;
using Basement.OEPFramework.UnityEngine.Loop;
using Basement.OEPFramework.UnityEngine.Loop.TimeData;

namespace Basement.OEPFramework.UnityEngine.Futures
{
    public class UpdateLoopFuture : FutureBehaviour
    {
        private Action<UpdateLoopFuture, ITimeData> _updateAction;
        
        public UpdateLoopFuture(Action<UpdateLoopFuture, ITimeData> updateAction)
        {
            _updateAction = updateAction;
        }
        
        protected override void OnRun()
        {
            LoopOn(Loops.UPDATE, Update);
            Play();
        }

        private void Update(ITimeData timeData)
        {
            _updateAction(this, timeData);
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
