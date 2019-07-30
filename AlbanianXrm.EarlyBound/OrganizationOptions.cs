using AlbanianXrm.EarlyBound.Helpers;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace AlbanianXrm.EarlyBound
{
    [Serializable]

    public class OrganizationOptions
    {
        public static class Defaults
        {
            public const string Output = "Test.cs";
        }

        public OrganizationOptions() { }

        [DisplayName("Organization")]
        [Description("Current organization")]
        [ReadOnly(true)]
        public string Key { get; set; }

        [Category("General")]
        [DisplayName("Namespace")]
        [Description("Generated namespace")]
        public string Namespace { get; set; }

        [Category("General")]
        [DisplayName("Service Context")]
        [Description("Serviec Context Name")]
        public string ServiceContextName { get; set; }

        private string _Output;

        [Category("General")]
        [DisplayName("Output")]
        [Description("Output path")]
        [DefaultValue(Defaults.Output)]
        [Editor(typeof(FilePathEditor), typeof(UITypeEditor))]
        public string Output
        {
            get { return _Output ?? Defaults.Output; }
            set { _Output = string.IsNullOrEmpty(value) ? Defaults.Output : value; }
        }

        [Category("General")]
        [DisplayName("Language")]
        [Description("Output file language")]
        [DefaultValue(LanguageEnum.CS)]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public LanguageEnum Language { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
