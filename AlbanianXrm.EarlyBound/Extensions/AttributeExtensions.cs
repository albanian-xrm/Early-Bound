using System;
using System.Reflection;

namespace AlbanianXrm.EarlyBound.Extensions
{
    static class AttributeExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(element, typeof(T));
        }

        public static bool AttributeIsDefined<T>(this MemberInfo element) where T : Attribute
        {
            return Attribute.IsDefined(element, typeof(T));
        }
    }
}
