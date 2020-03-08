using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace AlbanianXrm.EarlyBound.Extensions
{
    static class MirrorExtensions
    {
        public static void SetPrivateValue<TSource, TProperty>(this TSource target, Expression<Func<TSource, TProperty>> propertyLambda, object value)
        {
            Type type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            type.GetProperty(propInfo.Name).SetValue(target, value);
        }
    }
}
