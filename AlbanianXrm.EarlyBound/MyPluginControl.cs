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
using System.Collections.Generic;

namespace AlbanianXrm.EarlyBound
{
    public partial class MyPluginControl : PluginControlBase, IGitHubPlugin
    {
        private TreeViewAdvBeforeCheckEventHandler treeEventHandler;
        private Factories.MyPluginFactory MyPluginFactory;
        private Logic.EntityMetadataHandler EntityMetadataHandler;
        private Logic.AttributeMetadataHandler AttributeMetadataHandler;
        private Logic.RelationshipMetadataHandler RelationshipMetadataHandler;
        private Logic.CoreToolsDownloader CoreToolsDownloader;
        private Logic.EntityGeneratorHandler EntityGeneratorHandler;
        internal Logic.PluginViewModel pluginViewModel;
        internal EntityMetadata[] entityMetadatas = new EntityMetadata[] { };
        internal OptionSetMetadataBase[] optionSetMetadatas = new OptionSetMetadataBase[] { };
        internal Options options;
        internal Queue<WorkAsyncInfo> queue = new Queue<WorkAsyncInfo>();

        public void StartWorkAsync(WorkAsyncInfo work)
        {
            if (!queue.Any())
            {
                pluginViewModel.AllowRequests = false;
                WorkAsync(work);
            }
            queue.Enqueue(work);
        }

        public void WorkAsyncEnded()
        {
            queue.Dequeue();
            if (queue.Any())
            {
                WorkAsync(queue.Peek());
            }
            else
            {
                pluginViewModel.AllowRequests = true;
            }

        }

        public string RepositoryName { get { return "Early-Bound"; } }

        public string UserName { get { return "Albanian-Xrm"; } }

        public MyPluginControl()
        {
            InitializeComponent();
            MyPluginFactory = new Factories.MyPluginFactory();
            AttributeMetadataHandler = MyPluginFactory.NewAttributeMetadataHandler(this);
            CoreToolsDownloader = MyPluginFactory.NewCoreToolsDownloader(this);
            EntityGeneratorHandler = MyPluginFactory.NewEntityGeneratorHandler(this, metadataTree, txtOutput);
            EntityMetadataHandler = MyPluginFactory.NewEntityMetadataHandler(this, metadataTree);
            RelationshipMetadataHandler = MyPluginFactory.NewRelationshipMetadataHandler(this);
            pluginViewModel = MyPluginFactory.NewPluginViewModel();
            treeEventHandler = new TreeViewAdvBeforeCheckEventHandler(this.MetadataTree_BeforeCheck);
            this.metadataTree.BeforeCheck += treeEventHandler;
            btnGenerateEntities.Enabled = pluginViewModel.Generate_Enabled;
            btnGetMetadata.Enabled = pluginViewModel.ActiveConnection;
            DataBind();
        }

        private void DataBind()
        {
            metadataTree.DataBindings.Add(nameof(metadataTree.Enabled), pluginViewModel, nameof(pluginViewModel.MetadataTree_Enabled));
            optionsGrid.DataBindings.Add(nameof(optionsGrid.Enabled), pluginViewModel, nameof(pluginViewModel.OptionsGrid_Enabled));
            toolStrip.DataBindings.Add(nameof(toolStrip.Enabled), pluginViewModel, nameof(pluginViewModel.AllowRequests));
            pluginViewModel.PropertyChanged += PluginViewModel_PropertyChanged;
        }

        private void PluginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(pluginViewModel.Generate_Enabled):
                    btnGenerateEntities.Enabled = pluginViewModel.Generate_Enabled;
                    break;
                case nameof(pluginViewModel.ActiveConnection):
                    btnGetMetadata.Enabled = pluginViewModel.ActiveConnection;
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
                LogWarning("Settings not found => a new settings file has been created!");
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
                LogInfo("Settings found and loaded");
            }
            optionsGrid.SelectedObject = options;
        }

        private void Options_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(options.CrmSvcUtils))
            {
                pluginViewModel.Generate_Enabled = options.CrmSvcUtils != null;
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
            ShowInfoNotification("To contribute to this plugin visit its code repository", new Uri("https://github.com/Albanian-Xrm/Early-Bound"));
            LogInfo("Saving current settings");
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
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
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

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            LogInfo("Saving current settings");
            SettingsManager.Instance.Save(GetType(), options);
        }

        private void MetadataTree_BeforeCheck(object sender, TreeNodeAdvBeforeCheckEventArgs e)
        {
            if (!options.CoupledRelationships) return;
            if (e.Node.Tag is RelationshipMetadataBase)
            {
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
                                    StartWorkAsync(new WorkAsyncInfo
                                    {
                                        Message = $"Getting relationships for entity {entityName}",
                                        Work = (worker, args) =>
                                        {
                                            args.Result = Service.Execute(new RetrieveEntityRequest()
                                            {
                                                EntityFilters = EntityFilters.Relationships,
                                                LogicalName = entityName
                                            });
                                        },
                                        PostWorkCallBack = (args) =>
                                        {
                                            try
                                            {
                                                if (args.Error != null)
                                                {
                                                    MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                                if (args.Result is RetrieveEntityResponse result)
                                                {
                                                    item.ExpandedOnce = true;
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
                                            finally
                                            {
                                                WorkAsyncEnded();
                                            }

                                        }
                                    });
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
                AttributeMetadataHandler.GetAttributes(((EntityMetadata)e.Node.Parent.Tag).LogicalName, e.Node);
            }
            else if (e.Node.Text == "Relationships" && e.NewCheckState == CheckState.Checked && !e.Node.ExpandedOnce)
            {
                RelationshipMetadataHandler.GetRelationships(((EntityMetadata)e.Node.Parent.Tag).LogicalName, e.Node);
            }
        }
    }
}