using System;

namespace AlbanianXrm.Extensions
{
    internal static class StringExtensions
    {
        public static string[] Split(this string value, string separator, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return value.Split(new string[] { separator }, splitOptions);
        }
    }
}
