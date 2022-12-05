using AlbanianXrm.EarlyBound.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace AlbanianXrm.EarlyBound
{
    public class Options : INotifyPropertyChanged
    {
        public Options()
        {
            OrganizationOptions = new Dictionary<string, OrganizationOptions>();
            _ModelBuilder = new ModelBuilderVersionEditor().GetVersion(_ModelBuilder);
            _BtnGetCLIVisible = string.IsNullOrEmpty(_ModelBuilder);
            _RecycableMemoryStream = new MemoryStreamEditor().GetVersion(_RecycableMemoryStream);
            _CrmSvcUtilExtensions = new CrmSvcUtilExtensionsEditor().GetVersion(_CrmSvcUtilExtensions);
            CacheMetadata = true;
        }

        [Category("General")]
        [DisplayName("Coupled Relationships")]
        [Description("Choosing one relationship also chooses the coupled relationship")]
        [DefaultValue(false)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool CoupledRelationships { get; set; }

        [Category("General")]
        [DisplayName("Cache Metadata")]
        [Description("Use the cached metadata during the code generation.")]
        [DefaultValue(true)]
        [TypeConverter(typeof(YesNoConverter))]
        public bool CacheMetadata { get; set; }


        private bool _BtnGetCLIVisible;

        [XmlIgnore]
        public bool BtnGetCLIVisible
        {
            get { return _BtnGetCLIVisible; }
        }

        string _ModelBuilder;
        [Category("Version")]
        [DisplayName("ModelBuilder")]
        [Description("The version of the Power Platform Tools CLI.")]
        [Editor(typeof(ModelBuilderVersionEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public string ModelBuilder
        {
            get { return _ModelBuilder; }
            set
            {
                if (_ModelBuilder == value) return;
                _ModelBuilder = value;
                _BtnGetCLIVisible = string.IsNullOrEmpty(value);
                NotifyPropertyChanged(nameof(BtnGetCLIVisible));
                NotifyPropertyChanged();
            }
        }

        string _RecycableMemoryStream;

        [Category("Version")]
        [DisplayName("Recyclable Memory Stream")]
        [Description("The version of the Recyclable Memory Stream.")]
        [Editor(typeof(MemoryStreamEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public string RecycableMemoryStream
        {
            get { return _RecycableMemoryStream; }
            set
            {
                if (_RecycableMemoryStream == value) return;
                _RecycableMemoryStream = value;
                NotifyPropertyChanged();
            }
        }

        string _CrmSvcUtilExtensions;

        [Category("Version")]
        [DisplayName("AlbanianXrm.CrmSvcUtilExtensions")]
        [Description("The version of the AlbanianXrm.CrmSvcUtilExtensions.")]
        [Editor(typeof(CrmSvcUtilExtensionsEditor), typeof(UITypeEditor))]
        [XmlIgnore]
        public string CrmSvcUtilExtensions
        {
            get { return _CrmSvcUtilExtensions; }
            set
            {
                if (_CrmSvcUtilExtensions == value) return;
                _CrmSvcUtilExtensions = value;
                NotifyPropertyChanged();
            }
        }

        [Category("Version")]
        [DisplayName("This Plugin")]
        [Description("The version of Albanian Early Bound.")]
        [XmlIgnore]
        public Version AlbanianEarlyBound
        {

            get { return GetType().Assembly.GetName().Version; }
        }

#if DEBUG
        [Category("Debug")]
        [DisplayName("Launch Debugger")]
        [Description("Launch the debugger in certain instants of the lifetime of CrmSvcUtils.")]
        [XmlIgnore]
        public bool LaunchDebugger { get; set; }
        [Category("Debug")]
        [DisplayName("Verbose Logging")]
        [Description("Write verbose logging in certain instants of the lifetime of CrmSvcUtils.")]
        [XmlIgnore]
        public bool VerboseLogging { get; set; }
#endif

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
                NotifyPropertyChanged();
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
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [XmlIgnore]
        [Browsable(false)]
        public Dictionary<string, OrganizationOptions> OrganizationOptions { get; private set; }
    }
}
