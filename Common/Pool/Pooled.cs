#if REFVIEW
using Basement.Common.Util;
#endif

namespace Basement.Common.Pool
{
    public abstract class Pooled :
    #if REFVIEW
        ReferenceCounter,
    #endif
    IPooled
    {
        private readonly WeakRef<IObjectPool> _weakPool;

        protected Pooled(IObjectPool pool)
        {
            _weakPool = new WeakRef<IObjectPool>(pool);
        }

        public virtual void Release()
        {
            ToInitialState();
            if (_weakPool.isAlive)
                _weakPool.obj.ReturnObj(this);
        }

        public abstract void ToInitialState();
    }
}
