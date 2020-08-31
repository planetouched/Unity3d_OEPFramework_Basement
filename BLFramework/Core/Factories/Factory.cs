using System;
using System.Collections.Generic;

namespace Basement.BLFramework.Core.Factories
{
    public class Factory : IFactory
    {
        private readonly Dictionary<string, Type> _variantsString = new Dictionary<string, Type>();

        public virtual object Build(string key)
        {
            return Activator.CreateInstance(GetVariant(key));
        }

        public virtual object Build<T1>(string key, T1 param)
        {
            return Activator.CreateInstance(GetVariant(key), param);
        }

        public virtual object Build<T1, T2>(string key, T1 param, T2 param2)
        {
            return Activator.CreateInstance(GetVariant(key), param, param2);
        }

        public virtual object Build<T1, T2, T3>(string key, T1 param, T2 param2, T3 param3)
        {
            return Activator.CreateInstance(GetVariant(key), param, param2, param3);
        }

        public virtual object Build<T1, T2, T3, T4>(string key, T1 param, T2 param2, T3 param3, T4 param4)
        {
            return Activator.CreateInstance(GetVariant(key), param, param2, param3, param4);
        }

        public virtual object Build<T1, T2, T3, T4, T5>(string key, T1 param, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            return Activator.CreateInstance(GetVariant(key), param, param2, param3, param4, param5);
        }
        
        private Type GetVariant(string key)
        {
            Type variant;
            if (_variantsString.TryGetValue(key, out variant))
                return _variantsString[key];
            throw new Exception("No factory for " + key);
        }

        public IFactory AddVariant(string key, Type type)
        {
            _variantsString.Add(key, type);
            return this;
        }
    }
}
