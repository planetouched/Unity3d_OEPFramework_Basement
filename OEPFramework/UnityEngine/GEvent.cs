using System;
using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine._Base;

namespace Basement.OEPFramework.UnityEngine
{
    public static class GEvent
    {
        static int uniqueCounter;

        public static string GetUniqueCategory()
        {
            return "#" + uniqueCounter++;
        }

        static readonly Dictionary<string, List<Action<object>>> items = new Dictionary<string, List<Action<object>>>();

        public static void Attach(string category, Action<object> method, IDroppableItem detacher)
        {
            List<Action<object>> list;
            if (items.TryGetValue(category, out list))
                list.Add(method);
            else
                items.Add(category, new List<Action<object>> { method });

            if (detacher != null)
            {
                detacher.onDrop += obj =>
                {
                    Detach(category, method);
                };
            }
        }

        public static void Detach(string category, Action<object> method)
        {
            List<Action<object>> list;
            if (!items.TryGetValue(category, out list)) return;
            list.Remove(method);

            if (list.Count == 0)
                items.Remove(category);
        }

        public static void Call(string category, object obj = null)
        {
            List<Action<object>> list;
            if (!items.TryGetValue(category, out list)) return;
            if (list.Count == 0) return;

            switch (list.Count)
            {
                case 1:
                    {
                        list[0](obj);
                        break;
                    }
                case 2:
                    {
                        var tmp0 = list[0];
                        var tmp1 = list[1];
                        tmp0(obj);
                        tmp1(obj);
                        break;
                    }
                case 3:
                    {
                        var tmp0 = list[0];
                        var tmp1 = list[1];
                        var tmp2 = list[2];
                        tmp0(obj);
                        tmp1(obj);
                        tmp2(obj);
                        break;
                    }
                default:
                    {
                        var copy = new Action<object>[list.Count];
                        list.CopyTo(copy, 0);
                        foreach (var method in copy)
                            method(obj);
                        break;
                    }
            }
        }

        public static void DetachCategory(string category)
        {
            items.Remove(category);
        }        
    }
}
