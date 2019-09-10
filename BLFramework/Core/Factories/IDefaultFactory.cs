using System;

namespace Basement.BLFramework.Core.Factories
{
    public interface IDefaultFactory
    {
        object Build(Type type);
        object Build<T1>(Type type, T1 param1);
        object Build<T1, T2>(Type type, T1 param1, T2 param2);
        object Build<T1, T2, T3>(Type type, T1 param1, T2 param2, T3 param3);
        object Build<T1, T2, T3, T4>(Type type, T1 param1, T2 param2, T3 param3, T4 param4);
        object Build<T1, T2, T3, T4, T5>(Type type, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
    }
}
