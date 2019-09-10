using System;
using System.Collections.Generic;

namespace Basement.Common.Util
{
    public abstract class ReferenceCounter
    {
        private static readonly Dictionary<Type, int> _references = new Dictionary<Type, int>();
        private static readonly object _syncRoot = new object();
        private readonly Type _classType;

        public static int GetCount(Type type)
        {
            lock (_syncRoot)
            {
                if (!_references.ContainsKey(type)) return 0;
                return _references[type];
            }
        }

        protected ReferenceCounter()
        {
            lock (_syncRoot)
            {
                _classType = GetType();
                if (!_references.ContainsKey(_classType))
                {
                    _references.Add(_classType, 0);
                }
                _references[_classType]++;

                foreach (var baseType in GetBaseTypes(_classType))
                {
                    if (!_references.ContainsKey(baseType))
                        _references.Add(baseType, 0);

                    _references[baseType]++;
                }
            }
        }

        IEnumerable<Type> GetBaseTypes(Type type)
        {
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }

        ~ReferenceCounter()
        {
            lock (_syncRoot)
            {
                _references[_classType]--;
                foreach (var baseType in GetBaseTypes(_classType))
                {
                    _references[baseType]--;
                }
            }
        }
    }
}
