using System;
using System.ComponentModel;

namespace AlbanianXrm.EarlyBound
{
    [Serializable]

    public class OrganizationOptions
    {

        public OrganizationOptions() { }

        [DisplayName("Organization")]
        [Description("Current organization")]
        [ReadOnly(true)]
        public string Key { get; set; }

        [Category("General")]
        [DisplayName("Namespace")]
        [Description("Generated namespace")]
        public string Namespace { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
