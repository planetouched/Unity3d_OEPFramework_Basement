using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Coroutine
{
    public class CoroutineFuture<T1, T2, T3, T4> : CoroutineFutureBase
    {
        private Func<T1, T2, T3, T4, IEnumerator<IFuture>> _func;

        private readonly T1 param1;
        private readonly T2 param2;
        private readonly T3 param3;
        private readonly T4 param4;

        public CoroutineFuture(Func<T1, T2, T3, T4, IEnumerator<IFuture>> func, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
            _func = func;
        }

        protected override void OnRun()
        {
            enumerator = _func(param1, param2, param3, param4);
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            _func = null;
        }
    }
}
