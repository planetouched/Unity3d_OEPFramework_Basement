using System;

namespace Basement.BLFramework.Core.Factories
{
    public interface IFactory
    {
        object Build(string key);
        object Build<T1>(string key, T1 param);
        object Build<T1, T2>(string key, T1 param, T2 param2);
        object Build<T1, T2, T3>(string key, T1 param, T2 param2, T3 param3);
        object Build<T1, T2, T3, T4>(string key, T1 param, T2 param2, T3 param3, T4 param4);
        object Build<T1, T2, T3, T4, T5>(string key, T1 param, T2 param2, T3 param3, T4 param4, T5 param5);
        IFactory AddVariant(string key, Type type);
    }
}
