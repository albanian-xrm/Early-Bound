using AlbanianXrm.EarlyBound.Extensions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class DescriptionEnumConverter : EnumConverter
    {
        private readonly Type target;

        public DescriptionEnumConverter(Type target) : base(target)
        {
            this.target = target;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(string).Equals(sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return typeof(string).Equals(destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (var fieldInfo in target.GetFields().Where(x => x.AttributeIsDefined<DescriptionAttribute>()))
            {
                var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if ((string)value == description.Description)
                    return Enum.Parse(target, fieldInfo.Name);
            }
            return Enum.Parse(target, (string)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var fieldInfo = target.GetField(Enum.GetName(target, value));
            var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            if (description != null)
                return description.Description;
            else
                return value.ToString();
        }
    }
}
