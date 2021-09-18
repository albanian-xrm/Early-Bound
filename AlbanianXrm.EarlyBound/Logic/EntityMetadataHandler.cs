using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Helpers;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityMetadataHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly AlBackgroundWorkHandler backgroundWorkHandler;
        private readonly TreeViewAdv metadataTree;
        private readonly EntitySelectionHandler entitySelectionHandler;
        private readonly SfComboBox cmbFindEntity;

        public EntityMetadataHandler(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler, TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler, SfComboBox cmbFindEntity)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
            this.metadataTree = metadataTree;
            this.entitySelectionHandler = entitySelectionHandler;
            this.cmbFindEntity = cmbFindEntity;
        }

        public void GetEntityList(bool selectAll = false)
        {
            var options = this.myPlugin.options;
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewWorker(
                    DoWork,
                    new Tuple<string, bool>((string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : Path.GetFullPath(options.CurrentOrganizationOptions.Output)) + ".alb", selectAll),
                    WorkEnded
                ).WithViewModel(myPlugin.pluginViewModel)
                 .WithMessage(myPlugin, Resources.GETTING_ENTITY_LIST)
            );
        }

        public Tuple<RetrieveAllEntitiesResponse, Dictionary<string, EntitySelection>> DoWork(Tuple<string, bool> arg)
        {
            var result = myPlugin.Service.Execute(new RetrieveAllEntitiesRequest()
            {
                EntityFilters = arg.Item2 ? EntityFilters.All : EntityFilters.Privileges
            }) as RetrieveAllEntitiesResponse;
            if (!arg.Item2)
            {
                foreach (var item in result.EntityMetadata)
                {
                    item.SetPrivateValue(x => x.Attributes, Array.Empty<AttributeMetadata>());
                    item.SetPrivateValue(x => x.ManyToOneRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                    item.SetPrivateValue(x => x.OneToManyRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                    item.SetPrivateValue(x => x.ManyToManyRelationships, Array.Empty<ManyToManyRelationshipMetadata>());
                }
            }
            ForrestSerializer forrestSerializer = new ForrestSerializer(arg.Item1);

            return new Tuple<RetrieveAllEntitiesResponse, Dictionary<string, EntitySelection>>(result, forrestSerializer.Deserialize());
        }

        public void WorkEnded(Tuple<string, bool> input, Tuple<RetrieveAllEntitiesResponse, Dictionary<string, EntitySelection>> value, Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (value != null)
                {
                    RetrieveAllEntitiesResponse result = value.Item1;
                    metadataTree.BackgroundImage = null;
                    metadataTree.Enabled = true;
                    metadataTree.Nodes.Clear();
                    var dataSource = new List<ComboItem>();
                    myPlugin.entityMetadatas = result.EntityMetadata;
                    foreach (var item in result.EntityMetadata.OrderBy(x => x.LogicalName))
                    {
                        if (item.DisplayName.LocalizedLabels.Count == 0) continue;

                        TreeNodeAdv attributes = new TreeNodeAdv("Attributes")
                        {
                            ShowCheckBox = true,
                            InteractiveCheckBox = true
                        };

                        TreeNodeAdv relationships = new TreeNodeAdv("Relationships")
                        {
                            ShowCheckBox = true,
                            InteractiveCheckBox = true
                        };
                        var entityName = $"{item.LogicalName}: {item.DisplayName.LocalizedLabels[0].Label}";
                        dataSource.Add(new ComboItem() { Key = item.LogicalName, Value = entityName });
                        TreeNodeAdv node = new TreeNodeAdv(entityName,
                            new TreeNodeAdv[] { attributes, relationships })
                        {
                            ExpandedOnce = true,
                            ShowCheckBox = true,
                            InteractiveCheckBox = true,
                            Tag = item
                        };
                        metadataTree.Nodes.Add(node);
                        if (input.Item2)
                        {
                            AttributeMetadataHandler.CreateAttributeNodes(attributes, item, checkedState: true);
                            RelationshipMetadataHandler.CreateRelationshipNodes(relationships, item, checkedState: true);
                        }
                    }
                    if (!input.Item2)
                    {
                        entitySelectionHandler.SelectGenerated();
                    }
                    cmbFindEntity.DataSource = dataSource;
                    myPlugin.pluginViewModel.All_Metadata_Requested = input.Item2;
                    myPlugin.pluginViewModel.Generate_Enabled = true;
                }
            }
#pragma warning disable CA1031 // We don't want our plugin to crash because of unhandled exceptions
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
