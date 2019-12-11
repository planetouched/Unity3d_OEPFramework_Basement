using System;

namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public interface ILoopable
    {
        void LoopOn(int loopType, Action action, bool callNow = false);
        void LoopOff(int loopType);
        /*
        void SetIndexToLast(int loopType);
        void SwapWith(LoopBehaviour target, int loopType);
        void SetIndexToFirst(int loopType);*/
    }
}
