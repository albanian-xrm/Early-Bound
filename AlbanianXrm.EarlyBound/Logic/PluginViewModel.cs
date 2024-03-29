﻿using AlbanianXrm.XrmToolBox.Shared;
using Syncfusion.Windows.Forms.Tools;
using System.Collections.Generic;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class PluginViewModel : ToolViewModelBase
    {
        protected override void AllowRequestsChanged()
        {
            if (_MetadataTree_Enabled && _ActiveConnection) //true means MetadataTree_Enabled changed because AllowRequests changed
            {
                NotifyPropertyChanged(nameof(MetadataTree_Enabled));
            }
            if (_OptionsGrid_Enabled) //true means OptionsGrid_Enabled changed because AllowRequests changed
            {
                NotifyPropertyChanged(nameof(OptionsGrid_Enabled));
            }
            if (_Generate_Enabled)
            {
                NotifyPropertyChanged(nameof(Generate_Enabled));
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
                    NotifyPropertyChanged(nameof(MetadataTree_Enabled));
                }
                NotifyPropertyChanged(nameof(ActiveConnection));
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
                NotifyPropertyChanged(nameof(MetadataTree_Enabled));
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
                NotifyPropertyChanged(nameof(OptionsGrid_Enabled));
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
                NotifyPropertyChanged(nameof(Generate_Enabled));
            }
        }

        private bool _All_Metadata_Requested = false;
        public bool All_Metadata_Requested
        {
            get { return _All_Metadata_Requested; }
            set
            {
                if (_All_Metadata_Requested == value) return;
                _All_Metadata_Requested = value;
                NotifyPropertyChanged(nameof(All_Metadata_Requested));
            }
        }

        private string _LaunchCommand;

        public string LaunchCommand
        {
            get
            {
                return _LaunchCommand;
            }
            set
            {
                _LaunchCommand = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LaunchCommandEnabled));
            }
        }

        public bool LaunchCommandEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(_LaunchCommand);
            }
        }

        internal TreeNodeAdv[] AllEntities = new TreeNodeAdv[] { };
        public readonly Dictionary<string, TreeNodeAdv[]> AllAttributes = new Dictionary<string, TreeNodeAdv[]>();
        public readonly Dictionary<string, TreeNodeAdv[]> AllRelationships = new Dictionary<string, TreeNodeAdv[]>();
    }
}
