using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using XrmToolBox.Extensibility.Interfaces;
using AlbanianXrm.EarlyBound.Properties;
using System.Globalization;
using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Interfaces;
using AlbanianXrm.XrmToolBox.Shared;
using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.XrmToolBox.Shared.Extensions;

namespace AlbanianXrm.EarlyBound
{
    public partial class MyPluginControl : PluginControlBase, IGitHubPlugin
    {
        private TreeViewAdvBeforeCheckEventHandler treeEventHandler;
        private readonly IMyPluginFactory MyPluginFactory;
        private readonly Logic.EntityMetadataHandler EntityMetadataHandler;
        private readonly Logic.AttributeMetadataHandler AttributeMetadataHandler;
        private readonly Logic.RelationshipMetadataHandler RelationshipMetadataHandler;
        private readonly Logic.CoreToolsDownloader CoreToolsDownloader;
        private readonly Logic.EntityGeneratorHandler EntityGeneratorHandler;
        private readonly Logic.EntitySelectionHandler EntitySelectionHandler;
        private readonly Logic.FindEntityHandler FindEntityHandler;
        private readonly AlBackgroundWorkHandler BackgroundWorkHandler;
        internal Logic.PluginViewModel pluginViewModel;
        internal EntityMetadata[] entityMetadatas = Array.Empty<EntityMetadata>();
        internal OptionSetMetadataBase[] optionSetMetadatas = Array.Empty<OptionSetMetadataBase>();
        internal Options options;

        public string RepositoryName { get { return "Early-Bound"; } }

        public string UserName { get { return "Albanian-Xrm"; } }

        public MyPluginControl()
        {
            InitializeComponent();           
            MyPluginFactory = Factories.MyPluginFactory.GetMyPluginFactory(this);
            pluginViewModel = MyPluginFactory.NewPluginViewModel();
            BackgroundWorkHandler = MyPluginFactory.NewBackgroundWorkHandler();
            AttributeMetadataHandler = MyPluginFactory.NewAttributeMetadataHandler();
            CoreToolsDownloader = MyPluginFactory.NewCoreToolsDownloader();
            EntityGeneratorHandler = MyPluginFactory.NewEntityGeneratorHandler(metadataTree, txtOutput);
            RelationshipMetadataHandler = MyPluginFactory.NewRelationshipMetadataHandler();
            EntitySelectionHandler = MyPluginFactory.NewEntitySelectionHandler(metadataTree, AttributeMetadataHandler, RelationshipMetadataHandler);
            EntityMetadataHandler = MyPluginFactory.NewEntityMetadataHandler(metadataTree, EntitySelectionHandler, cmbFindEntity);
            FindEntityHandler = MyPluginFactory.NewFindEntityHandler(metadataTree, cmbFindEntity);
            treeEventHandler = new TreeViewAdvBeforeCheckEventHandler(this.MetadataTree_BeforeCheck);
            this.metadataTree.BeforeCheck += treeEventHandler;
            btnGenerateEntities.Enabled = pluginViewModel.Generate_Enabled;
            mnuSelectGenerated.Visible = pluginViewModel.Generate_Enabled;
            mnuSelectNone.Visible = pluginViewModel.Generate_Enabled;
            btnGetMetadata.Enabled = pluginViewModel.ActiveConnection;
            mnuGetMetadata.Enabled = pluginViewModel.ActiveConnection;
            mnuSelectAll.Enabled = pluginViewModel.ActiveConnection;
            mnuCopyCommand.Enabled = pluginViewModel.LaunchCommandEnabled;
            DataBind();
        }

        private void DataBind()
        {
            metadataTree.Bind(_ => _.Enabled, pluginViewModel, _ => _.MetadataTree_Enabled);
            optionsGrid.Bind(_ => optionsGrid.Enabled, pluginViewModel, _ => _.OptionsGrid_Enabled);
            toolStrip.Bind(_ => _.Enabled, pluginViewModel, _ => _.AllowRequests);
            mnuMetadataTree.Bind(_ => _.Enabled, pluginViewModel, _ => _.AllowRequests);
            pluginViewModel.PropertyChanged += PluginViewModel_PropertyChanged;
        }

