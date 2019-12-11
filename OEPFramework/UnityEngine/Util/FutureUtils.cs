using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Coroutine;
using Basement.OEPFramework.Futures.Util;
using Basement.OEPFramework.UnityEngine.Futures;

namespace Basement.OEPFramework.UnityEngine.Util
{
    public static class FutureUtils
    {
        public static IFuture Run(Action method)
        {
            return new FutureTask(method);
        }

        public static IFuture Coroutine(Func<IEnumerator<IFuture>> func)
        {
            return new CoroutineFuture(func);
        }

        public static IFuture Coroutine<T1>(Func<T1, IEnumerator<IFuture>> func, T1 param1)
        {
            return new CoroutineFuture<T1>(func, param1);
        }

        public static IFuture Coroutine<T1, T2>(Func<T1, T2, IEnumerator<IFuture>> func, T1 param1, T2 param2)
        {
            return new CoroutineFuture<T1, T2>(func, param1, param2);
        }

        public static IFuture Coroutine<T1, T2, T3>(Func<T1, T2, T3, IEnumerator<IFuture>> func, T1 param1, T2 param2, T3 param3)
        {
            return new CoroutineFuture<T1, T2, T3>(func, param1, param2, param3);
        }

        public static IFuture Coroutine<T1, T2, T3, T4>(Func<T1, T2, T3, T4, IEnumerator<IFuture>> func, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            return new CoroutineFuture<T1, T2, T3, T4>(func, param1, param2, param3, param4);
        }

        public static IFuture Wait(float sec)
        {
            return new WaitFuture(sec);
        }

        public static IFuture Delay(float sec, IFuture delayFuture)
        {
            return new DelayFuture(sec, delayFuture);
        }

        public static IFuture DummyBreak()
        {
            return new DummyBreakFuture();
        }

        public static IFuture UpdateLoop(Action<UpdateLoopFuture> updateAction)
        {
            return new UpdateLoopFuture(updateAction);
        }

        public static IFuture FutureScenario(FutureScenario futureScenario)
        {
            return new FutureScenarioFuture(futureScenario);
        }
        
        public static IFuture SequenceFuture(params IFuture[] futures)
        {
            return new SequenceFuture(futures);
        }

        public static IFuture SyncLoop(int loopType)
        {
            return new SyncLoopFuture(loopType);
        }
    }
}
