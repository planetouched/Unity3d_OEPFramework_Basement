using System;
using System.Collections;
using System.Collections.Generic;

namespace Basement.Common
{
    public class RawNode
    {
        public static readonly RawNode emptyNode = new RawNode();
        
        public string nodeKey { get; }
        public int nodesCount => IsArray() ? array.Count : dictionary.Count;

        private readonly object _rawData;
        private KeyValuePair<string, RawNode>[] _sortedCache;
        private KeyValuePair<string, RawNode>[] _unsortedCache;

        public RawNode(object rawData = null, string nodeKey = null)
        {
            _rawData = rawData;
            this.nodeKey = nodeKey;
        }

        public object GetRawData()
        {
            return _rawData;
        }

        public void SetValue<T>(string key, T value)
        {
            dictionary[key] = value;
        }

        #region getNode
        public virtual int GetInt(string key, int defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToInt32(value);
            return defaultValue;
        }
        
        public int GetInt16(string key, int defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToInt16(value);
            return defaultValue;
        }
        
        public DateTime GetDateTime(string key)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToDateTime(value);
            return DateTime.Now;
        }
        
        public DateTime[] GetDateTimeArray(string key, DateTime[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new DateTime[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);
            return value == null ? defaultValue : new RawNode(value).GetDateTimeArray();
        }
        
        public DateTime[] GetDateTimeArray()
        {
            if (_rawData == null) return new DateTime[0];
            var ret = new List<DateTime>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToDateTime(e));
            return ret.ToArray();
        }
        
        public long[] GetLongArray(string key, long[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new long[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);
            return value == null ? defaultValue : new RawNode(value).GetLongArray();
        }
        public long[] GetLongArray()
        {
            if (_rawData == null) return new long[0];
            var ret = new List<long>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToInt64(e));
            return ret.ToArray();
        }
        public bool[] GetBoolArray(string key, bool[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new bool[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);
            return value == null ? defaultValue : new RawNode(value).GetBoolArray();
        }
        public bool[] GetBoolArray()
        {
            if (_rawData == null) return new bool[0];
            var ret = new List<bool>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToBoolean(e));
            return ret.ToArray();
        }
        public uint GetUInt(string key, uint defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToUInt32(value);
            return defaultValue;
        }

        public virtual long GetLong(string key, long defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return (long)value;
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToSingle(value);
            return defaultValue;
        }

