using AlbanianXrm.EarlyBound.Helpers;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace AlbanianXrm.EarlyBound
{
    [Serializable]
    public class OrganizationOptions
    {
        private static class Defaults
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
        [DefaultValue(Language.CS)]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public Language Language { get; set; }

        [Category("General")]
        [DisplayName("Remove PropertyChanged")]
        [Description("Remove INotifyPropertyChanging and INotifyPropertyChanged Interfaces from generated code.")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool RemovePropertyChanged { get; set; }

        [Category("General")]
        [DisplayName("Remove Prefix")]
        [Description("Remove Publisher Prefix from Entities, Attributes and Optionsets.")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool RemovePublisherPrefix { get; set; }

        [Category("General")]
        [DisplayName("Xml Documentation")]
        [Description("Generate Xml documentation from metadata description.")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool GenerateXmlDocumentation { get; set; }

        [Category("General")]
        [DisplayName("Attribute Constants")]
        [Description("Generate Attributes as Constants in the class Fields.")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool GenerateAttributeConstants { get; set; }

        [Category("OptionSet Enums")]
        [DisplayName("Generate Enums")]
        [Description("Generate OptionSet Enums.")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool OptionSetEnums { get; set; }

        [Category("OptionSet Enums")]
        [DisplayName("Generate TwoOptions")]
        [Description("Generate Enumerations or Constants for TwoOptions attribute.")]          
        [DefaultValue(TwoOptions.NO)]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public TwoOptions TwoOptions { get; set; }

        [Category("OptionSet Enums")]
        [DisplayName("Enum Properties")]
        [Description("Generate Enum Properties")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool OptionSetEnumProperties { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
