using System;
using System.ComponentModel;
using System.Globalization;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class YesNoConverter : BooleanConverter
    {
        public const string YES = "YES";
        public const string NO = "NO";

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is bool bValue && destinationType == typeof(string))
            {
                return bValue ? YES : NO;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string txt = value as string;
            if (YES == txt) return true;
            if (NO == txt) return false;
            return base.ConvertFrom(context, culture, value);
        }
    }
}
