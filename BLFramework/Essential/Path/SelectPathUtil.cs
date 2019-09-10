using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Basement.BLFramework.Essential.Path
{
    static class SelectPathUtil
    {
        public static bool IsSimple(string rawSelector)
        {
            return !(IsRange(rawSelector) || IsExcept(rawSelector) || IsComposite(rawSelector) || IsAll(rawSelector));
        }

        private static bool IsRange(string rawSelector)
        {
            return rawSelector.Contains("~");
        }

        private static bool IsExcept(string rawSelector)
        {
            return rawSelector.Contains("!");
        }

        private static bool IsComposite(string rawSelector)
        {
            return rawSelector.Contains(",");
        }

        private static bool IsAll(string rawSelector)
        {
            return rawSelector == "*";
        }

        static void AddRange(string range, IOrderedDictionary fastKeys, ref List<string> affectedKeys)
        {
            var tmp = range.Split('~');
            int start = Convert.ToInt32(tmp[0]);
            int end = Convert.ToInt32(tmp[1]);
            for (int i = start; i <= end; i++)
            {
                string newKey = Convert.ToString(i);
                if (fastKeys.Contains(newKey))
                    affectedKeys.Add(newKey);
            }
        }

        static void ExceptRange(string range, IOrderedDictionary fastKeys, ref List<string> affectedKeys)
        {
            var tmp = range.Split('~');
            int start = Convert.ToInt32(Trim(tmp[0]));
            int end = Convert.ToInt32(Trim(tmp[1]));
            for (int i = start; i <= end; i++)
            {
                string newKey = Convert.ToString(i);
                if (fastKeys.Contains(newKey))
                    affectedKeys.Remove(newKey);
            }
        }

        static string Trim(string key)
        {
            return key.TrimStart('!');
        }
        
        public static IList<string> GetAffectedKeys(string rawSelector, IList<string> allKeys)
        {
            if (IsAll(rawSelector))
                return allKeys;

            var fastKeys = new OrderedDictionary();
            
            foreach (var key in allKeys)
                fastKeys.Add(key, null);

            var affectedKeys = new List<string>();
            bool isExcept = IsExcept(rawSelector);
            if (isExcept)
                affectedKeys.AddRange(allKeys);


            string[] parts = rawSelector.Split(',');

            foreach (string part in parts)
            {
                bool exceptTest = IsExcept(part);

                if (isExcept != exceptTest)
                    throw new Exception("Select::GetSelector нельзя смешивать except_selector и set_selector");

                if (IsRange(part))
                {
                    if (isExcept)
                        ExceptRange(part, fastKeys, ref affectedKeys);
                    else
                        AddRange(part, fastKeys, ref affectedKeys);
                }
                else
                {
                    if (isExcept)
                    {
                        string tid = Trim(part);
                        affectedKeys.Remove(tid);
                    }
                    else
                        affectedKeys.Add(part);
                }
            }
            return affectedKeys;
        }
    }
}
