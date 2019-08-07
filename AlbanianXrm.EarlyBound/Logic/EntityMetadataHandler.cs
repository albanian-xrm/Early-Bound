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
        MyPluginControl myPlugin;
        TreeViewAdv metadataTree;

        public EntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
        }

        public void GetEntityList()
        {
            myPlugin.StartWorkAsync(new WorkAsyncInfo
            {
                Message = "Getting entity list",
                Work = (worker, args) =>
                {
                    var result = myPlugin.Service.Execute(new RetrieveAllEntitiesRequest()
                    {
                        EntityFilters = EntityFilters.Privileges
                    }) as RetrieveAllEntitiesResponse;
                    foreach (var item in result.EntityMetadata)
                    {
                        typeof(EntityMetadata).GetProperty(nameof(item.Attributes)).SetValue(item, new AttributeMetadata[] { });
                        typeof(EntityMetadata).GetProperty(nameof(item.ManyToOneRelationships)).SetValue(item, new OneToManyRelationshipMetadata[] { });
                        typeof(EntityMetadata).GetProperty(nameof(item.OneToManyRelationships)).SetValue(item, new OneToManyRelationshipMetadata[] { });
                        typeof(EntityMetadata).GetProperty(nameof(item.ManyToManyRelationships)).SetValue(item, new ManyToManyRelationshipMetadata[] { });
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
                    catch (Exception ex)
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
