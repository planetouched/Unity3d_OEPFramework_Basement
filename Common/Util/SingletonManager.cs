using System;
using System.Collections.Generic;

namespace Basement.Common.Util
{
    public static class SingletonManager
    {
        static readonly Dictionary<Type, object> singletones = new Dictionary<Type, object>();

        public static T Add<T>(T singleton)
        {
            singletones.Add(typeof(T), singleton);
            return singleton;
        }

        public static T Get<T>()
        {
            return (T)singletones[typeof (T)];
        }

        public static T Remove<T>()
        {
            var obj = Get<T>();
            singletones.Remove(typeof (T));
            return obj;
        }

        public static IList<KeyValuePair<Type, object>> RemoveAll()
        {
            var list = new List<KeyValuePair<Type, object>>();

            foreach (var pair in singletones)
                list.Add(pair);

            singletones.Clear();
            return list;
        }
    }
}
