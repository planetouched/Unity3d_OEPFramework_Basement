using System;
using Basement.OEPFramework.UnityEngine.Loop.TimeData;

namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public interface ILoopable
    {
        void LoopOn(int loopType, Action<ITimeData> action, bool callNow = false);
        void LoopOff(int loopType);
        /*
        void SetIndexToLast(int loopType);
        void SwapWith(LoopBehaviour target, int loopType);
        void SetIndexToFirst(int loopType);*/
    }
}
