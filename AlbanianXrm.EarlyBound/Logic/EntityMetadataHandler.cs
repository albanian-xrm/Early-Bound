using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Properties;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityMetadataHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly TreeViewAdv metadataTree;

        public EntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
        }

        public void GetEntityList()
        {
            myPlugin.StartWorkAsync(new WorkAsyncInfo
            {
                Message = Resources.GETTING_ENTITY_LIST,
                Work = (worker, args) =>
                {
                    var result = myPlugin.Service.Execute(new RetrieveAllEntitiesRequest()
                    {
                        EntityFilters = EntityFilters.Privileges
                    }) as RetrieveAllEntitiesResponse;
                    foreach (var item in result.EntityMetadata)
                    {
                        item.SetPrivateValue(x => x.Attributes, Array.Empty<AttributeMetadata>());
                        item.SetPrivateValue(x => x.ManyToOneRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                        item.SetPrivateValue(x => x.OneToManyRelationships, Array.Empty<OneToManyRelationshipMetadata>());
                        item.SetPrivateValue(x => x.ManyToManyRelationships, Array.Empty<ManyToManyRelationshipMetadata>());
                    }
                    args.Result = result;
                },
                PostWorkCallBack = (args) =>
                {
                    try
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (args.Result is RetrieveAllEntitiesResponse result)
                        {
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
                            }
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
