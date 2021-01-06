using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace AlbanianXrm.EarlyBound.Extensions
{
    static class MirrorExtensions
    {
        public const string EXPRESSION_REFERS_METHOD = "Expression '{0}' refers to a method, not a property.";
        public const string EXPRESSION_REFERS_FIELD = "Expression '{0}' refers to a method, not a property.";
        public const string EXPRESSION_REFERS_OTHER_PROPERTY = "Expression '{0}' refers to a property that is not from type {1}.";

        public static void SetPrivateValue<TSource, TProperty>(this TSource target, Expression<Func<TSource, TProperty>> propertyLambda, object value)
        {
            Type type = typeof(TSource);

            var propInfo = propertyLambda.GetProperty();

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        EXPRESSION_REFERS_OTHER_PROPERTY,
                        propertyLambda.ToString(),
                        type));

            type.GetProperty(propInfo.Name).SetValue(target, value);
        }

        public static PropertyInfo GetProperty<TObject, TProperty>(this Expression<Func<TObject, TProperty>> propertyLambda)
        {
            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException(
                    string.Format(
                        EXPRESSION_REFERS_METHOD,
                        propertyLambda.ToString()));

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException(
                    string.Format(
                        EXPRESSION_REFERS_FIELD,
                        propertyLambda.ToString()));

            return propInfo;
        }
    }
}
