using System;

namespace Basement.BLFramework.Core.Factories
{
    public class DefaultFactory : IDefaultFactory
    {
        public virtual object Build(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public virtual object Build<T1>(Type type, T1 param1)
        {
            return Activator.CreateInstance(type, param1);
        }

        public virtual object Build<T1, T2>(Type type, T1 param1, T2 param2)
        {
            return Activator.CreateInstance(type, param1, param2);
        }

        public virtual object Build<T1, T2, T3>(Type type, T1 param1, T2 param2, T3 param3)
        {
            return Activator.CreateInstance(type, param1, param2, param3);
        }

        public virtual object Build<T1, T2, T3, T4>(Type type, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            return Activator.CreateInstance(type, param1, param2, param3, param4);
        }

        public virtual object Build<T1, T2, T3, T4, T5>(Type type, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            return Activator.CreateInstance(type, param1, param2, param3, param4, param5);
        }
    }
}
