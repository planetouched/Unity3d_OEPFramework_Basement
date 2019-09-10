using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Coroutine
{
    public class CoroutineFuture<T1, T2, T3> : CoroutineFutureBase
    {
        private Func<T1, T2, T3, IEnumerator<IFuture>> _func;

        private readonly T1 param1;
        private readonly T2 param2;
        private readonly T3 param3;

        public CoroutineFuture(Func<T1, T2, T3, IEnumerator<IFuture>> func, T1 param1, T2 param2, T3 param3)
        {
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            _func = func;
        }

        protected override void OnRun()
        {
            enumerator = _func(param1, param2, param3);
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            _func = null;
        }
    }
}
