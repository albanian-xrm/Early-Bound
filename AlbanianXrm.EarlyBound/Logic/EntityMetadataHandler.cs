using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Properties;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityMetadataHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly TreeViewAdv metadataTree;
        private readonly EntitySelectionHandler entitySelectionHandler;

        public EntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
            this.entitySelectionHandler = entitySelectionHandler;
        }

        public void GetEntityList(bool selectAll = false)
        {
            var options = this.myPlugin.options;
            myPlugin.StartWorkAsync(new WorkAsyncInfo
            {
                Message = Resources.GETTING_ENTITY_LIST,
                AsyncArgument = (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : Path.GetFullPath(options.CurrentOrganizationOptions.Output)) + ".alb",
                Work = (worker, args) =>
                {
                    var result = myPlugin.Service.Execute(new RetrieveAllEntitiesRequest()
                    {
                        EntityFilters = selectAll ? EntityFilters.All : EntityFilters.Privileges
                    }) as RetrieveAllEntitiesResponse;
                    if (!selectAll)
                    {
                        foreach (var item in result.EntityMetadata)
                        {
                            item.SetPrivateValue(x => x.Attributes, Array.Empty<AttributeMetadata>());
                            item.SetPrivateValue(x => x.ManyToOneRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                            item.SetPrivateValue(x => x.OneToManyRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                            item.SetPrivateValue(x => x.ManyToManyRelationships, Array.Empty<ManyToManyRelationshipMetadata>());
                        }
                    }
                    ForrestSerializer forrestSerializer = new ForrestSerializer(args.Argument as string);

                    args.Result = new object[] { result, forrestSerializer.Deserialize() };
                },
                PostWorkCallBack = (args) =>
                {
                    try
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (args.Result is object[] arrResult)
                        {
                            RetrieveAllEntitiesResponse result = arrResult[0] as RetrieveAllEntitiesResponse;
                            metadataTree.BackgroundImage = null;
                            metadataTree.Enabled = true;
                            metadataTree.Nodes.Clear();
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

                                TreeNodeAdv node = new TreeNodeAdv($"{item.LogicalName}: {item.DisplayName.LocalizedLabels[0].Label}",
                                    new TreeNodeAdv[] { attributes, relationships })
                                {
                                    ExpandedOnce = true,
                                    ShowCheckBox = true,
                                    InteractiveCheckBox = true,
                                    Tag = item
                                };
                                metadataTree.Nodes.Add(node);
                                if (selectAll)
                                {
                                    AttributeMetadataHandler.CreateAttributeNodes(attributes, item, checkedState: true);
                                    RelationshipMetadataHandler.CreateRelationshipNodes(relationships, item, checkedState: true);
                                }
                            }
                            if (!selectAll)
                            {
                                entitySelectionHandler.SelectGenerated();
                            }

                            myPlugin.pluginViewModel.All_Metadata_Requested = selectAll;
                            myPlugin.pluginViewModel.Generate_Enabled = true;
                        }
                    }
#pragma warning disable CA1031 // We don't want our plugin to crash because of unhandled exceptions
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        myPlugin.WorkAsyncEnded();
                    }
                }
            });
        }
    }
}
