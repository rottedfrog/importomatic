using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ImportOMatic3000
{
    public class VariableDictionary : IDictionary<string, string>
    {
        readonly IDictionary<string, string> _dict;
        public VariableDictionary(IDictionary<string, string> dict)
        {
            _dict = dict;
        }

        public string this[string key]
        {
            get
            {
                if (!_dict.ContainsKey(key))
                { throw new KeyNotFoundException("Could not find a field or variable named {key}."); }
                return _dict[key];
            }
            set => _dict[key] = value;
        }

        public ICollection<string> Keys => _dict.Keys;

        public ICollection<string> Values => _dict.Values;

        public int Count => _dict.Count;

        public bool IsReadOnly => _dict.IsReadOnly;

        public void Add(string key, string value) => _dict.Add(key, value);

        public void Add(KeyValuePair<string, string> item) => _dict.Add(item);

        public void Clear() => _dict.Clear();

        public bool Contains(KeyValuePair<string, string> item) => _dict.Contains(item);

        public bool ContainsKey(string key) => _dict.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => _dict.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _dict.GetEnumerator();

        public bool Remove(string key) => _dict.Remove(key);

        public bool Remove(KeyValuePair<string, string> item) => _dict.Remove(item);

        public bool TryGetValue(string key, out string value) => _dict.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    }

    static class TypeHelper
    {
        public static string TypeName(Type t)
        {
            if (t == typeof(string))
            { return "text"; }
            if (t == typeof(DateTime))
            { return "date"; }
            if (t == typeof(Decimal))
            { return "number"; }
            if (t == typeof(bool))
            { return "boolean"; }
            return t.Name;
        }
    }
}