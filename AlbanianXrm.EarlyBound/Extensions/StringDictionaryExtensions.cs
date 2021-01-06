using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianXrm.EarlyBound.Extensions
{
    static class StringDictionaryExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToEnumerable(this StringDictionary stringDictionary)
        {
            foreach (string key in stringDictionary.Keys)
            {
                yield return new KeyValuePair<string, string>(key, stringDictionary[key]);
            }
        }
    }
}
