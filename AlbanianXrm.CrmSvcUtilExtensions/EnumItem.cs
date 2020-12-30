using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    /// <remarks>
    /// Most of the code in this class is taken from https://github.com/mjfpalmer/CRMSvcUtilGenerateOptionSetEnums
    /// </remarks>
    internal class EnumItem
    {
        public EnumItem(int key, string value, string description)
        {
            this.Key = key;
            this.Value = ToValidIdentifier(value);
            this.Description = description;
        }

        public int Key { get; }

        public string Value { get; private set; }

        public string Description { get; }

        internal static IEnumerable<EnumItem> ToUniqueValues(OptionMetadataCollection optionMetadataCollection)
        {
            return ToUniqueValues(optionMetadataCollection.Select(omc => new EnumItem(omc.Value.Value, omc.Label.UserLocalizedLabel.Label, omc.Label.UserLocalizedLabel.Label)).ToList());
        }

        internal static IEnumerable<EnumItem> ToUniqueValues(BooleanOptionSetMetadata booleanOptionSetMetadata)
        {
            List<EnumItem> values = new List<EnumItem>();
            if (booleanOptionSetMetadata.FalseOption == null)
            {
                values.Add(new EnumItem(0, "No", "No"));
            }
            else
            {
                values.Add(new EnumItem(booleanOptionSetMetadata.FalseOption.Value.Value, booleanOptionSetMetadata.FalseOption.Label.UserLocalizedLabel.Label, booleanOptionSetMetadata.FalseOption.Label.UserLocalizedLabel.Label));
            }

            if (booleanOptionSetMetadata.TrueOption == null)
            {
                values.Add(new EnumItem(1, "Yes", "Yes"));
            }
            else
            {
                values.Add(new EnumItem(booleanOptionSetMetadata.TrueOption.Value.Value, booleanOptionSetMetadata.TrueOption.Label.UserLocalizedLabel.Label, booleanOptionSetMetadata.TrueOption.Label.UserLocalizedLabel.Label));
            }
            return ToUniqueValues(values);
        }

        private static IEnumerable<EnumItem> ToUniqueValues(IEnumerable<EnumItem> source)
        {
            Dictionary<string, int> uniqueValues = source.GroupBy(sv => sv.Value).ToDictionary(g => g.Key, g => g.Count());
            source.ToList().ForEach(sv => sv.Value = uniqueValues[sv.Value] == 1 ? sv.Value : string.Format(CultureInfo.InvariantCulture, "{0}_{1}", sv.Value, sv.Key.ToString(CultureInfo.InvariantCulture)));
            return source;
        }

        private static string ToValidIdentifier(string source)
        {
            StringBuilder result = new StringBuilder();

            source = ReplaceSpecial(source ?? string.Empty);
            
            if (string.IsNullOrWhiteSpace(source))
            {
                return "None";
            }

            foreach (Match m in Regex.Matches(source, "[A-Za-z0-9]"))
            {
                result.Append(m.ToString());
            }

            if (result.Length == 0)
            {
                return "None";
            }

            if (char.IsDigit(result[0]))
            {
                result.Insert(0, "_");
            }

            return result.ToString();
        }

        private static string ReplaceSpecial(string value)
        {
            return value.Replace("<>", "NotEquals").Replace("!=", "NotEquals").Replace("=", "Equals").Replace("<", "LessThan").Replace(">", "GreaterThan").Replace("+", "Plus");
        }
    }
}
