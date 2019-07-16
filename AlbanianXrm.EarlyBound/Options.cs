using AlbanianXrm.EarlyBound.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml.Serialization;

namespace AlbanianXrm.EarlyBound
{
    public class Options
    {
        public Options()
        {
            OrganizationOptions = new Dictionary<string, OrganizationOptions>();
        }

        [Category("General")]
        [DisplayName("Output")]
        [Description("Output path")]
        [Editor(typeof(FilePathEditor), typeof(UITypeEditor))]
        public string Output { get; set; }

        [Category("General")]
        [DisplayName("Language")]
        [Description("Output file language")]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public LanguageEnum Language { get; set; }

        [Category("Organization")]
        [DisplayName("Organization Options")]
        [Description("Current organization options")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [XmlIgnore]
        public OrganizationOptions CurrentOrganizationOptions { get; set; }

        [Browsable(false)]
        public OrganizationOptions[] OrganizationOptionsList
        {
            get => OrganizationOptions.Values.ToArray();
            set
            {
                OrganizationOptions = new Dictionary<string, OrganizationOptions>();
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        if (OrganizationOptions.ContainsKey(item.Key))
                        {
                            OrganizationOptions[item.Key] = item;
                        }
                        else
                        {
                            OrganizationOptions.Add(item.Key, item);
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public Dictionary<string, OrganizationOptions> OrganizationOptions { get; set; }
    }
}
