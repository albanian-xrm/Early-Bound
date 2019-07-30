using AlbanianXrm.EarlyBound.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AlbanianXrm.EarlyBound
{
    public class Options : INotifyPropertyChanged
    {
        public static class Defaults
        {
            public const string NuGetFeed = "https://packages.nuget.org/api/v2";
        }

        public Options()
        {
            OrganizationOptions = new Dictionary<string, OrganizationOptions>();
            _CrmSvcUtils = CrmSvcUtilsEditor.GetVersion(_CrmSvcUtils);
        }

        [Category("General")]
        [DisplayName("Coupled Relationships")]
        [Description("Choosing one relationship also chooses the coupled relationship")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool CoupledRelationships { get; set; }

        private string _NuGetFeed;
        [Category("General")]
        [DisplayName("NuGet Feed")]
        [Description("The path of the NuGet feed. You can use a directory or a private NuGet feed.")]
        [DefaultValue(Defaults.NuGetFeed)]
        public string NuGetFeed
        {
            get { return _NuGetFeed ?? Defaults.NuGetFeed; }
            set { _NuGetFeed = string.IsNullOrEmpty(value) ? Defaults.NuGetFeed : value; }
        }

        Version _CrmSvcUtils;

        [Category("General")]
        [DisplayName("Core Tools")]
        [Description("The version of the CRM Service Utility.")]
        [Editor(typeof(CrmSvcUtilsEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public Version CrmSvcUtils
        {
            get { return _CrmSvcUtils; }
            set
            {
                if (_CrmSvcUtils == value) return;
                _CrmSvcUtils = value;
                RaisePropertyChanged(nameof(CrmSvcUtils));
            }
        }

        private OrganizationOptions _CurrentOrganizationOptions;
        [Category("Organization")]
        [DisplayName("Organization Options")]
        [Description("Current organization options")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [XmlIgnore]
        public OrganizationOptions CurrentOrganizationOptions
        {
            get
            {
                return _CurrentOrganizationOptions;
            }
            set
            {
                if (_CurrentOrganizationOptions == value) return;
                _CurrentOrganizationOptions = value;
                RaisePropertyChanged(nameof(CurrentOrganizationOptions));
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [XmlIgnore]
        [Browsable(false)]
        public Dictionary<string, OrganizationOptions> OrganizationOptions { get; set; }
    }
}
