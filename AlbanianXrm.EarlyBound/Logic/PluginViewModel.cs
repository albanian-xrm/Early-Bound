using System.ComponentModel;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class PluginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _AllowRequests = true;
        public bool AllowRequests
        {
            get { return _AllowRequests; }
            set
            {
                if (_AllowRequests == value) return;
                _AllowRequests = value;
                if (_MetadataTree_Enabled && _ActiveConnection) //true means MetadataTree_Enabled changed because AllowRequests changed
                {
                    RaisePropertyChanged(nameof(MetadataTree_Enabled));
                }
                if (_OptionsGrid_Enabled) //true means OptionsGrid_Enabled changed because AllowRequests changed
                {
                    RaisePropertyChanged(nameof(OptionsGrid_Enabled));
                }
                if (_Generate_Enabled)
                {
                    RaisePropertyChanged(nameof(Generate_Enabled));
                }
                RaisePropertyChanged(nameof(AllowRequests));
            }
        }

        private bool _ActiveConnection = false;
        public bool ActiveConnection
        {
            get { return _ActiveConnection; }
            set
            {
                if (_ActiveConnection == value) return;
                _ActiveConnection = value;
                if (_MetadataTree_Enabled && _AllowRequests) //true means MetadataTree_Enabled changed because ActiveConnection changed
                {
                    RaisePropertyChanged(nameof(MetadataTree_Enabled));
                }
                RaisePropertyChanged(nameof(ActiveConnection));
            }
        }

        private bool _MetadataTree_Enabled = true;
        public bool MetadataTree_Enabled
        {
            get { return _MetadataTree_Enabled && _ActiveConnection && _AllowRequests; }
            set
            {
                if (_MetadataTree_Enabled == value) return;
                _MetadataTree_Enabled = value;
                RaisePropertyChanged(nameof(MetadataTree_Enabled));
            }
        }

        private bool _OptionsGrid_Enabled = true;
        public bool OptionsGrid_Enabled
        {
            get { return _OptionsGrid_Enabled && _AllowRequests; }
            set
            {
                if (_OptionsGrid_Enabled == value) return;
                _OptionsGrid_Enabled = value;
                RaisePropertyChanged(nameof(OptionsGrid_Enabled));
            }
        }

        private bool _Generate_Enabled = false;
        public bool Generate_Enabled
        {
            get { return _Generate_Enabled && _AllowRequests; }
            set
            {
                if (_Generate_Enabled == value) return;
                _Generate_Enabled = value;
                RaisePropertyChanged(nameof(Generate_Enabled));
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
