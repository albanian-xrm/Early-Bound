using AlbanianXrm.EarlyBound.Helpers;
using System.ComponentModel;
using System.Drawing.Design;

namespace AlbanianXrm.EarlyBound
{
    public class Options
    {
        [Category("General")]
        [DisplayName("Output")]
        [Description("Output path")]
        [Editor(typeof(FilePathEditor), typeof(UITypeEditor))]
        public string Output { get; set; }

        [Category("General")]
        [DisplayName("Namespace")]
        [Description("Generated namespace")]
        public string Namespace { get; set; }

        [Category("General")]
        [DisplayName("Language")]
        [Description("Output file language")]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public LanguageEnum Language { get; set; }      
    }
}
