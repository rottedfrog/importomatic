using System.Collections.Generic;

namespace ImportOMatic3000
{
    static class DictionaryHelper
    {
        public static IDictionary<string, string> ToVariableDictionary(this IDictionary<string, string> dict)
        {
            return new VariableDictionary(dict);
        }

    }
}