        private void PluginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(pluginViewModel.Generate_Enabled):
                    btnGenerateEntities.Enabled = pluginViewModel.Generate_Enabled;
                    mnuSelectGenerated.Visible = pluginViewModel.Generate_Enabled;
                    mnuSelectNone.Visible = pluginViewModel.Generate_Enabled;
                    break;
                case nameof(pluginViewModel.ActiveConnection):
                    btnGetMetadata.Enabled = pluginViewModel.ActiveConnection;
                    mnuGetMetadata.Enabled = pluginViewModel.ActiveConnection;
                    mnuSelectAll.Enabled = pluginViewModel.ActiveConnection;
                    break;
                case nameof(pluginViewModel.All_Metadata_Requested):
                    mnuSelectAll.ToolTipText = pluginViewModel.All_Metadata_Requested ? Resources.SELECT_ALL_NO_REQUEST : Resources.SELECT_ALL_REQUEST;
                    break;
                case nameof(pluginViewModel.LaunchCommandEnabled):
                    mnuCopyCommand.Enabled = pluginViewModel.LaunchCommandEnabled;
                    break;
                default:
                    break;
            }
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            pluginViewModel.ActiveConnection = ConnectionDetail != null;
            var organization = ConnectionDetail?.Organization ?? "";
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out options))
            {
                options = new Options
                {
                    CurrentOrganizationOptions = new OrganizationOptions()
                    {
                        Key = organization
                    }
                };
                options.OrganizationOptions.Add(options.CurrentOrganizationOptions.Key, options.CurrentOrganizationOptions);
                options.PropertyChanged += Options_PropertyChanged;
                LogWarning(Resources.SETTINGS_NOT_FOUND);
            }
            else
            {
                if (!options.OrganizationOptions.TryGetValue(organization, out OrganizationOptions current))
                {
                    current = new OrganizationOptions()
                    {
                        Key = organization
                    };
                    options.OrganizationOptions.Add(current.Key, current);
                }
                options.CurrentOrganizationOptions = current;
                LogInfo(Resources.SETTINGS_FOUND);
            }
            optionsGrid.SelectedObject = options;
        }

        private void Options_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(options.CrmSvcUtils) || e.PropertyName == nameof(options.RecycableMemoryStream))
            {
                pluginViewModel.Generate_Enabled = options.CrmSvcUtils != null && options.RecycableMemoryStream != null;
            }
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            ShowInfoNotification(Resources.CONTRIBUTE_NOTIFICATION, new Uri(Resources.CONTRIBUTE_URI));
            LogInfo(Resources.SAVING_SETTINGS);
            SettingsManager.Instance.Save(GetType(), options);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            if (options != null && detail != null)
            {
                var organization = detail.Organization;
                LogInfo(Resources.CONNECTION_CHANGED, detail.WebApplicationUrl);
                if (!options.OrganizationOptions.TryGetValue(organization, out OrganizationOptions current))
                {
                    current = new OrganizationOptions()
                    {
                        Key = organization
                    };
                    options.OrganizationOptions.Add(current.Key, current);
                }
                options.CurrentOrganizationOptions = current;
            }
            pluginViewModel.ActiveConnection = detail != null;
        }

        private void BtnGetMetadata_Click(object sender, EventArgs e)
        {
            EntityMetadataHandler.GetEntityList();
        }

        private void MnuSelectAll_Click(object sender, EventArgs e)
        {
            if (pluginViewModel.All_Metadata_Requested)
            {
                metadataTree.BeginUpdate();
                foreach (TreeNodeAdv item in metadataTree.Nodes)
                {
                    item.CheckState = CheckState.Checked;
                }
                metadataTree.EndUpdate(update: true);
            }
            else
            {
                EntityMetadataHandler.GetEntityList(selectAll: true);
            }

        }

        private void MnuSelectNone_Click(object sender, EventArgs e)
        {
            metadataTree.BeginUpdate();
            foreach (TreeNodeAdv item in metadataTree.Nodes)
            {
                item.CheckState = CheckState.Unchecked;
            }
            metadataTree.EndUpdate(update: true);
        }

        private void TreeViewAdv1_BeforeExpand(object sender, TreeViewAdvCancelableNodeEventArgs e)
        {
            if (e.Node.ExpandedOnce) return;
            metadataTree.BeginUpdate();
            var metadata = (EntityMetadata)e.Node.Parent.Tag;
            if (e.Node.Text == "Attributes")
            {
                AttributeMetadataHandler.GetAttributes(metadata.LogicalName, e.Node);
            }
            else if (e.Node.Text == "Relationships")
            {
                RelationshipMetadataHandler.GetRelationships(metadata.LogicalName, e.Node);
            }
            metadataTree.EndUpdate(true);
        }

        private void BtnCoreTools_Click(object sender, EventArgs e)
        {
            CoreToolsDownloader.DownloadCoreTools();
        }

        private void BtnGenerateEntities_Click(object sender, EventArgs e)
        {
            EntityGeneratorHandler.GenerateEntities(options);
        }

        private void MnuSaveSettings_Click(object sender, EventArgs e)
        {
            LogInfo(Resources.SAVING_SETTINGS);
            SettingsManager.Instance.Save(GetType(), options);
        }

        private void MetadataTree_BeforeCheck(object sender, TreeNodeAdvBeforeCheckEventArgs e)
        {
            if (e.Node.Tag as RelationshipMetadataBase != null)
            {
                if (!options.CoupledRelationships) return;
                string entity1 = "";
                string entity2 = "";
                string schemaName = "";
                if (e.Node.Tag is OneToManyRelationshipMetadata oneToMany)
                {
                    entity1 = oneToMany.ReferencingEntity;
                    entity2 = oneToMany.ReferencedEntity;
                    schemaName = oneToMany.SchemaName;
                }
                else if (e.Node.Tag is ManyToManyRelationshipMetadata manyToMany)
                {
                    entity1 = manyToMany.Entity1LogicalName;
                    entity2 = manyToMany.Entity2LogicalName;
                    schemaName = manyToMany.SchemaName;
                }

                foreach (TreeNodeAdv entity in metadataTree.Nodes)
                {
                    var entityName = ((EntityMetadata)entity.Tag).LogicalName;
                    if (entityName == entity1 || entityName == entity2)
                    {
                        foreach (TreeNodeAdv item in entity.Nodes)
                        {
                            if (item.Text == "Relationships")
                            {
                                if (!item.ExpandedOnce)
                                {
                                    BackgroundWorkHandler.EnqueueBackgroundWork(
                                        AlBackgroundWorkerFactory.NewWorker<string, RetrieveEntityResponse>(
                                        work: RetrieveRelationships,
                                        argument: entityName,
                                        workFinished: (string argument, RetrieveEntityResponse value, Exception exception) =>
                                        {
                                            try
                                            {
                                                if (exception != null)
                                                {
                                                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                                if (value is RetrieveEntityResponse result)
                                                {
                                                    item.ExpandedOnce = true;
                                                    var entityMetadata = entityMetadatas.FirstOrDefault(x => x.LogicalName == entityName);
                                                    entityMetadata.SetPrivateValue(x => x.ManyToManyRelationships, result.EntityMetadata.ManyToManyRelationships);
                                                    entityMetadata.SetPrivateValue(x => x.OneToManyRelationships, result.EntityMetadata.OneToManyRelationships);
                                                    entityMetadata.SetPrivateValue(x => x.ManyToOneRelationships, result.EntityMetadata.ManyToOneRelationships);

                                                    foreach (var relationship in result.EntityMetadata.ManyToManyRelationships.Union<RelationshipMetadataBase>(
                                                                         result.EntityMetadata.OneToManyRelationships).Union(
                                                                         result.EntityMetadata.ManyToOneRelationships).OrderBy(x => x.SchemaName))
                                                    {
                                                        TreeNodeAdv node = new TreeNodeAdv($"{relationship.SchemaName}")
                                                        {
                                                            ExpandedOnce = true,
                                                            ShowCheckBox = true,
                                                            Tag = relationship
                                                        };
                                                        if (schemaName == relationship.SchemaName)
                                                        {
                                                            node.CheckState = e.NewCheckState;
                                                        }
                                                        item.Nodes.Add(node);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }).WithViewModel(pluginViewModel).WithMessage(this, string.Format(CultureInfo.CurrentCulture, Resources.GETTING_RELATIONSHIPS, entityName)));
                                }
                                else
                                {
                                    foreach (TreeNodeAdv relationship in item.Nodes)
                                    {
                                        if (relationship == e.Node) continue;
                                        if (((RelationshipMetadataBase)relationship.Tag).SchemaName == schemaName)
                                        {
                                            this.metadataTree.BeforeCheck -= treeEventHandler;
                                            relationship.CheckState = e.NewCheckState;
                                            treeEventHandler = new TreeViewAdvBeforeCheckEventHandler(this.MetadataTree_BeforeCheck);
                                            this.metadataTree.BeforeCheck += treeEventHandler;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (e.Node.Text == "Attributes" && e.NewCheckState == CheckState.Checked && !e.Node.ExpandedOnce)
            {
                AttributeMetadataHandler.GetAttributes(((EntityMetadata)e.Node.Parent.Tag).LogicalName, e.Node, checkedState: true);
            }
            else if (e.Node.Text == "Relationships" && e.NewCheckState == CheckState.Checked && !e.Node.ExpandedOnce)
            {
                RelationshipMetadataHandler.GetRelationships(((EntityMetadata)e.Node.Parent.Tag).LogicalName, e.Node, checkedState: true);
            }
        }

        private RetrieveEntityResponse RetrieveRelationships(string entity)
        {
            return Service.Execute(new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = entity
            }) as RetrieveEntityResponse;
        }

        private void MnuSelectGenerated_Click(object sender, EventArgs e)
        {
            MnuSelectNone_Click(sender, e);
            EntitySelectionHandler.SelectGenerated();
        }

        private void MnuCopyCommand_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(pluginViewModel.LaunchCommand);
        }
    }
}