        public double GetDouble(string key, double defaultValue = 0)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToDouble(value);
            return defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return value != null ? value.ToString() : defaultValue;
            return defaultValue;
        }

        public bool IsString(string key)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return value is string;
            return false;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            object value;
            if (_rawData != null && dictionary.TryGetValue(key, out value))
                return Convert.ToBoolean(value);
            return defaultValue;
        }
        #endregion

        #region getSelf
        public int ToInt()
        {
            return _rawData != null ? Convert.ToInt32(_rawData) : 0;
        }

        public long ToLong()
        {
            return _rawData != null ? (long)_rawData : 0;
        }
        
        public float ToFloat()
        {
            return _rawData != null ? Convert.ToSingle(_rawData) : 0;
        }
        
        public double ToDouble()
        {
            return _rawData != null ? Convert.ToDouble(_rawData) : 0;
        }

        public override string ToString()
        {
            return _rawData != null ? _rawData.ToString() : "";
        }
        #endregion

        #region getArray
        public int[] GetIntArray(string key, int[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new int[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);
            return value == null ? defaultValue : new RawNode(value).GetIntArray();
        }

        public int[] GetIntArray()
        {
            if (_rawData == null) return new int[0];
            var ret = new List<int>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToInt32(e));
            return ret.ToArray();
        }

        public uint[] GetUIntArray(string key, uint[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new uint[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);
            return value == null ? defaultValue : new RawNode(value).GetUIntArray();
        }

        public uint[] GetUIntArray()
        {
            if (_rawData == null) return new uint[0];
            var ret = new List<uint>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToUInt32(e));
            return ret.ToArray();
        }

        public float[] GetFloatArray(string key, float[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new float[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);

            return value == null ? defaultValue : new RawNode(value).GetFloatArray();
        }

        public float[] GetFloatArray()
        {
            if (_rawData == null) return new float[0];
            var ret = new List<float>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToSingle(e));
            return ret.ToArray();
        }

        public double[] GetDoubleArray(string key, double[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new double[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);

            return value == null ? defaultValue : new RawNode(value).GetDoubleArray();
        }

        public double[] GetDoubleArray()
        {
            if (_rawData == null) return new double[0];
            var ret = new List<double>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToDouble(e));
            return ret.ToArray();
        }

        public string[] GetStringArray(string key, string[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new string[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);

            return value == null ? defaultValue : new RawNode(value).GetStringArray();
        }

        public string[] GetStringArray()
        {
            if (_rawData == null) return new string[0];
            var ret = new List<string>();
            foreach (var e in (List<object>)_rawData)
                ret.Add(Convert.ToString(e));
            return ret.ToArray();
        }

        public Dictionary<string, object> [] GetObjectArray(string key, Dictionary<string, object>[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new Dictionary<string, object>[0];
            object value = null;
            if (_rawData != null)
                dictionary.TryGetValue(key, out value);

            return value == null ? defaultValue : new RawNode(value).GetObjectArray();
        }

        public Dictionary<string, object>[] GetObjectArray()
        {
            if (_rawData == null)
                return new Dictionary<string, object>[0];

            var ret = new List<Dictionary<string, object>>();

            foreach (var e in (List<object>) _rawData)
            {
                ret.Add((Dictionary<string, object>) e);
            }

            return ret.ToArray();
        }

        public RawNode[] GetRawNodeArray(string key, RawNode[] defaultValue = null)
        {
            defaultValue = defaultValue ?? new RawNode[0];

            object value = null;

            if (_rawData != null)
            {
                dictionary.TryGetValue(key, out value);
            }

            return value == null ? defaultValue : new RawNode(value).GetRawNodeArray();
        }

        public RawNode[] GetRawNodeArray(RawNode[] defaultValue = null)
        {
            if (_rawData == null)
            {
                return new RawNode[0];
            }

            var ret = new List<RawNode>();

            foreach (var e in (List<object>) _rawData)
            {
                ret.Add(new RawNode(e));
            }

            return ret.ToArray();
        }

        #endregion

        public IEnumerable<string> GetSortedKeys()
        {
            foreach (var pair in GetSortedCollection())
            {
                yield return pair.Key;
            }
        }

        public IEnumerable<string> GetUnsortedKeys()
        {
            foreach (var pair in GetUnsortedCollection())
            {
                yield return pair.Key;
            }
        }

        public IEnumerable<KeyValuePair<string, RawNode>> GetSortedCollection()
        {
            if (dictionary != null)
            {
                if (_sortedCache == null)
                {
                    var keys = new string[dictionary.Count];
                    _sortedCache = new KeyValuePair<string, RawNode>[keys.Length];
                    dictionary.Keys.CopyTo(keys, 0);
                    Array.Sort(keys, StringComparer.InvariantCulture);

                    for (int i = 0; i < keys.Length; i++)
                    {
                        var key = keys[i];
                        _sortedCache[i] = new KeyValuePair<string, RawNode>(key, new RawNode(dictionary[key]));
                    }
                }

                foreach (var pair in _sortedCache)
                    yield return pair;
            }
        }

        public IEnumerable<KeyValuePair<string, RawNode>> GetUnsortedCollection()
        {
            if (dictionary != null)
            {
                if (_unsortedCache == null)
                {
                    _unsortedCache = new KeyValuePair<string, RawNode>[dictionary.Count];
                    int idx = 0;
                    foreach (var pair in dictionary)
                        _unsortedCache[idx++] = new KeyValuePair<string, RawNode>(pair.Key, new RawNode(pair.Value));
                }

                foreach (var pair in _unsortedCache)
                    yield return pair;
            }
        }
        
        public RawNode GetNode(string key)
        {
            if (_rawData != null)
            {
                object value;
                if (dictionary.TryGetValue(key, out value))
                {
                    return new RawNode(value, key);
                }
            }

            return new RawNode(null, key);
        }

        public bool IsInit()
        {
            return _rawData != null;
        }
        
        public bool CheckKey(string key)
        {
            return dictionary != null && dictionary.ContainsKey(key);
        }

        public RawNode GetNode(int index)
        {
            return _rawData != null ? new RawNode(array[index]) : emptyNode;
        }

        public bool IsDictionary()
        {
            return _rawData is Dictionary<string, object>;
        }
        
        public bool IsArray()
        {
            return _rawData is List<object>;
        }

        public List<object> array
        {
            get
            {
                if (_rawData == null)
                {
                    return new List<object>();
                }

                return (List<object>)_rawData;
            }
        }

        public Dictionary<string, object> dictionary 
        {
            get
            {
                if (_rawData == null)
                {
                    return new Dictionary<string, object>();
                }

                return (Dictionary<string, object>)_rawData;
            }
        }

        public RawNode Concatenate(RawNode node)
        {
            if (node == null)
                return this;

            _sortedCache = null;
            _unsortedCache = null;

            var toAddDictionary = dictionary;
            var addDictionary = node.dictionary;
            var keys = addDictionary.Keys;
            foreach (var k in keys)
            {
                if (!toAddDictionary.ContainsKey(k))
                    toAddDictionary.Add(k, addDictionary[k]);
            }
            return new RawNode(toAddDictionary);
        }
    }
}
